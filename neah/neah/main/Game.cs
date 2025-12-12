using neah.algorithm;
using neah.entetys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace neah.main
{
    public class Game
    {
        private Grid grid;
        private Queen queen;

        public Game(int width, int height)
        {
            grid = new Grid(width, height);

        }
        public int workercount { get; set; } = 0;
        public int GridWidth => grid.width;
        public int GridHeight => grid.height;
        public int tick { get; set; } = 0;
        public int lastEntityId { get; set; } = 0;
        public int QueenFoodCount => queen?.food ?? 0;
        public void Initialize_Game()
        {
            // adds queen to centre of grid on the first bit of air 
            Random rand = new Random();
            queen = new Queen(0, 'Q');
            AddEntityToGameGrid(grid.width / 2, grid.height / 4 - 1, queen);

            for (int i = 0; i <= 3; i++)
            {
                // adds 4 workers along the first line of air
                Entity Worker = new Worker(i, 'A');


                int x = rand.Next(0, grid.width);

                var cell = grid.GetCellAtLocation(x, grid.height / 4 - 1);


                AddEntityToGameGrid(x, grid.height / 4 - 1, Worker);
                lastEntityId++;
                workercount++;


            }
            for (int i = 0; i <= 1; i++)
            {
                // adds 2 bits of food in the air zone 
                Entity food = new Food(lastEntityId++, 0, 0);
                int x = rand.Next(0, grid.width);
                int y = rand.Next(0, grid.height / 4);
                AddEntityToGameGrid(x, y, food);
                lastEntityId++;
            }

        }
        public void ReplaceCellAtLocation(int x, int y, Cell newCell)
        {
            grid.ReplaceCellAtLocation(x, y, newCell);
        }
        public void Run()
        {
            bool running = true;
            grid.PrintGrid();
            while (running)
            {

                inputselector();

                // if queen has 4 food lay eggs 
                if (queen != null && queen.food == 4)
                {
                    queen.food = 0;
                    queen.LayEggs(this);

                }


                Thread.Sleep(100);
            }
        }

        public void AddEntityToGameGrid(int x, int y, Entity entity)
        {
            grid.AddEntityToCellLocation(x, y, entity);
        }
        
        public void inputselector()
        {
            Console.WriteLine("1 : order the ants to dig \nanything else : end tick ");
            var input = Console.ReadKey(true);
            if (input.KeyChar == '1')
            {
                Console.WriteLine("please enter the x and y position of the thing you want to dig (x cord then enter y cord then entre)");
                int x = int.Parse(Console.ReadLine());
                int y = int.Parse(Console.ReadLine());
                dig(x, y);
            }
            else
            {
                Console.Clear();
                grid.PrintGrid();
                tick++;

            }

        }
        public void dig(int x, int y)
        {
            if (grid.GetCellAtLocation(x,y) is Dirt)
            {
                Dirt dirtcell = (Dirt)grid.GetCellAtLocation(x, y);
                dirtcell.digprogress++;
                if (dirtcell.digprogress >= dirtcell.hardness)
                {
                    // replace with air cell
                    ReplaceCellAtLocation(x, y, new Air(x, y));
                    Console.Clear();
                    printgrid();
                    
                }

            }
            else
            {
               throw new InvalidOperationException("Cell is not dirt");
            }


        }
        public void printgrid()
        {
            grid.PrintGrid();
        }
        public void antwander(queue queue, Ant ant )
        {
            if (queue.tasks.Count == 0 && ant.clamedtaskid == -1)
            {
                Random rand = new Random();
                int x = rand.Next(GridWidth);
                int y = rand.Next(0, GridHeight / 4);
                task wander = new task(queue.lasttaskid++, "wander", (x, y));
                ant.Currenttask = wander;
                ant .clamedtaskid = -1;
            }
        }
        /*
         * Algorithm :
         * find closest ant to end position that doesnt already have a task 
         * work out path to end position
         * send and to end position 
         * 
         */
    }
}
