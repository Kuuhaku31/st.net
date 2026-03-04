namespace ST02.Syntax
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Order order1 = new(1, "Alan Kay",   new(2022, 2, 2));
            Order order2 = new(2, "Ted Nelson", new(2022, 3, 7));
            order1.DisplayOrder();
            order2.DisplayOrder();

            Book book1 = new("J.K. Rowling",       "Harry Potter and the Sorcerer's Stone", 309);
            Book book2 = new("George R.R. Martin", "A Game of Thrones",                     694);
            book1.DisplayBook();
            book2.DisplayBook();

            Console.WriteLine();

            // 属性读写
            order2.CustomerName = "Tim Berners-Lee";
            order2.DisplayOrder();

            Console.WriteLine($"Order1 ID: {order1.OrderId}, Customer: {order1.CustomerName}, Date: {order1.OrderDate}");
        }
    }
}