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
        public void Initialize_Game()
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
        public void ReplaceCellAtLocation(int x, int y, Cell newCell)
        {
            grid.ReplaceCellAtLocation(x, y, newCell);
        }
        public void Run()
        {
            bool running = true;
            grid.PrintGrid();
            while (running)
            {

                inputselector();

                // if queen has 4 food lay eggs 
                if (queen != null && queen.food == 4)
                {
                    queen.food = 0;
                    queen.LayEggs(this);

                }


                Thread.Sleep(100);
            }
        }

        public void AddEntityToGameGrid(int x, int y, Entity entity)
        {
            grid.AddEntityToCellLocation(x, y, entity);
        }
        
        public void inputselector()
        {
            Console.WriteLine("1 : order the ants to dig \nanything else : end tick ");
            var input = Console.ReadKey(true);
            if (input.KeyChar == '1')
            {
                Console.WriteLine("please enter the x and y position of the thing you want to dig (x cord then enter y cord then entre)");
                int x = int.Parse(Console.ReadLine());
                int y = int.Parse(Console.ReadLine());
                algorithm.Task digtask = new algorithm.Task(queue1.lasttaskid++, "dig", (x, y));
            }
            else
            {
                Console.Clear();
                grid.PrintGrid();
                tick++;

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

                    int alt = dist[u] + 1; // uniform weight
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
