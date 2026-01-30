using neah.algorithm;
using neah.entetys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace neah.main
{
    /*
     * to do:
     * make so ants add food to food stores
     * make so food stores can onlu be made undegroud
     * add farms (only underground) that slowly generate food over time
     * queen only gives birth underground
     * move all starting yap into game initialize ( cos looks cooler)
     * add auto ticking and speed dial 
     * */
    /*
     * adds add food to food stores before wander task
     * */
    public class Game
    {
        private Grid grid;
        private Queen queen;
        public queue queue1 = new queue();

        public Game(int width, int height)
        {
            grid = new Grid(width, height);
            queue1 = new queue();
        }
        public int workercount { get; set; } = 0;
        public int GridWidth => grid.width;
        public int GridHeight => grid.height;
        public int tick { get; set; } = 0;
        public int lastEntityId { get; set; } = 0;
        public int QueenFoodCount => queen?.food ?? 0;
        public void Initialise_Game()
        {
            // adds queen to centre of grid on the first bit of air 
            Random rand = new Random();
            queen = new Queen(0, 'Q');
            AddEntityToGameGrid(grid.width / 2, grid.height / 4 - 1, queen);

            for (int i = 0; i <= 3; i++)
            {
                // adds 4 workers along the first line of air
                Entity Worker = new Worker(i, 'A'); 
                


                int x = rand.Next(0, grid.width);

                var cell = grid.GetCellAtLocation(x, grid.height / 4 - 1);


                AddEntityToGameGrid(x, grid.height / 4 - 1, Worker);
                
                lastEntityId++;
                workercount++;


            }
            for (int i = 0; i <= 1; i++)
            {
                // adds 2 bits of food in the air zone 
                Entity food = new Food(lastEntityId++, 0, 0);
                int x = rand.Next(0, grid.width);
                int y = rand.Next(0, grid.height / 4);
                AddEntityToGameGrid(x, y, food);
                lastEntityId++;
            }

        }
        // add print all tasks in queue for debuging
        public void PrintAllTasksInQueue()
        {
            Console.WriteLine("Current Tasks in Queue:");
            foreach (var task in queue1.tasks)
            {
                Console.WriteLine($"Task ID: {task.id}, Type: {task.tasktype}, Target Position: ({task.targetposition.Item1}, {task.targetposition.Item2})");
            }
        }
        public void UgentHungerCheck() {List<Ant> ants = GetAllAnts();
            foreach (var ant in ants)
            {
                if ((ant.food <= 40 && ant.clamedtaskid == -1) )
                {
                    Console.WriteLine( "a ant hungers");
                    ClosestFoodWtaskadd(ant);
                }
                
                if(ant.food <= 20 && ant.clamedtaskid != -1 && ant.Currenttask.tasktype != "gatherfood")
                {
                    Console.WriteLine("a ant hungers");
                    // add current task back to queue
                    if (ant.Currenttask != null)
                    {
                        queue1.addtask(ant.Currenttask);
                    }
                    ClosestFoodWtaskadd(ant);
                }
            }
        }
        public void ReplaceCellAtLocation(int x, int y, Cell newCell)
        {
            grid.ReplaceCellAtLocation(x, y, newCell);
        }
        public void Run()
        {
            bool running = true;
            Thread.Sleep(20); 

            grid.PrintGrid();
            while (running)
            {
                HungerAnts();
                // PrintAllTasksInQueue(); use for debuging
                UgentHungerCheck();

                inputselector();
                ProcessAntMovementAndTasks();
                
                GeneralTickUpdates();
                                






                // if queen has  food lay eggs 
                // add stuff so queen has grace period on egg laying 
                // mb later add queen preference to lay eggs underground cos currently spams eggs on the food source and guzzels it all 
                if (queen != null && queen.food >= 60 && queen.EggGracePeriod <= 0)
                {
                    //throw new Exception("Queen is laying eggs");
                    queen.food = queen.food-30;
                    queen.LayEggs(this);
                    // add stuff here for queen moving underground / hiding to lay eggs 
                    queen.EggGracePeriod = 30;

                }


                Thread.Sleep(10);
            }
        }
        public void GeneralTickUpdates()
        {
            queen.EggGracePeriod--;
            ProcessEggHatching();
            Check4EmptyStores();
            // check if there is a food store that isnt full if so add a food store fill task to quue
        }
        public void Check4EmptyStores()
        {
            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    var cell = grid.GetCellAtLocation(x, y);
                    var foodStores = cell.Entities.OfType<FoodStore>().ToList();
                    foreach (var store in foodStores)
                    {
                        if (store.foodcontained < store.capacity)
                        {
                            // add task to queue to fill this store
                            algorithm.Task foodstoretask = new algorithm.Task(queue1.lasttaskid++, "foodstoregather", (x, y));
                            queue1.addtask(foodstoretask);
                        }
                    }
                }
            }
        }
        public void AddFoodToFoodStore(Ant ant)
        {
            var cell = grid.GetCellAtLocation(ant.Position.Item1, ant.Position.Item2);
            var foodStores = cell.Entities.OfType<FoodStore>().ToList();
            foreach (var store in foodStores)
            {
                store.addfood(ant);
                ant.foodcarried = 0;
                return;
            }

        }
        // egg hatch add up every tick untill = hatch time then add worker to grid at egg pos and remove egg from grid
        public void ProcessEggHatching()
        {
            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    var cell = grid.GetCellAtLocation(x, y);
                    var eggs = cell.Entities.OfType<Egg>().ToList();
                    foreach (var egg in eggs)
                    {
                        egg.hatchTime--;
                        if (egg.hatchTime <= 0)
                        {
                            egg.HatchEgg(this);
                            cell.RemoveEntity(egg);
                            Console.WriteLine($"An egg has hatched at ({x},{y})");
                        }
                    }
                }
            }
        }
        private void ProcessAntMovementAndTasks()
        {
            // 1) Assign queued tasks to the closest available ant using ClosestAnt()
            // Make a copy so we can safely remove tasks from the queue while iterating.
            var pending = queue1.tasks.ToList();
            foreach (var task in pending)
            {
                try
                {
                    // ClosestAnt will set the ant's Currenttask and clamedtaskid if a free ant exists.
                    ClosestAnt(task);
                    // If assignment succeeded, remove the task from the queue.
                    queue1.removetask(task.id);
                }
                catch
                {
                    // couldn't assign this task now (no free ants or other error) — leave it in the queue.
                }
            }

            // 2) Move ants along their paths and let idle ants wander
            var ants = GetAllAnts();
            foreach (var ant in ants)
            {
                if (ant.Currenttask != null)
                {
                    var task = ant.Currenttask;
                    var tx = task.targetposition.Item1;
                    var ty = task.targetposition.Item2;

                    // If this is a dig task and the ant is already adjacent (or on) the target,
                    // do NOT attempt to pathfind — let the ant perform dig work each tick until finished.
                    bool isDigAndAdjacent = task.tasktype.Equals("dig", StringComparison.OrdinalIgnoreCase)
                                            && IsAdjacentOrOn(ant.Position, task.targetposition);

                    if (!isDigAndAdjacent)
                    {
                        // Ensure the ant has a path unless it's already in working position (e.g. adjacent for dig)
                        if (ant.path == null || ant.path.Count == 0)
                        {
                            try
                            {
                                pathfind(ant); // compute path (may target adjacent traversable cell if needed)
                            }
                            catch
                            {
                                // can't reach — release the task so others can try
                                ant.Currenttask = null;
                                ant.clamedtaskid = -1;
                                ant.path = null;
                                continue;
                            }
                        }
                    }

                    // If there is a path, move one step per tick
                    if (!isDigAndAdjacent && ant.path != null && ant.path.Count > 0)
                    {
                        int nextIdx = ant.path[0];
                        int nextX = nextIdx % GridWidth;
                        int nextY = nextIdx / GridWidth;

                        // If the next cell became blocked, try to re-path; if that fails, drop the task.
                        var nextCell = grid.GetCellAtLocation(nextX, nextY);
                        if (!nextCell.IsTraversable)
                        {
                            try
                            {
                                pathfind(ant);
                            }
                            catch
                            {
                                ant.Currenttask = null;
                                ant.clamedtaskid = -1;
                                ant.path = null;
                            }
                            continue;
                        }

                        // Move the ant: remove from old cell, add to new cell, update position
                        var oldPos = ant.Position;
                        int oldX = oldPos.Item1;
                        int oldY = oldPos.Item2;
                        var oldCell = grid.GetCellAtLocation(oldX, oldY);
                        oldCell.RemoveEntity(ant);
                        grid.AddEntityToCellLocation(nextX, nextY, ant);
                        ant.Position = (nextX, nextY);

                        // remove the step atn took
                        ant.path.RemoveAt(0);

                        // If we've reached the end of the path, attempt to work on the task (may start multi-tick dig)
                        if (ant.path.Count == 0)
                        {
                            PerformTaskWork(ant);
                        }
                    }
                    else
                    {
                        // No movement to perform this tick (either because dig-and-adjacent or no path) — attempt work
                        PerformTaskWork(ant);
                    }
                }
                else
                {
                    // idle ant
                    antwander(ant);
                }
            }
        }

        // Helper: returns true if pos is same cell or cardinal neighbour of target
        private static bool IsAdjacentOrOn((int, int) pos, (int, int) target)
        {
            int dx = Math.Abs(pos.Item1 - target.Item1);
            int dy = Math.Abs(pos.Item2 - target.Item2);
            return (dx + dy) <= 1;
        }

        // Perform one tick of work for the ant's current task.
        // Keeps the ant assigned for multi-tick tasks (dig) until the task is actually finished.
        public void FillinvWfood(Ant ant)
        {
            var cell = grid.GetCellAtLocation(ant.Position.Item1, ant.Position.Item2);
            var foods = cell.Entities.OfType<Food>().ToList();
            foreach (var food in foods)
            {
                int spaceleft = ant.carryingcapacity - ant.foodcarried;
                if (spaceleft > 0)
                {
                    if (food.currentAmount <= spaceleft)
                    {
                        ant.foodcarried += food.currentAmount;
                        food.currentAmount = 0;
                        // remove food entity from grid
                        cell.RemoveEntity(food);
                    }
                    else
                    {
                        ant.foodcarried += spaceleft;
                        food.currentAmount -= spaceleft;
                    }
                }
                return;
            }
        }
        private void PerformTaskWork(Ant ant)
        {
            if (ant.Currenttask == null) return;

            var task = ant.Currenttask;
            var tx = task.targetposition.Item1;
            var ty = task.targetposition.Item2;

            switch (task.tasktype.ToLowerInvariant())
            {
                case "dig":
                    // Ant must be adjacent or on the target to dig.
                    if (!IsAdjacentOrOn(ant.Position, task.targetposition))
                    {
                        // Not in position: try to pathfind next tick.
                        try
                        {
                            pathfind(ant);
                        }
                        catch
                        {
                            // can't reach: release task
                            ant.Currenttask = null;
                            ant.clamedtaskid = -1;
                            ant.path = null;
                        }
                        return;
                    }

                    // In position: perform a single dig tick.
                    var targetCell = grid.GetCellAtLocation(tx, ty);
                    if (targetCell is Dirt)
                    {
                        // call dig which increments digprogress and may convert to Air when done
                        dig(tx, ty);

                        // After dig call, check if cell is still Dirt (work not finished).
                        var afterCell = grid.GetCellAtLocation(tx, ty);
                        if (afterCell is Dirt)
                        {
                            // Still digging: keep the ant assigned and prevent it from moving.
                            ant.path = new List<int>();
                            return; // keep working next tick
                        }
                        else
                        {
                            // Finished digging: free the ant/task.
                            ant.Currenttask = null;
                            ant.clamedtaskid = -1;
                            ant.path = null;
                            return;
                        }
                    }
                    else
                    {
                        // Target is no longer dirt (maybe someone else dug it). mark task complete/fail and free ant.
                        ant.Currenttask = null;
                        ant.clamedtaskid = -1;
                        ant.path = null;
                        return;
                    }
                case "gatherfood":
                // require ant to be on the food cell to gather
                    if (ant.Position != task.targetposition)
                    {
                        try
                        {
                            pathfind(ant);
                        }
                        catch
                        {
                            ant.Currenttask = null;
                            ant.clamedtaskid = -1;
                            ant.path = null;
                        }
                        return;
                    }
                    try
                    {
                        var foodCell = grid.GetCellAtLocation(tx, ty);
                        var foodEntity = foodCell.Entities.OfType<Food>().FirstOrDefault();
                        if (foodEntity != null)
                        {

                            // ant first eats up to max food if posible then gathers up to max inventory space 
                            // if no food in food node then remove it from grid
                            int foodNeeded = ant.maxfood - ant.food;
                            if (foodNeeded > 0)
                            {
                                if(foodEntity.currentAmount <= foodNeeded)
                                {
                                    ant.food += foodEntity.currentAmount;
                                    foodNeeded -= foodEntity.currentAmount;
                                    foodEntity.currentAmount = 0;
                                    // remove food entity from grid
                                    foodCell.RemoveEntity(foodEntity);
                                }
                                else
                                {
                                    ant.food += foodNeeded;
                                    foodEntity.currentAmount -= foodNeeded;
                                    foodNeeded = 0;
                                }
                                
                            }
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                    // gather is one-shot here
                    ant.Currenttask = null;
                    ant.clamedtaskid = -1;
                    ant.path = null;
                    return;
                case "foodstoregather":
                    // Two stage task:
                    // - stage A: go to nearest raw food source (Food entity, not FoodStore) and fill inventory
                    // - stage B: carry inventory to the original food store target and deposit

                    // Ensure we remember the store target
                    if (ant.FoodStoreTarget == null)
                    {
                        ant.FoodStoreTarget = task.targetposition;
                    }

                    // If currently filling from source phase is not started, start it by targeting nearest raw food
                    if (!ant.FillingFromSource && ant.foodcarried < ant.carryingcapacity)
                    {
                        // find nearest Food (exclude FoodStore)
                        Food closestFood = null;
                        int bestDist = int.MaxValue;
                        for (int x = 0; x < grid.width; x++)
                        {
                            for (int y = 0; y < grid.height; y++)
                            {
                                var c = grid.GetCellAtLocation(x, y);
                                foreach (var e in c.Entities)
                                {
                                    if (e is Food f)
                                    {
                                        // skip if this entity has 0 amount
                                        if (f.currentAmount <= 0) continue;
                                        int dist = Math.Abs(ant.Position.Item1 - x) + Math.Abs(ant.Position.Item2 - y);
                                        if (dist < bestDist)
                                        {
                                            bestDist = dist;
                                            closestFood = f;
                                        }
                                    }
                                }
                            }
                        }

                        if (closestFood == null)
                        {
                            // no raw food available: release task so others might handle/store re-queue
                            ant.Currenttask = null;
                            ant.clamedtaskid = -1;
                            ant.path = null;
                            ant.FoodStoreTarget = null;
                            ant.FillingFromSource = false;
                            return;
                        }

                        // target the food source first
                        ant.Currenttask.targetposition = closestFood.Position;
                        ant.FillingFromSource = true;
                        ant.path = null; // force path recompute next tick
                        try { pathfind(ant); }
                        catch { /* path may be computed next tick */ }
                        return;
                    }

                    // If filling from source phase active
                    if (ant.FillingFromSource)
                    {
                        // Not at food source yet -> pathfind / move
                        if (ant.Position != ant.Currenttask.targetposition)
                        {
                            try { pathfind(ant); }
                            catch
                            {
                                // can't reach food, abort this gather-to-store task
                                ant.Currenttask = null;
                                ant.clamedtaskid = -1;
                                ant.path = null;
                                ant.FoodStoreTarget = null;
                                ant.FillingFromSource = false;
                            }
                            return;
                        }

                        // At food source: fill inventory (uses existing helper)
                        FillinvWfood(ant);

                        // After filling, switch to deliver-to-store phase
                        ant.FillingFromSource = false;

                        if (ant.FoodStoreTarget.HasValue)
                        {
                            ant.Currenttask.targetposition = ant.FoodStoreTarget.Value;
                            ant.path = null;
                            try { pathfind(ant); }
                            catch { /* try next tick */ }
                        }
                        else
                        {
                            // no store target recorded — finish task
                            ant.Currenttask = null;
                            ant.clamedtaskid = -1;
                            ant.path = null;
                        }
                        return;
                    }

                    // Deliver phase: we have foodcarried > 0 (or filling not needed) and should head to store
                    if (ant.FoodStoreTarget.HasValue)
                    {
                        if (ant.Position != ant.FoodStoreTarget.Value)
                        {
                            try { pathfind(ant); }
                            catch
                            {
                                // can't reach store -> abort and reset
                                ant.Currenttask = null;
                                ant.clamedtaskid = -1;
                                ant.path = null;
                                ant.FoodStoreTarget = null;
                                ant.FillingFromSource = false;
                            }
                            return;
                        }

                        // At store position: deposit
                        AddFoodToFoodStore(ant);

                        // task finished — cleanup
                        ant.Currenttask = null;
                        ant.clamedtaskid = -1;
                        ant.path = null;
                        ant.FoodStoreTarget = null;
                        ant.FillingFromSource = false;
                        return;
                    }

                    // Fallback: cleanup
                    ant.Currenttask = null;
                    ant.clamedtaskid = -1;
                    ant.path = null;
                    ant.FoodStoreTarget = null;
                    ant.FillingFromSource = false;
                    return;
                // two part task first ant must fill inventory with food from nearest food source then go to food store and add it



                case "build":
                    // require ant to be on or adjacent 
                    if (!IsAdjacentOrOn(ant.Position, task.targetposition))
                    {
                        try
                        {
                            pathfind(ant);
                        }
                        catch
                        {
                            ant.Currenttask = null;
                            ant.clamedtaskid = -1;
                            ant.path = null;
                        }
                        return;
                    }

                    try
                    {
                        var buildCell = grid.GetCellAtLocation(tx, ty);
                        if (buildCell.IsTraversable)
                        {
                            CreateFoodStore(tx, ty);
                        }
                    }
                    catch
                    {
                        // ignore
                    }

                    // build is one-shot here
                    ant.Currenttask = null;
                    ant.clamedtaskid = -1;
                    ant.path = null;
                    return;

                default:
                    // Other tasks: if in position, complete; otherwise try to pathfind.
                    if (!IsAdjacentOrOn(ant.Position, task.targetposition))
                    {
                        try
                        {
                            pathfind(ant);
                        }
                        catch
                        {
                            ant.Currenttask = null;
                            ant.clamedtaskid = -1;
                            ant.path = null;
                        }
                        return;
                    }

                    // complete generic task
                    ant.Currenttask = null;
                    ant.clamedtaskid = -1;
                    ant.path = null;
                    return;
            }
        }

        private void CompleteAntTask(Ant ant)
        {
            
            PerformTaskWork(ant);
        }

        // Gathers all Ant instances currently in the grid.
        private List<Ant> GetAllAnts()
        {
            List<Ant> ants = new List<Ant>();
            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    var cell = grid.GetCellAtLocation(x, y);
                    foreach (var entity in cell.Entities)
                    {
                        if (entity is Ant ant)
                        {
                            ants.Add(ant);
                        }
                    }
                }
            }
            return ants;
        }
        /*
         * need to add food tasks for ant 
         * within the ant if the ant has less then 2 food then it will create a gather food task and add it to itself if it already has a task it should add it back to the queue 
         * if there is a food store ant should gather from that before gathering from random food on the grid
         * if there is no more task in queue the ant should gather food from the nearest food source and add it to a food store 
         * player should also be able to add food tasks manually which will go to the back of the queue (low priority)
         * */

        public void AddEntityToGameGrid(int x, int y, Entity entity)
        {
            grid.AddEntityToCellLocation(x, y, entity);
        }
        public (int, int) UserInputCords()
        {
            int X, Y;

            // get x from user with validation
            while (true)
            {
                Console.Write("X cord: ");
                var sx = Console.ReadLine();
                if (!int.TryParse(sx, out X))
                {
                    Console.WriteLine("Please enter a valid integer for X.");
                    continue;
                }

                if (X < 0 || X >= grid.width)
                {
                    Console.WriteLine($"X out of range (0 .. {grid.width - 1}). Please enter again.");
                    continue;
                }

                break;
            }

            // get y from user with validation
            while (true)
            {
                Console.Write("Y cord: ");
                var sy = Console.ReadLine();
                if (!int.TryParse(sy, out Y))
                {
                    Console.WriteLine("Please enter a valid integer for Y.");
                    continue;
                }

                if (Y < 0 || Y >= grid.height)
                {
                    Console.WriteLine($"Y out of range (0 .. {grid.height - 1}). Please enter again.");
                    continue;
                }

                break;
            }

            return (X, Y);
        }



        public void inputselector()
        {
            Console.WriteLine("1 : order the ants to dig \n2: order ants to make a food store \n3: select a cord to get the info of \nanything else : end tick ");
            var input = Console.ReadKey(true);

            if (input.KeyChar == '1')
            {
                Console.WriteLine("please enter the x and y position of the thing you want to dig");
                (int, int) cords = UserInputCords();
                algorithm.Task digtask = new algorithm.Task(queue1.lasttaskid++, "dig", (cords.Item1, cords.Item2));

                // enqueue the task so ProcessAntMovementAndTasks will assign it
                queue1.addtask(digtask);
                Console.WriteLine($"Queued dig task #{digtask.id} at ({cords.Item1},{cords.Item2})");
            }
            else if (input.KeyChar == '2')
            {
                Console.WriteLine("please enter the x and y position of the thing you want to add food store ");
                (int, int) cords = UserInputCords();
                algorithm.Task buildtask = new algorithm.Task(queue1.lasttaskid++, "build", (cords.Item1, cords.Item2));

                // enqueue the build task
                queue1.addtask(buildtask);
                Console.WriteLine($"Queued build task #{buildtask.id} at ({cords.Item1},{cords.Item2})");
            }
            else if (input.KeyChar == '3')
            {
                Console.WriteLine("please enter the x and y position of the thing you want to get info on ");
                (int, int) cords = UserInputCords();
                var cell = grid.GetCellAtLocation(cords.Item1, cords.Item2);
                Console.WriteLine($"Cell at ({cords.Item1},{cords.Item2}): Type={cell.GetType().Name}, Entities={cell.Entities.Count}");
                foreach (var entity in cell.Entities)
                {
                    Console.WriteLine($" - Entity ID={entity.Id}, Type={entity.GetType().Name}, Symbol={entity.Symbol}");
                    if (entity is Ant ant)
                    {
                        Console.WriteLine($"   - Ant Food={ant.food}, Current Task ID={ant.clamedtaskid}");
                    }
                    else if (entity is Food food)
                    {
                        Console.WriteLine($"   - Food Amount={food.currentAmount}");
                    }
                    else if (entity is FoodStore store)
                    {
                        Console.WriteLine($"   - FoodStore Contained={store.foodcontained}, Capacity={store.capacity}");
                    }
                }
            }
            else
            {
                Console.Clear();
                grid.PrintGrid();
                tick++;
            }
        }
        public void HungerAnts()
        {
            List<Ant> ants = GetAllAnts();
            foreach (var ant in ants)
            {
                ant.food--;
                if (ant.food <= 0)
                {
                    // remove ant from grid
                    var pos = ant.Position;
                    var cell = grid.GetCellAtLocation(pos.Item1, pos.Item2);
                    cell.RemoveEntity(ant);
                    Console.WriteLine($"An ant has died of hunger at ({pos.Item1},{pos.Item2})");
                }
            }
        }
        public void dig(int x, int y)
        {
            if (grid.GetCellAtLocation(x,y) is Dirt)
            {
                Dirt dirtcell = (Dirt)grid.GetCellAtLocation(x, y);
                dirtcell.digprogress++;
                if (dirtcell.digprogress >= dirtcell.hardness)
                {
                    // replace with air cell
                    ReplaceCellAtLocation(x, y, new Air(x, y));
                    Console.Clear();
                    printgrid();
                    // every tick must call again to dig untill it is done 
                }

            }
            else
            {
               throw new InvalidOperationException("Cell is not dirt");
            }


        }
        public void CreateFoodStore(int x, int y)
        {
            if (!grid.IsInGridRange(x, y))
                throw new ArgumentOutOfRangeException(nameof(x), "Position out of grid range.");
            // where to add the onlyunder ground limit 

            
            var fs = new FoodStore(++lastEntityId, 'S');
            AddEntityToGameGrid(x, y, fs);

            
            fs.Position = (x, y);


        }

        
        public void printgrid()
        {
            grid.PrintGrid();
        }
        public void antwander(Ant ant )
        {
            if (queue1.tasks.Count == 0 && ant.clamedtaskid == -1)
            {
                Random rand = new Random();
                int x = rand.Next(GridWidth);
                int y = rand.Next(0, GridHeight / 4);
                algorithm.Task wander = new algorithm.Task(queue1.lasttaskid++, "wander", (x, y));
                ant.Currenttask = wander;
                ant .clamedtaskid = -1;
            }
        }
        /*
         * Algorithm :
         * find closest ant to end position that doesnt already have a task 
         * work out path to end position
         * send and to end position 
         *  starting point will be the end goal eg food then djikstas from there to find the closest ant and then resverse path and get ant to follow it 
         */
        public void ClosestAnt(algorithm.Task task)
        {
            List<Ant> ants = new List<Ant>();
            for(int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    var  cell = grid.GetCellAtLocation(x, y);
                    foreach (var entity in cell.Entities)
                    {
                        if (entity is Ant)
                        {
                            Ant ant = (Ant)entity;
                            if (ant.clamedtaskid == -1)
                            {
                                ants.Add(ant);
                            }
                        }
                    }
                }
            }
            if(ants.Count == 0)
            {
                throw new Exception("no ants cn task");
            }
            int xpos = task.targetposition.Item1;
            int ypos = task.targetposition.Item2;
            Ant closestant = null;
            int closestdistance = int.MaxValue;
            foreach (var ant in ants)
            {
                int distance = Math.Abs(ant.Position.Item1 - xpos) + Math.Abs(ant.Position.Item2 - ypos);
                if (distance < closestdistance)
                {
                    closestdistance = distance;
                    closestant = ant;
                }
            }
            if(closestant != null)
            {
                closestant.Currenttask = task;
                closestant.clamedtaskid = task.id;
            }
            else
            {
                throw new Exception("no ants cn task");
            }
        }
        // methord to find closes food thing (store or just food) to ant
        public void ClosestFoodWtaskadd(Ant ant)
        {
            List<Food> foodnat = new List<Food>();
            List<FoodStore> foodStores = new List<FoodStore>();
            // need to add stuff so it check food and food stores
            
            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    var cell = grid.GetCellAtLocation(x, y);
                    foreach (var entity in cell.Entities)
                    {
                        if (entity is Food)
                        {
                            Food food = (Food)entity;
                            foodnat.Add(food);
                        }
                    }
                }
            }
            if (foodnat.Count == 0)
            {
                throw new Exception("no food found");
            }
            int closestdistance = int.MaxValue;
            Food closestfood = null;
            foreach (var food in foodnat)
            {
                int distance = Math.Abs(ant.Position.Item1 - food.Position.Item1) + Math.Abs(ant.Position.Item2 - food.Position.Item2);
                if (distance < closestdistance)
                {
                    closestdistance = distance;
                    closestfood = food;
                }
            }
            if (closestfood != null)
            {
                algorithm.Task foodtask = new algorithm.Task(queue1.lasttaskid++, "gatherfood", closestfood.Position);
                ant.Currenttask = foodtask;
                ant.clamedtaskid = foodtask.id;
            }
            else
            {
                throw new Exception("no food found");
            }
        }
        public void pathfind(Ant ant)
        {

            // check if start and end are the same
            // check area around ant to see if its full boxed
            // if any of these then thwrow exception
            // implement djikstras algorithm

            // return path into ant path list then ant moves along it each tick
            if (ant == null) throw new ArgumentNullException(nameof(ant));
            if (ant.Currenttask == null) throw new InvalidOperationException("Ant has no task to pathfind to.");

            var start = ant.Position;
            var end = ant.Currenttask.targetposition;

            int startX = start.Item1;
            int startY = start.Item2;
            int endX = end.Item1;
            int endY = end.Item2;

            if (!grid.IsInGridRange(startX, startY))
                throw new ArgumentOutOfRangeException("Ant start position out of grid range.");
            if (!grid.IsInGridRange(endX, endY))
                throw new ArgumentOutOfRangeException("Task target position out of grid range.");

            int startIdx = startY * GridWidth + startX;
            int goalX = endX;
            int goalY = endY;

            // if goal cell ismnt traversable find closest adjacent traversable cell (will have to add doubble check for doubble land locked)
            var goalCell = grid.GetCellAtLocation(endX, endY);
            if (!goalCell.IsTraversable)
            {
                // look for adjacent traversable cell
                (int x, int y)? best = null;
                int bestDist = int.MaxValue;
                // check 4 directions   
                (int dx, int dy)[] dirs = { (1, 0), (-1, 0), (0, 1), (0, -1) };
                foreach (var d in dirs)
                {
                    int nx = endX + d.dx;
                    int ny = endY + d.dy;
                    // skip positions outside the grid
                    if (!grid.IsInGridRange(nx, ny)) continue;
                    var nc = grid.GetCellAtLocation(nx, ny);
                    // only consider if ant can go there 
                    if (!nc.IsTraversable) continue;
                    if (nc.IsTraversable)
                    {
                        // use manhattan distance to find closest (dm cos canot move diagonally)    
                        int manhattan = Math.Abs(nx - startX) + Math.Abs(ny - startY);
                        if (manhattan < bestDist)
                        {
                            bestDist = manhattan;
                            best = (nx, ny);
                        }
                    }
                }
                if (best.HasValue)
                {
                    // set the goal to the chosen cell to get to (for food it its the cell for dig....)
                    goalX = best.Value.x;
                    goalY = best.Value.y;
                }
                else
                {
                    // could add more here to add extra digs to get to a point or stop player from doing it 
                    throw new InvalidOperationException("No reachable traversable cell adjacent to target.");
                }
            }

            int goalIdx = goalY * GridWidth + goalX;

            // If already at goal empty path
            if (startX == goalX && startY == goalY)
            {
                ant.path = new List<int>(); // already there
                return;
            }
            // n is area of grid
            int n = GridWidth * GridHeight;
            var dist = new int[n];
            var parent = new int[n];
            var visited = new bool[n];
            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
                parent[i] = -1;
            }

            var pq = new PriorityQueue<int, int>();
            dist[startIdx] = 0;
            pq.Enqueue(startIdx, 0);

            (int dx, int dy)[] neighbors = { (1, 0), (-1, 0), (0, 1), (0, -1) };

            while (pq.Count > 0)
            {
                int u = pq.Dequeue();
                if (visited[u]) continue;
                visited[u] = true;

                if (u == goalIdx) break;

                int ux = u % GridWidth;
                int uy = u / GridWidth;

                foreach (var d in neighbors)
                {
                    int vx = ux + d.dx;
                    int vy = uy + d.dy;
                    if (!grid.IsInGridRange(vx, vy)) continue;
                    int v = vy * GridWidth + vx;

                    // allow traversal if cell is traversable
                    var vcell = grid.GetCellAtLocation(vx, vy);
                    if (!vcell.IsTraversable) continue;

                    int alt = dist[u] + 1; 
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        parent[v] = u;
                        pq.Enqueue(v, alt);
                    }
                }
            }

            if (parent[goalIdx] == -1)
            {
                // add so that if this happens task is attempted again at back of quque or passed onto dif ant 
                throw new InvalidOperationException("No path found from ant to task.");
            }

            // reconstruct path (from start to goal)
            var rev = new List<int>();
            for (int at = goalIdx; at != -1; at = parent[at])
            {
                rev.Add(at);
            }
            rev.Reverse();

            // Remove the first element if it is the start cell, so ant.path contains steps to take (next cell first)
            if (rev.Count > 0 && rev[0] == startIdx)
            {
                rev.RemoveAt(0);
            }

            ant.path = rev;
        }


    }

}
