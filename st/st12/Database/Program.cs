
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

            // Create
            // AddToDo(context);

            // Read
            var date = new DateTime(2024, 1, 1);
            var tasks2 = toDos
                .Where(t => t.Deadline > date)
                .Select(t => $"Name: {t.Name}, Deadline: {t.Deadline}, Completed: {t.Completed}");
            foreach(var item in tasks2)
            {
                Console.WriteLine(item);
            }
        }
        // 自动释放资源
    }

    private static void AddToDo(TaskContext context)
    {
        // 获取ToDos表格
        var toDos = context.ToDos;

        // Create
        toDos.Add(new ToDo("Buy milk", DateTime.Now));
        toDos.Add(new ToDo("Buy PC", new DateTime(2023, 12, 24), true));
        toDos.Add(new ToDo("Buy chocolate", new DateTime(2024, 2, 14), true));
        context.SaveChanges(); // 保存更改
    }
}
