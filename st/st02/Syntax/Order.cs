namespace ST02.Syntax
{
    internal class Order(int orderId, string customerName, DateTime orderDate)
    {
        private int      _orderId      = orderId;
        private string   _customerName = customerName;
        private DateTime _orderDate    = orderDate;

        public int      OrderId      { get { return _orderId;      } set { _orderId      = value; } }
        public string   CustomerName { get { return _customerName; } set { _customerName = value; } }
        public DateTime OrderDate    { get { return _orderDate;    } set { _orderDate    = value; } }

        public void DisplayOrder()
        {
            Console.WriteLine($"Order ID: {_orderId}, Customer: {_customerName}, Date: {_orderDate}");
        }
    }
}