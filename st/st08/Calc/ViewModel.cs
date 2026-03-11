
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calc;

internal partial class ViewModel : ObservableObject
{
    // 当前输入的算式
    [ObservableProperty]
    private string _expression = "";

    // 计算结果
    [ObservableProperty]
    private string _result = "";


    // 输入字符时调用
    [RelayCommand]
    private void EnterChar(string charStr)
    {
        Expression += charStr; // 添加到当前输入的算式末尾
        FlushResult();         // 刷新结果显示
    }

    // 按下 Backspace 时调用
    [RelayCommand]
    private void DeleteChar()
    {
        if (Expression.Length == 0) return; // 如果没有输入，直接返回

        Expression = Expression[..^1]; // 删除输入的最后一个字符
        FlushResult();                 // 刷新结果显示
    }

    // 按下 Enter 或者 = 时调用
    [RelayCommand]
    private void Replace()
    {
        Expression = Result; // 将结果显示替换为当前输入的算式
        FlushResult();       // 刷新结果显示
    }

    // 按下 C 键时调用
    // 清空当前的输入和结果显示
    [RelayCommand]
    private void Clear()
    {
        Expression = ""; // 清空输入
        Result = "";     // 清空结果显示
    }


    // 刷新结果显示
    private void FlushResult()
    {
        // 如果表达式为空，结果显示也应该为空
        if (Expression == "")
        {
            Result = "";
            return;
        }

        var value = Model.Calc(Expression); // 计算当前输入的算式
        if (value.HasValue) Result = value.Value.ToString();
        else Result = "Error";
    }
}