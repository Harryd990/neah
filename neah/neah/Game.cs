using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public class Game
    {
        private Grid grid;
        private Queen queen;

        public Game(int width, int height)
        {
            grid = new Grid(width, height);
            
        }
        public int tick { get; set; } = 0;
        public int lastEntityId { get; set; } = 0;
        public void Initialize_Ants()
        {
            Random rand = new Random();
            queen = new Queen(0, 'Q');
            AddEntityToGameGrid(grid.width / 2,grid.height / 4 -1, queen);

            for (int i =0; i <= 3;i++)
            {
                
                Entity ant = new Ant(i, 'A');
                

                int x = rand.Next(0, grid.width);
                
                var cell = grid.GetCellAtLocation(x, grid.height / 4 - 1);
                
                
                AddEntityToGameGrid(x, grid.height / 4 -1, ant);
                lastEntityId++;
                
                    
            }
            
        }
        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                grid.PrintGrid();
                var input = Console.ReadKey(true);
                tick++;
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
    }
}
