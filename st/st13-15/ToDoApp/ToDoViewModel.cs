
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace ToDoApp;


internal partial class ToDoViewModel : ObservableObject
{
    private readonly ToDoModel _model;

    public ObservableCollection<ToDo> ListViewRows { get; set; }

    public ToDoViewModel()
    {
        _model = new();
        ListViewRows = [];

        // 添加数据到 ListViewRows
        foreach (var todo in _model.ToDos)
        {
            ListViewRows.Add(todo);
        }
    }
}
