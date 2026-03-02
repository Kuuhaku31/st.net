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
    }
}