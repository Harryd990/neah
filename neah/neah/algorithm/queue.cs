using neah.main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah.algorithm
{
    internal class queue
    {
        public queue()
        {
        }
        
        public List<task> tasks = new List<task>();
        public int lasttaskid = 0;
        public void addtask(task newtask)
        {
            tasks.Add(newtask);
        }
        public void removetask(int taskid)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].id == taskid)
                {
                    tasks.RemoveAt(i);
                }
            }
        }
        public task getnexttask(Game game)
        {
            if (tasks.Count == 0)
            {
                throw new Exception("the wander func isnt working");
            }
            if (game.QueenFoodCount <1)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].tasktype == "gatherfood")
                    {
                        task foodtask = tasks[i];
                        tasks.RemoveAt(i);
                        return foodtask;
                    }
                }
            }
            else
            {
                task nexttask = tasks[0];
                tasks.RemoveAt(0);
                return nexttask;

            }
            throw new Exception("taks isnt working");


        }
        public void checkwander(Game game)
        {
            if (tasks.Count < game.lastEntityId)
            {
                Random rand = new Random();
                int x = rand.Next (game.GridWidth);
                int y = rand.Next(0, game.GridHeight / 4);
                addtask(new task(lasttaskid + 1, "wander", (x, y)));
            }
        }
    }
    // add priority queue for ant tasks but priorities should change dynamicaly based off average ant food levels 
}
