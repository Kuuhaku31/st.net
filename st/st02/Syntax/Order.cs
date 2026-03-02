namespace ST02.Syntax
{
    internal class Order(int orderId, string customerName, DateTime orderDate)
    {
        private readonly int      _orderId      = orderId;
        private readonly string   _customerName = customerName;
        private readonly DateTime _orderDate    = orderDate;

        public void DisplayOrder()
        {
            Console.WriteLine($"Order ID: {_orderId}, Customer: {_customerName}, Date: {_orderDate}");
        }
    }
}