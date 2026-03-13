
var    toDoList = new List<string>(); // 创建一个空的待办事项列表
string userInput;                     // 用于存储用户输入的选项

// 显示菜单并处理用户输入
const string msg = @"
Choose an option:
1. Add Task
2. Remove Task
3. Update Task
4. Exit
";

// 不断显示菜单并处理用户输入，直到用户选择退出
do
{
    // 显示当前的待办事项列表
    Console.WriteLine("\nToDo List:");
    foreach(var item in toDoList) Console.WriteLine("- " + item);

    // 显示菜单并获取用户输入
    Console.WriteLine(msg);
    userInput = Console.ReadLine() ?? "";

    // 根据用户输入的选项执行相应的操作来添加、删除或更新待办事项，或者退出程序
    switch(userInput)
    {
    // 添加任务到列表
    case "1":
        Console.Write("Enter a task to add: ");
        var newTask = Console.ReadLine() ?? "";
        toDoList.Add(newTask);
        break;

    // 删除任务从列表
    case "2":
        Console.Write("Enter task index to remove: ");
        var index = Console.ReadLine() ?? "";
        if(int.TryParse(index, out int removeIndex) && removeIndex >= 0 && removeIndex < toDoList.Count)
            toDoList.RemoveAt(removeIndex);
        else Console.WriteLine("Invalid index. Please try again.");
        break;

    // 更新列表中的任务
    case "3":
        Console.Write("Enter task index to update: ");
        var updateIndex = Console.ReadLine() ?? "";
        Console.Write("Enter new task description: ");
        var newDescription = Console.ReadLine() ?? "";
        if(int.TryParse(updateIndex, out int idx) && idx >= 0 && idx < toDoList.Count)
            toDoList[idx] = newDescription;
        else Console.WriteLine("Invalid index. Please try again.");            
        break;

    // 退出程序
    case "4":
        Console.WriteLine("Exiting...");
        break;

    // 处理无效输入
    default:
        Console.WriteLine("Invalid option. Please try again.");
        break;
    }
}
while(userInput != "4");
