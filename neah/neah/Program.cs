using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Game game = new Game(10, 10);
        game.Initialize_Ants();
        game.Run();


        Console.ReadLine();
       
    }
    

}