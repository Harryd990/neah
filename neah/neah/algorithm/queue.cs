using neah.entetys;
using neah.main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neah.algorithm
{
    public class queue
    {
        public queue()
        {
        }
        
        public List<Task> tasks = new List<Task>();
        public int lasttaskid = 0;
        public void addtask(Task newtask)
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
        public Task getnexttask(Game game, Ant ant)
        {
            /*
            if (tasks.Count == 0)
            {
                throw new Exception("the wander func isnt working");
            }
            // could remove the queen stuff cos kinda useless w this implementation
            if (game.QueenFoodCount <40)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].tasktype == "gatherfood")
                    {
                        Task foodtask = tasks[i];
                        tasks.RemoveAt(i);
                        ant.clamedtaskid = foodtask.id;
                        return foodtask;
                    }
                }
            }
            this is redundent stuff can be removed but this is the place to add priority based tasking 
            */
            if (tasks.Count > 0)
            {
                Task nexttask = tasks[0];
                tasks.RemoveAt(0);
                ant.clamedtaskid = nexttask.id;
                return nexttask;
            }
            else
            {
                Random rand = new Random();
                int x = rand.Next(game.GridWidth);
                int y = rand.Next(0, game.GridHeight / 4);
                algorithm.Task wander = new algorithm.Task(lasttaskid++, "wander", (x, y));
                return wander;
            }
            

                

            
           


        }
        
    }
    // add priority queue for ant tasks but priorities should change dynamicaly based off average ant food levels 
}
