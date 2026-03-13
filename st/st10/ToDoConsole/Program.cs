var toDoList = new List<string>();
string userInput;
do
{
    Console.WriteLine("\nToDo List:");
    foreach (var item in toDoList)
    {
        Console.WriteLine("- " + item);
    }

    Console.WriteLine(@"
Choose an option:
1. Add Task
2. Remove Task
3. Exit");
    userInput = Console.ReadLine() ?? "";

    switch (userInput)
    {
        case "1":
            Console.Write("Enter a task to add: ");
            var newTask = Console.ReadLine() ?? "";
            // Add task to list




            break;
        case "2":
            Console.Write("Enter task index to remove: ");
            var index = Console.ReadLine() ?? "";
            // Remove task from list




            break;
    }
} while (userInput != "3");