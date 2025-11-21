using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Grid grid = new Grid(8, 8);
        grid.innitializeGrid("[]");
        grid.PrintGrid();
        Console.ReadLine();
    }
    

}