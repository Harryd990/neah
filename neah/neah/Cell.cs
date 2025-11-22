using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    public abstract class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public abstract bool IsTraversable { get; }
        public abstract string Symbol { get; }


        public List<Entity> Entities { get; } = new List<Entity>();

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            
            Entities = new List<Entity>();
        }

        public void AddEntity (Entity entity)
        {
            Entities.Add(entity);
        }
        
    }
}
    
