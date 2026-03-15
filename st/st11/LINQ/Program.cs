
// 定义任务列表
List<Task> tasks = [
    new Task("Buy milk", DateTime.Now),
    new Task("Buy PC", new DateTime(2023, 12, 24), true),
    new Task("Buy chocolate", new DateTime(2024, 2, 14), true)
];

// Select方法将每个任务转换为字符串表示
// var results = tasks.Select(t => $"Name: {t.Name}, Deadline: {t.Deadline}, Completed: {t.Completed}");
var results = tasks.Select(Task.ToString);

// 输出结果
foreach(var item in results)
{
    Console.WriteLine(item);
}


/// <summary>
/// 任务类，包含名称、截止日期和完成状态
/// </summary>
/// <param name="name"></param>
/// <param name="deadline"></param>
/// <param name="completed"></param>
class Task(string name, DateTime deadline, bool completed = false)
{
    public string   Name      { get; set; } = name;
    public DateTime Deadline  { get; set; } = deadline;
    public bool     Completed { get; set; } = completed;

    public override string ToString()
    {
        return $"Name: {Name}, Deadline: {Deadline}, Completed: {Completed}";
    }

    public static string ToString(Task task)
    {
        return $"Name: {task.Name}, Deadline: {task.Deadline}, Completed: {task.Completed}";
    }
}

