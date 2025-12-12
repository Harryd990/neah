using neah.main;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Neah Ant Colony Simulation");
        Console.WriteLine("--------------------------");
        Console.WriteLine("enter width then height of sim");
        
        int width = Convert.ToInt32(Console.ReadLine());
        int height = Convert.ToInt32(Console.ReadLine());
        Console.ReadLine();
        Game game = new Game(width, height);
        game.Initialize_Game();
        


        game.Run();
       







        Console.ReadLine();
       
    }
    

}