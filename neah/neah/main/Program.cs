using neah.main;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Neah Ant Colony Simulation");
        Console.WriteLine("--------------------------");
        Console.WriteLine("enter width then height of sim");
        int width = Convert.Toint32( Console.ReadLine());
        ConvertToInt32  converttoint = new ConvertToInt32();
        Console.ReadLine();
        Game game = new Game(15, 20);
        game.Initialize_Game();
        


        game.Run();
       







        Console.ReadLine();
       
    }
    

}