using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah.entetys
{
    internal class Food : Entity
    {
        public Food(int id, int x, int y) : base(id, 'F')
        {
            Random random = new Random();

            // sets food range between 25 50 100
            Foodrange = (25, random.Next(1000, 10000));
            currentAmount = random.Next(Foodrange.Item1, Foodrange.Item2 + 1);

            Id = Id;
        }
        
        public override int Id { get; set; }
        public override char Species { get; set; } = 'F';
        public override string Symbol { get; set; } = "[F]";
        public int virtFoodContained { get; set; } = 0;

        public (int, int) Foodrange { get; set; }
        public int currentAmount { get; set; }
    }

    
    
}
