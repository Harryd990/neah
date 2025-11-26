using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Game game = new Game(10, 10);
        game.Initialize_Ants();
        Entity queen = new Queen(0, 'Q');
        game.AddEntityToGameGrid(1, 1, queen);
        game.Run();


        Console.ReadLine();
       
    }
    

}