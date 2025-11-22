using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public class Air : Cell
    {
        public Air(int x, int y) : base(x, y)
        {

        }
        

        public override bool IsTraversable => true;
        public override string Symbol => Entities.Count > 0
            ? $"[{Entities[0].Species}]"
            : "[ ]";
    }

}
    

