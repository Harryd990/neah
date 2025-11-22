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
            for (int y=0; y<w; y++)
            {
                for (int x=0; x<h; x++)
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
                output += $"{e.Id}\n{e.Name}";
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

    }
}

