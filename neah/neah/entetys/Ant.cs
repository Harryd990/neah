using neah.algorithm;
using neah.main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
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
        

        public virtual List<int> path { get; set; } = new List<int>();
        public virtual void Move(int direction)
        {
            switch (direction)
            {
                case 1:
                    Position = (Position.Item1, Position.Item2 - 1);
                    break;
                case 2:
                    Position = (Position.Item1, Position.Item2 + 1);
                    break;
                case 3:
                    Position = (Position.Item1 - 1, Position.Item2);
                    break;
                case 4:
                    Position = (Position.Item1 + 1, Position.Item2);
                    break;
                default:
                    throw new InvalidOperationException("Invalid direction");
            }
        }
        



    }
}
