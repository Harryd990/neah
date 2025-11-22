using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Grid grid = new Grid(8, 8);
        
        Entity first  = new Ant(1,'A');
        
        grid.AddEntityToCellLocation(1, 1, first);

        Console.WriteLine(grid.GetCellDetails(1, 1));

        grid.PrintGrid();
        
        Console.ReadLine();
       
    }
    

}