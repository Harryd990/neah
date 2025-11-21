using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        Grid grid = new Grid(3, 2);
        grid.SetCell(0, 0, "A");
        grid.SetCell(1, 0, "B");
        grid.SetCell(2, 0, "C");
        grid.SetCell(0, 1, "D");
        grid.SetCell(1, 1, "E");
        grid.SetCell(2, 1, "F");
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                Console.Write(grid.GetCell(x, y) + " ");
            }
            Console.WriteLine();
        }
        Console.ReadLine();
    }
    
}