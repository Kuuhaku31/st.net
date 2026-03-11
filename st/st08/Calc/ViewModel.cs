
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calc
{
    internal partial class ViewModel : ObservableObject
    {
        // 计算器的核心逻辑都在 Model 类中实现，ViewModel 负责连接 Model 和 View。
        private readonly Model _model = new();

        // 下一个要执行的运算子。最开始是空的，表示还没有输入过运算子。
        [ObservableProperty]
        private Operator _nextOperator = Operator.Empty;

        // 计算结果
        [ObservableProperty]
        private int _result = 0;

        // 当前输入的数字。用户每输入一个数字，这个值都会更新。
        [ObservableProperty]
        private int _currentInput = 0;

        // 当用户按下 "C" 键时调用这个方法。它会清除当前输入的数字，但不清除计算结果和运算子。
        // 清除当前输入的数字，但不清除计算结果和运算子。
        [RelayCommand]
        private void ClearNumber()
        {
            CurrentInput = 0;
        }

        // 当用户按下 "AC" 键时调用这个方法。它会清除所有内容，重置计算器到初始状态。
        // 清除所有内容，重置计算器到初始状态。
        [RelayCommand]
        private void Clear()
        {
            // 重置 Model 中的计算状态
            _model.Clear();

            // 重置 ViewModel 中的状态
            NextOperator = Operator.Empty;
            Result = 0;
            CurrentInput = 0;
        }

        // 用户输入数字时调用这个方法。
        // 它会将当前输入的数字乘以 10（相当于在末尾添加一个零），然后加上新输入的数字。
        [RelayCommand]
        private void EnterNumber(string numStr)
        {
            // 将字符串转换为整数，并更新当前输入的数字
            CurrentInput = CurrentInput * 10 + Convert.ToInt32(numStr);
        }

        // 当用户按下运算子键（如 "+", "-", "x", "="）时调用这个方法。
        // 它会先使用当前的运算子和输入的数字来计算结果，然后更新下一个运算子和当前输入。
        [RelayCommand]
        private void EnterOperator(Operator nextOp)
        {
            // 使用当前的运算子和输入的数字来计算结果
            Result = _model.Calc(NextOperator, CurrentInput);

            // 更新下一个运算子和当前输入
            NextOperator = nextOp;
            CurrentInput = 0;
        }
    }
}