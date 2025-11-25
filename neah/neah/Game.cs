using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public class Game
    {
        private Grid grid;
        
        public Game(int width, int height)
        {
            grid = new Grid(width, height);
        }
        public int lastEntityId { get; set; } = 0;
        public void Initialize_Ants()
        {
            Random rand = new Random();
            
            for (int i =0; i <= 3;i++)
            {
                
                Entity ant = new Ant(i, 'A');
                
                int x = rand.Next(0, grid.width);
                int y = rand.Next(0, grid.height);
                var cell = grid.GetCellAtLocation(x, y);
                if (!Cell.IsCellType(cell, typeof(Air)))
                {
                    lastEntityId--;
                    continue;
                }

                else
                {
                    grid.AddEntityToCellLocation(x, y, ant);
                    lastEntityId++;
                }
                    
            }
            
        }
        public void Run()
        {
            bool running = true;

            while (running)
            {
                
                grid.PrintGrid();
                var input = Console.ReadKey(true);
                
                Thread.Sleep(100);
            }
        }
        public void AddEntityToGameGrid(int x, int y, Entity entity)
        {
            grid.AddEntityToCellLocation(x, y, entity);
        }
    }
}
