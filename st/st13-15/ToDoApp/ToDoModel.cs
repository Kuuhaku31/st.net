
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp;


/// <summary>
/// 定义一个 DbContext 来管理 ToDo 实体与数据库的交互
/// </summary>
internal class ToDoContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=testdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
}

/// <summary>
/// 模型层
/// </summary>
internal static class ToDoModel
{
    // 定义一个静态列表来存储 ToDo 项目，初始读取数据库中的数据
    public static List<ToDo> ToDos { get; set; } 

    /// <summary>
    /// 静态构造函数，从数据库加载 ToDo 列表并监听每个 ToDo 的属性变化
    /// </summary>
    static ToDoModel()
    {
        using var context = new ToDoContext();
        ToDos = [.. context.ToDos.Select(t => new ToDo(t.Name, t.Deadline, t.Completed, t.Priority, t.Id))];
    }


    // 定义模型的更新、添加和删除方法，这些方法目前仅输出调试信息，实际应用中可以连接数据库或其他数据存储

    /// <summary>
    /// 更新 ToDo 对象的名称，并将更改保存到数据库
    /// </summary>
    /// <param name="todo"></param>
    /// <param name="name"></param>
    public static void
    UpdateName(ToDo todo, string name)
    {
        using var context = new ToDoContext();
        var taskToUpdate = context.ToDos.Find(todo.Id);
        if(taskToUpdate != null)
        {
            taskToUpdate.Name = name;
            context.SaveChanges();
            Debug.WriteLine($"Name has been updated to {name} in ToDo#{todo.Id}");
        }
    }

    /// <summary>
    /// 更新 ToDo 对象的截止日期，并将更改保存到数据库
    /// </summary>
    /// <param name="todo"></param>
    /// <param name="deadline"></param>
    public static void
    UpdateDeadline(ToDo todo, DateTime deadline)
    {
        using var context = new ToDoContext();
        var taskToUpdate = context.ToDos.Find(todo.Id);
        if(taskToUpdate != null)
        {
            taskToUpdate.Deadline = deadline;
            context.SaveChanges();
            Debug.WriteLine($"Deadline has been updated to {deadline} in ToDo#{todo.Id}");
        }
    }

    /// <summary>
    /// 更新 ToDo 对象的优先级，并将更改保存到数据库
    /// </summary>
    /// <param name="todo"></param>
    /// <param name="priority"></param>
    public static void
    UpdatePriority(ToDo todo, int priority)
    {
        using var context = new ToDoContext();
        var taskToUpdate = context.ToDos.Find(todo.Id);
        if(taskToUpdate != null)
        {
            taskToUpdate.Priority = priority;
            context.SaveChanges();
            Debug.WriteLine($"Priority has been updated to {priority} in ToDo#{todo.Id}");
        }
    }

    /// <summary>
    /// 更新 ToDo 对象的完成状态，并将更改保存到数据库
    /// </summary>
    /// <param name="todo"></param>
    /// <param name="completed"></param>
    public static void
    UpdateCompleted(ToDo todo, bool completed)
    {
        using var context = new ToDoContext();
        var taskToUpdate = context.ToDos.Find(todo.Id);
        if(taskToUpdate != null)
        {
            taskToUpdate.Completed = completed;
            context.SaveChanges();
            Debug.WriteLine($"Completed has been updated to {completed} in ToDo#{todo.Id}");
        }
    }

    /// <summary>
    /// 添加新的 ToDo 项目到数据库，并返回添加后的 ToDo 对象
    /// </summary>
    /// <param name="todo"></param>
    /// <returns></returns>
    public static ToDo Add(ToDo todo)
    {
        ToDo newToDo;
        using(var context = new ToDoContext())
        {
            var entry = context.ToDos.Add(todo);
            newToDo   = entry.Entity; // 获取添加后的 ToDo 对象，此时对象的 Id 已经由数据库生成
            context.SaveChanges();

            Debug.WriteLine($"ToDo#{newToDo.Id} has been added");
        }
        return newToDo;
    }

    /// <summary>
    /// 从数据库删除指定的 ToDo 项目，并输出调试信息
    /// </summary>
    /// <param name="todo"></param>
    public static void Delete(ToDo todo)
    {
        using var context = new ToDoContext();
        var taskToDelete = context.ToDos.Find(todo.Id);
        if(taskToDelete != null) // 如果找到对应的 ToDo 对象，从数据库中删除该对象，并保存更改
        {
            context.ToDos.Remove(taskToDelete);
            context.SaveChanges();
            Debug.WriteLine($"ToDo#{todo.Id} has been deleted");
        }
    } 
}

/// <summary>
/// 定义 ToDo 类，包含名称、截止日期、完成状态、优先级和可选的 ID 属性
/// 并使用 ObservableObject 以支持属性变化通知
/// </summary>
/// <param name="name"></param>
/// <param name="deadline"></param>
/// <param name="completed"></param>
/// <param name="priority"></param>
/// <param name="id"></param>
internal partial class
ToDo(string name, DateTime deadline, bool completed = false, int priority = 1, int? id = null) : ObservableObject
{
    public int? Id { get; set; } = id;

    [ObservableProperty]
    private string _name = name;
    
    [ObservableProperty]
    private DateTime _deadline = deadline;

    [ObservableProperty]
    private bool _completed = completed;

    [ObservableProperty]
    private int _priority = priority;
}
