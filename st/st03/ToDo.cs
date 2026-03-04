namespace Syntax
{
    internal class ToDo
    {
        private int _id;
        private string _name;
        private DateTime _deadline;
        private bool _completed;

        public ToDo(int id, string name, DateTime deadline, bool completed)
        {
            _id = id;
            _name = name;
            _deadline = deadline;
            _completed = completed;
        }

        public int Id
        {
            get
            {
                Console.WriteLine("Get: " + _id);
                return _id;
            }
            set
            {
                Console.WriteLine("Set: " + value);
                _id = value;
            }
        }

        static void Main(string[] args)
        {
            ToDo t = new ToDo(1, "s", new DateTime(1), true);

            t.Id = 3;
            Console.WriteLine(t.Id);
        }
    }
}