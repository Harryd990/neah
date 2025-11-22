using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public class Ant : Entity
    {
        public Ant(int id, char Species) : base(id, Species)
        { 

        }
        
        public override int Id { get; set; }
        public override char Species { get; set; } = 'A';
        public override string Symbol { get; set; } = "[A]";

        public int food { get; set; } = 0;
        public int maxfood { get; set; } = 5;

        public int carryingcapacity { get; set; } = 2;
        public List<int> inventory { get; set; } = new List<int>();

        

    }
}
