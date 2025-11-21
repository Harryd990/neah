using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal class Cell
    {
        public int X { get; }
        public int Y { get; }
        public bool Is_air { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Is_air = true;
        }
    }
}
    
