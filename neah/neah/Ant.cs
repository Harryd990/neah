using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public abstract class Ant : Entity
    {
        public Ant(int id, char Species) : base(id, Species)
        { 

        }
        
        public override int Id { get; set; }
        public override char Species { get; set; } = 'N';
        public override string Symbol { get; set; } = "[N]";

        public virtual int food { get; set; } = 0;
        public virtual int maxfood { get; set; } = 5;

        public virtual int carryingcapacity { get; set; } = 2;
        public virtual List<int> inventory { get; set; } = new List<int>();

        

    }
}
