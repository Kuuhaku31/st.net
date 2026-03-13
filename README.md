# .NET 学习

```bash
# 编译并运行
dotnet build
dotnet run
```

添加到.sln文件中：

```bash
# 添加现有项目到解决方案
dotnet sln add <project_path>

# 创建新的解决方案
dotnet new sln

# 移除项目
dotnet sln remove <project_path>
```

## 创建 WPF 应用程序

```bash
# 创建 WPF 应用程序
dotnet new wpf -o MyWpfApp
```

## 代码格式化

导出当前配置文件

```bash
dotnet new editorconfig
```

## 添加测试

假设项目结构如下

```
st10\
    Collection\
        Collection.csproj
    ToDoConsole\
        ToDoConsole.csproj
    Collection.Tests\
        Collection.Tests.csproj
    ToDoConsole.Tests\
        ToDoConsole.Tests.csproj
```

1. 创建测试项目

```bash
dotnet new xunit -o Collection.Tests
dotnet new xunit -o ToDoConsole.Tests
```

2. 添加测试项目到解决方案

```bash
dotnet sln add Collection.Tests/Collection.Tests.csproj
dotnet sln add ToDoConsole.Tests/ToDoConsole.Tests.csproj
```

3. 添加对被测试项目的引用

```bash
dotnet add Collection.Tests/Collection.Tests.csproj reference Collection/Collection.csproj
dotnet add ToDoConsole.Tests/ToDoConsole.Tests.csproj reference ToDoConsole/ToDoConsole.csproj
```

4. 运行测试

```bash
dotnet test
```
