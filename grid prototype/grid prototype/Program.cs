using System.Security.Cryptography.X509Certificates;

internal class Program
{
    private static void Main(string[] args)
    {
        int grid_y = 0;
        int grid_x = 0;
        
        while(grid_y <= 0 && grid_x <= 0)
        {
            Console.WriteLine("please enter the height of the grid");
            grid_y = Convert.ToInt32(Console.ReadLine());
            

             Console.WriteLine("please enter the width of the grid");
             grid_x = Convert.ToInt32(Console.ReadLine());
          

        }
        string[,] grid = new string[grid_y, grid_x];
        for (int x = 0; x < grid_y; x++)
        {
            for (int y = 0; y < grid_x; y++)
            {
                grid[x, y] = "[]";
            }
        }
        
        for (int x = 0;x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                Console.Write(grid[x,y]);
            }
            Console.WriteLine();
        }
        
    }
    
}
public class Cell
{
    public string Value { get; set; }
    public Cell (string value= "")
    {
        Value = value;
    }
    public override string ToString()
    {
        return Value;
    }
}