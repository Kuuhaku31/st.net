
// 设置测试集合行为，禁用测试并行化以避免对控制台输入输出的干扰
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ToDoConsole.Tests;

using System.Reflection; // 引入反射命名空间以便在测试中动态调用 ToDoConsole 程序的入口点


public class UnitTest1
{
    // 测试用例：

    [Fact]
    public async Task
    AddTask_DisplaysTaskInLaterList()
    {
        var output = await RunProgramAsync("1", "Buy milk", "4");

        Assert.Contains("Exiting...", output);
        Assert.Contains("- Buy milk", GetLastToDoList(output));
    }

    [Fact]
    public async Task
    RemoveTask_WithValidIndex_RemovesTaskFromLaterList()
    {
        var output = await RunProgramAsync("1", "Buy milk", "2", "0", "4");

        Assert.DoesNotContain("- Buy milk", GetLastToDoList(output));
    }

    [Fact]
    public async Task
    UpdateTask_WithValidIndex_ReplacesTaskInLaterList()
    {
        var output = await RunProgramAsync("1", "Buy milk", "3", "0", "Read book", "4");
        var lastList = GetLastToDoList(output);

        Assert.Contains("- Read book", lastList);
        Assert.DoesNotContain("- Buy milk", lastList);
    }

    [Fact]
    public async Task
    InvalidOption_ShowsValidationMessage()
    {
        var output = await RunProgramAsync("9", "4");

        Assert.Contains("Invalid option. Please try again.", output);
    }

    [Fact]
    public async Task
    RemoveTask_WithInvalidIndex_ShowsValidationMessage()
    {
        var output = await RunProgramAsync("2", "3", "4");

        Assert.Contains("Invalid index. Please try again.", output);
    }


    // 运行 ToDoConsole 程序并捕获其输出以供测试断言使用
    private static async Task<string>
    RunProgramAsync(params string[] inputs)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;
        using var reader = new StringReader(string.Join(Environment.NewLine, inputs));
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(candidate => candidate.GetName().Name == "ToDoConsole")
                ?? Assembly.Load("ToDoConsole");
            var entryPoint = assembly.EntryPoint ?? throw new InvalidOperationException("ToDoConsole entry point not found.");
            var parameters = entryPoint.GetParameters().Length == 0
                ? null
                : new object?[] { Array.Empty<string>() };
            var result = entryPoint.Invoke(null, parameters);

            if(result is Task task)
            {
                await task;
            }

            return writer.ToString();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }

    // 从程序输出中提取最后一次打印的待办事项列表，以便在测试中进行断言
    private static string GetLastToDoList(string output)
    {
        const string marker = "ToDo List:";
        var lastMarkerIndex = output.LastIndexOf(marker, StringComparison.Ordinal);

        Assert.True(lastMarkerIndex >= 0, "Expected at least one to-do list to be printed.");

        var start = lastMarkerIndex + marker.Length;
        var nextMenuIndex = output.IndexOf("Choose an option:", start, StringComparison.Ordinal);

        return nextMenuIndex >= 0
            ? output[start..nextMenuIndex]
            : output[start..];
    }
}
