using neah.algorithm;
using neah.main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah.entetys
{
    public abstract class Ant : Entity
    {
        public Ant(int id, char Species) : base(id, Species)
        { 

        }
        public int clamedtaskid { get; set; } = -1;
        public task Currenttask { get; set; }
        // -1 means no class claimed
        // add stuff for queue so once ant has clamed a task it wont claim another till done
        public override int Id { get; set; }
        public override char Species { get; set; } = 'N';
        public override string Symbol { get; set; } = "[N]";

        public virtual int food { get; set; } = 0;
        public virtual int maxfood { get; set; } = 5;

        public virtual int carryingcapacity { get; set; } = 2;
        public virtual List<int> inventory { get; set; } = new List<int>();
        

        public virtual List<string> path { get; set; } = new List<string>();
        /*public virtual void Pathfind()
        {
            return inventory.Count > 0;
        }*/

        

    }
}
