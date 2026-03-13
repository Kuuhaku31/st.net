
List<string> toDoList = ["Write Monthly Report",
"Schedule Team Meeting",
"Update Project Plan"];

toDoList.Add("Review Budget");
toDoList.Remove("Update Project Plan");
toDoList.Insert(1, "Prepare Presentation");
toDoList[0] = "Write Weekly Report";

foreach(var item in toDoList)
{
    Console.WriteLine(item);
}

Console.WriteLine($"The first task is {toDoList[0]}");
