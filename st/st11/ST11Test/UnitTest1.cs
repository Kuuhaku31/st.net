
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
}
