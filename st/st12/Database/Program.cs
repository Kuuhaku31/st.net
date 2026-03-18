
namespace Database;

internal class Program
{
    static void Main(string[] args)
    {
        // 创建 TaskContext 实例
        using var context = new TaskContext();
        {
            // 获取ToDos表格
            var toDos = context.ToDos;

            AddToDo(context);
            UpdateToDo(context);
            DeleteToDo(context);

            // Read
            var tasks = toDos
                .Where(t => t.Completed == false) // 过滤未完成的任务
                .Select(t => $"Name: {t.Name}, Deadline: {t.Deadline}, Completed: {t.Completed}");
            foreach(var item in tasks)
            {
                Console.WriteLine(item);
            }
        }
        // 自动释放资源
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="context"></param>
    private static void
    AddToDo(TaskContext context)
    {
        ToDo[] toDoList = [
            new("Buy milk", DateTime.Now, true),
            new("Buy PC", new DateTime(2023, 12, 24), true),
            new("Buy chocolate", new DateTime(2024, 2, 14), true),
            new("Clean the kitchen", DateTime.Now.AddDays(1))
        ];

        // 获取ToDos表格
        var toDos = context.ToDos;

        // Create
        foreach(var toDo in toDoList) toDos.Add(toDo);
        context.SaveChanges(); // 保存更改
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="context"></param>
    private static void
    UpdateToDo(TaskContext context)
    {
        // 获取Name为"Buy milk"的任务
        // 更新任务的状态为未完成
        var taskToUpdate = context.ToDos.FirstOrDefault(t => t.Name == "Buy milk");
        taskToUpdate?.Completed = false;

        // 获取Name为"Buy chocolate"的任务
        // 更新任务的截止日期为2024/1/1
        var taskToUpdate2 = context.ToDos.FirstOrDefault(t => t.Name == "Buy chocolate");
        taskToUpdate2?.Deadline = new DateTime(2024, 1, 1);

        // 保存更改
        context.SaveChanges();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="context"></param>
    private static void
    DeleteToDo(TaskContext context)
    {
        // 删除Name为"Buy chocolate"的任务
        var taskToDelete = context.ToDos.FirstOrDefault(t => t.Name == "Buy chocolate");
        if(taskToDelete != null) context.ToDos.Remove(taskToDelete);

        // 删除2024/1/15之前的任务
        var deadLine = new DateTime(2024, 1, 15);
        var tasksToDelete = context.ToDos.Where(t => t.Deadline < deadLine).ToList();
        foreach(var task in tasksToDelete) context.ToDos.Remove(task);

        context.SaveChanges();
    }
}
