using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal class Dirt : Cell
    {
        public Dirt(int x, int y) : base(x, y)
        {
        }
        public int digprogress { get; set; } = 0;
        public override string Symbol => "@";
        public override bool IsTraversable => false;
    }
}

