
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, .NET!");
        
        Console.Write("Enter your name: ");

        string name = Console.ReadLine() ?? "";

        if(string.IsNullOrWhiteSpace(name)) Console.WriteLine("You didn't enter a name.");
        else Console.WriteLine($"Hello, {name}!");
    }
}