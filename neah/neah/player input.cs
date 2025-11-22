using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal class player_input
    {
        public void GetInput()
        {
            var inputKey = Console.ReadKey(true);
        }
        public void doInput(ConsoleKeyInfo key)
        {
            switch(key.Key)
            {
                case ConsoleKey.W:
                    
                    break;
                case ConsoleKey.A:
                    
                    break;
                case ConsoleKey.S:
                    
                    break;
                case ConsoleKey.D:
                    
                    break;
            }
        }
        public void processInput()
        {

        }

    }
}
