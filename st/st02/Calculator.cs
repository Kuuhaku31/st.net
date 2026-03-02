
namespace ST02
{
    internal class Calculator
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the first number: ");
            double num1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the second number: ");
            double num2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter an operator (+, -, *, /): ");
            string? op = Console.ReadLine();

            double? result;

            switch (op)
            {
                case "+": result = num1 + num2; break;
                case "-": result = num1 - num2; break;
                case "*": result = num1 * num2; break;
                case "/": result = num1 / num2; break;
                default: Console.WriteLine("Invalid operator."); result = null; break;
            }
            Console.WriteLine("Result: " + result ?? "No result due to invalid operator.");
        }
    }
}