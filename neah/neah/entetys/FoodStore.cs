using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using neah.entetys;

namespace neah.entetys
{
   
        internal class FoodStore : Entity, IFoodSource
    {
            public FoodStore(int id, char Species) : base(id, Species)
            {

            }
        public int foodcontained;
        public int capacity;
        public override (int, int) Position { get; set; }
        public int AvailableFood => foodcontained;

        public int TakeFood(int amount)
        {
            int taken = Math.Min(foodcontained, amount);
            foodcontained -= taken;
            return taken;
        }

        public override int Id { get; set; }
        // amount of food stored 
            

        // symbol and species for this entity
        public override char Species { get; set; } = 'S';
            public override string Symbol { get; set; } = "[S]";

        

        public void addfood(Ant ant)
        {
            int toAdd = Math.Min(ant.foodcarried, capacity - foodcontained);
            foodcontained += toAdd;
            ant.foodcarried -= toAdd;
            Console.WriteLine($"FoodStore at {Position} received {toAdd} food. Now: {foodcontained}/{capacity}");
        }
        
        public void removefood(Ant ant)
            {
                if (foodcontained - ant.foodcarried < 0)
                {
                    foodcontained = 0;
                }
                else
                {
                    foodcontained -= ant.foodcarried;
                }
        }
        // add stuff to make it so food stores can only be made underground + farms too 


    }
}

