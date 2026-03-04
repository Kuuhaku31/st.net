namespace ST03
{
    internal class ToDo(int id, string name, DateTime deadline, bool completed)
    {
        public int      Id        { get; set; } = id;
        public string   Name      { get; set; } = name;
        public DateTime Deadline  { get; set; } = deadline;
        public bool     Completed { get; set; } = completed;

        static void
        Main(string[] args)
        {
            ToDo t = new(1, "s", new DateTime(1), true);

            Console.WriteLine(t.Id);
        }
    }
}