using neah.entetys;
using neah.main;
using System;
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
                        Task foodtask = tasks[i];
                        tasks.RemoveAt(i);
                        ant.clamedtaskid = foodtask.id;
                        return foodtask;
                    }
                }
            }
            else
            {
                Task nexttask = tasks[0];
                tasks.RemoveAt(0);
                ant.clamedtaskid = nexttask.id;
                return nexttask;

            }
            throw new Exception("taks isnt working");


        }
        
    }
    // add priority queue for ant tasks but priorities should change dynamicaly based off average ant food levels 
}
