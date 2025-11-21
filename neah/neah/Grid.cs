using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public class Grid
    {
        public int Width {get;}
        public int Height {get;}
        private string[,]cells;
        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new string[width, height];


        }
        public string GetCell(int x, int y)
        {
            return cells[x, y];
        }

        public void SetCell(int x, int y, string value)
        {
            cells[x, y] = value;
        }
        public void ClearCell(int x, int y)
        {
            cells[x, y] = null;
        }
        public void ClearGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cells[x, y] = null;
                }
            }
        }
        public void PrintGrid()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(cells[x, y]);
                }
                Console.WriteLine();
            }
        }
        public void innitializeGrid(string defaultValue)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cells[x, y] = defaultValue;
                    
                }
            }
        }

    }
}
