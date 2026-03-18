
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ToDoApp;


/// <summary>
/// 视图模型层
/// 包含 ToDo 列表和用于添加新 ToDo 的属性
/// 并监听 ToDo 对象的属性变化以调用相应的更新方法
/// </summary>
internal partial class
ToDoViewModel: ObservableObject
{
    // 定义一个常量字符串作为新 ToDo 项目的默认名称
    private const string TO_DO_NAME_DEFAULT = "New ToDo";

    // 定义一个 ObservableCollection 来存储 ToDo 列表
    public ObservableCollection<ToDo> ListViewRows { get; set; }

    // 定义用于添加新 ToDo 的属性
    [ObservableProperty]
    private string _newToDoName = TO_DO_NAME_DEFAULT;

    // 定义用于添加新 ToDo 的截止日期属性，默认为今天
    [ObservableProperty]
    private DateTime _newToDoDeadline = DateTime.Today;

    // 定义用于添加新 ToDo 的优先级属性，默认为null
    [ObservableProperty]
    private int? _newToDoPriority = null;

    // 构造函数，初始化 ToDo 列表并监听每个 ToDo 的属性变化
    public ToDoViewModel()
    {
        // 初始化 ListViewRows 数组
        ListViewRows = [];
        foreach(var todo in ToDoModel.ToDos)
        {
            ListViewRows.Add(todo);
            todo.PropertyChanged += ToDoPropertyChanged; // 监听每个 ToDo 的属性变化
        }
    }

    /// <summary>
    /// 添加新的 ToDo 项目到列表中，并调用模型的添加方法
    /// </summary>
    [RelayCommand]
    private void AddToDo()
    {
        // 由数据库生成自增主键 ID
        var todo  = new ToDo(name: NewToDoName, deadline: NewToDoDeadline, priority: NewToDoPriority ?? 1);
        todo.PropertyChanged += ToDoPropertyChanged;

        // 先落库，保存后对象会带上数据库生成的 Id
        var saved = ToDoModel.Add(todo);
        ListViewRows.Add(saved);

        // 重置输入框的值
        NewToDoName     = TO_DO_NAME_DEFAULT;
        NewToDoDeadline = DateTime.Today;
        NewToDoPriority = null;
    }

    /// <summary>
    /// 从列表中删除指定的 ToDo 项目，并调用模型的删除方法
    /// </summary>
    /// <param name="parameter"></param>
    [RelayCommand]
    private void DeleteToDo(object parameter)
    {
        // 如果参数是 ToDo 对象，从列表中删除该对象，并调用模型的删除方法
        if(parameter is ToDo item)
        {
            ListViewRows.Remove(item);
            ToDoModel.Delete(item);
        }
    }

    /// <summary>
    /// 监听 ToDo 对象的属性变化，并调用相应的更新方法
    /// </summary>
    /// <param name="sender">传入的 ToDo 对象</param>
    /// <param name="e">属性变化事件参数</param>
    private void 
    ToDoPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // 如果 sender 是 ToDo 对象，根据属性名称调用相应的更新方法
        if(sender is ToDo todo) switch (e.PropertyName)
        {
        case nameof(ToDo.Name):      ToDoModel.UpdateName     (todo, todo.Name);      break; // 更新名称
        case nameof(ToDo.Deadline):  ToDoModel.UpdateDeadline (todo, todo.Deadline);  break; // 更新截止日期
        case nameof(ToDo.Priority):  ToDoModel.UpdatePriority (todo, todo.Priority);  break; // 更新优先级
        case nameof(ToDo.Completed): ToDoModel.UpdateCompleted(todo, todo.Completed); break; // 更新完成状态
        default: break;
        }
    }
}
