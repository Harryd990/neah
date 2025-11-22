using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal class Grid
    {
        // properties
        public int width {  get; private set; }
        public int height { get; private set; }
        private Cell[,] cells;
        
        public Grid(int w, int h) 
        { 
            width = w;
            height = h;
            //create array of empty slots for cells.
            cells = new Cell[w, h];

            // create the cell objects in each slot
            for (int x=0; x<w; x++)
            {
                for (int y=0; y<h/4; y++)
                {
                    cells[x, y] = new Air(x, y);
                }
                for (int y=h/4; y<h; y++)
                {
                    cells[x, y] = new Dirt(x, y);
                }
            }
        }

        public Cell GetCellAtLocation(int x, int y)
        {
            if (IsInGridRange(x,y)) return cells[x, y];
            else throw new InvalidOperationException("Not in range");
                
        }
        public void AddEntityToCellLocation(int x, int y, Entity thing)
        {
   
            cells[x,y].AddEntity(thing);
        }
        public bool IsInGridRange(int x, int y)
        {
            //check if x,y is in range
            return !((x < 0 || y < 0) || (x >= width || y >= height));
   
        }

        public string GetCellDetails(int x, int y)
        {
            string output = $"Cell {x},{y}\nCell Contents:\n";
            foreach (Entity e in GetCellAtLocation(x, y).Entities)
            {
                output += $"{e.Id}\n{e.Species}";
            }
            return output;
        }
        public void PrintGrid()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(cells[x, y].Symbol);
                }
                Console.WriteLine();
            }
        }
        public void ConvertToAir(int x, int y)
        {
            cells[x, y] = new Air(x, y);
        }

    }
}

