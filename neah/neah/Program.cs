using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Grid grid = new Grid(8, 8);
        Entity first  = new Entity(1,"Bob","the first entity");
        grid.AddEntityToCellLocation(1, 1, first);
        Console.WriteLine(grid.GetCellDetails(1, 1)); 
        Console.ReadLine();
       
    }
    

}