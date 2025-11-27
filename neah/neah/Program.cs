using neah;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Game game = new Game(15, 20);
        game.Initialize_Ants();
        
        game.Run();


        Console.ReadLine();
       
    }
    

}