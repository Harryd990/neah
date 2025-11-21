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
    }
}
