using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal class Worker : Ant
    {
        public Worker(int id, char Species) : base(id, Species)
        {

        }
        public override char Species { get; set; } = 'A';
        public override string Symbol { get; set; } = "[A]";

        public void GatherFood(Game game)
        {
            // Implementation for gathering food
        }
        public void dig(Game game)
        {
            // Implementation for digging
        }
    }
}
