namespace Syntax
{
    internal class Order
    {
        private int _orderId;
        private string _customerName;
        private DateTime _orderDate;

        public Order(int orderId, string customerName, DateTime orderDate)
        {
            _orderId = orderId;
            _customerName = customerName;
            _orderDate = orderDate;
        }

        public void DisplayOrder()
        {
            Console.WriteLine($"Order ID: {_orderId}, Customer: {_customerName}, Date: {_orderDate.ToShortDateString()}");
        }
    }
}