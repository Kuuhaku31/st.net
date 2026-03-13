
namespace Calc;

using System.Windows;
using System.Windows.Input;


/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    ///  处理文本输入事件，允许用户通过键盘输入算式并实时计算结果
    /// </summary>
    /// <param name="e">文本输入事件参数</param>
    protected override void
    OnPreviewTextInput(TextCompositionEventArgs e)
    {
        // 如果 DataContext 不是 ViewModel 类型，直接调用基类的事件处理方法并返回
        if(DataContext is not ViewModel viewModel)
        {
            base.OnPreviewTextInput(e);
        }

        // 如果输入的文本是 "="，则执行 ReplaceCommand 将结果显示替换为当前输入的算式，并标记事件已处理
        else if(e.Text == "=")
        {
            viewModel.ReplaceCommand.Execute(null);
            e.Handled = true;
        }

        // 如果输入的文本是一个合法的算式字符（数字、运算符或括号），则执行 EnterCharCommand 将该字符添加到当前输入的算式末尾，并标记事件已处理
        else if(e.Text.Length == 1 && "0123456789.+-*/()".Contains(e.Text[0]))
        {
            viewModel.EnterCharCommand.Execute(e.Text);
            e.Handled = true;
        }

        // 对于其他输入，调用基类的事件处理方法
        else base.OnPreviewTextInput(e);
    }

    /// <summary>
    /// 处理按键事件，允许用户通过键盘上的特殊按键（如 Backspace、Enter、Escape 等）来编辑算式和控制计算
    /// </summary>
    /// <param name="e"></param>
    protected override void
    OnPreviewKeyDown(KeyEventArgs e)
    {
        // 如果 DataContext 不是 ViewModel 类型，直接调用基类的事件处理方法并返回
        if(DataContext is not ViewModel viewModel)
        {
            base.OnPreviewKeyDown(e);
        }

        // 根据按下的键执行相应的命令来编辑算式或控制计算，并标记事件已处理
        else switch(e.Key)
        {
            // 处理 Backspace 和 Delete 键，执行 DeleteCharCommand 删除输入的最后一个字符
            case Key.Back:
            case Key.Delete:
                viewModel.DeleteCharCommand.Execute(null);
                e.Handled = true;
                return;
            case Key.Enter:
                viewModel.ReplaceCommand.Execute(null);
                e.Handled = true;
                return;
            case Key.Escape:
                viewModel.ClearCommand.Execute(null);
                e.Handled = true;
                return;

            // 处理数字键盘上的运算符按键
            case Key.OemPlus:
                if((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    viewModel.EnterCharCommand.Execute("+");
                else viewModel.ReplaceCommand.Execute(null);
                e.Handled = true;
                return;
            case Key.OemMinus:
            case Key.Subtract:
                viewModel.EnterCharCommand.Execute("-");
                e.Handled = true;
                return;
            case Key.Add:
                viewModel.EnterCharCommand.Execute("+");
                e.Handled = true;
                return;
            case Key.Multiply:
                viewModel.EnterCharCommand.Execute("*");
                e.Handled = true;
                return;
            case Key.Divide:
                viewModel.EnterCharCommand.Execute("/");
                e.Handled = true;
                return;
            case Key.Decimal:
                viewModel.EnterCharCommand.Execute(".");
                e.Handled = true;
                return;

            // 对于其他按键，调用基类的事件处理方法
            default:
                base.OnPreviewKeyDown(e);
                return;
        }
    }
}
