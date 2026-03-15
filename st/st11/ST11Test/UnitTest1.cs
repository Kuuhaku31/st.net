
namespace ST11Test;

using Xunit.Abstractions; // 引入 xUnit 的 ITestOutputHelper 接口以便在测试中输出日志信息

public class
UnitTest1(ITestOutputHelper output) // 通过构造函数注入 ITestOutputHelper 实例以便在测试方法中使用
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Test1()
    {
        _output.WriteLine("Hello World!");
    }

    [Fact]
    public void TestSelect()
    {
        List<Task> tasks = [
            new Task("Buy milk", DateTime.Now),
            new Task("Buy PC", new DateTime(2023, 12, 24), true),
            new Task("Buy chocolate", new DateTime(2024, 2, 14), true)
        ];

        var results = tasks.Select(t => $"{t.Name}, {t.Deadline}");

        foreach(var item in results)
        {
            _output.WriteLine(item);
        }
    }

    private class Task(string name, DateTime deadline, bool completed = false)
    {
        public string Name { get; set; } = name;
        public DateTime Deadline { get; set; } = deadline;
        public bool Completed { get; set; } = completed;
    }
}
