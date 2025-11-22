using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Grid grid = new Grid(10, 10);
        
        Entity first  = new Ant(1,'A');
        Entity second = new Ant(1, 'A');

        grid.AddEntityToCellLocation(1, 1, first);
        grid.AddEntityToCellLocation(4, 1, second);

        Console.WriteLine(grid.GetCellDetails(1, 1));

        grid.PrintGrid();
        
        Console.ReadLine();
       
    }
    

}