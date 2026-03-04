namespace ST02.Syntax
{
    internal class Order(int orderId, string customerName, DateTime orderDate)
    {
        // 字段
        private int      _orderId      = orderId;
        private string   _customerName = customerName;
        private DateTime _orderDate    = orderDate;

        // 属性
        public  int      OrderId      { get { return _orderId;      } set { _orderId      = value; } }
        public  string   CustomerName { get { return _customerName; } set { _customerName = value; } }
        public  DateTime OrderDate    { get { return _orderDate;    } set { _orderDate    = value; } }

        // 自动属性
        public  string   Remark       {get; set;} = "No remarks";

        public void
        DisplayOrder()
        {
            Console.WriteLine($"Order ID: {_orderId}, Customer: {_customerName}, Date: {_orderDate}, Remark: {Remark}");
        }
    }
}