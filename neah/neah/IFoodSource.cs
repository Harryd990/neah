using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal interface IFoodSource
    {
        (int, int) Position { get; }
        int AvailableFood { get; }
        int TakeFood(int amount); // Returns actual amount taken 
    }
}
