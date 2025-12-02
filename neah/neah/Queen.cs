using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace neah
{
    internal class Queen : Ant 
    {
        public Queen(int id, char Species) : base(id, Species)
        {

        }
        public override char Species { get; set; } = 'Q';
        public override string Symbol { get; set; } = "[Q]";

        // makes new egg at queen position
        public void LayEggs(Game game)
        {
            int y = Position.Item1;
            int x = Position.Item2;

            var egg = new Worker(game.lastEntityId++, 'A');
            game.AddEntityToGameGrid(x, y, egg);

        }
    }
}
