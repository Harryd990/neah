using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Game game = new Game(15, 20);
        game.Initialize_Game();
        game.dig(6,7);
        game.dig(6, 7);
        game.dig(6, 7);
        game.dig(6, 7);
        game.dig(6, 7);


        game.Run();
       







        Console.ReadLine();
       
    }
    

}