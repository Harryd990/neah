using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public class Symulation
    {
        private Grid grid; 
        public Symulation(int width, int height)
        {
            grid = new Grid(width, height);
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
    }
}
