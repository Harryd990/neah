using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Is_air { get; set; }

        public List<Entity> Entities { get; } = new List<Entity>();

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Is_air = true;
            Entities = new List<Entity>();
        }

        public void AddEntity (Entity entity)
        {
            Entities.Add(entity);
        }
        
    }
}
    
