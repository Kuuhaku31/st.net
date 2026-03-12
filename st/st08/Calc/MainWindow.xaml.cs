using System.Windows;
using System.Windows.Input;

namespace Calc;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void
    OnPreviewTextInput(TextCompositionEventArgs e)
    {
        if (DataContext is not ViewModel viewModel)
        {
            base.OnPreviewTextInput(e);
            return;
        }

        if (e.Text == "=")
        {
            viewModel.ReplaceCommand.Execute(null);
            e.Handled = true;
            return;
        }

        if (e.Text.Length == 1 && "0123456789.+-*/()".Contains(e.Text[0]))
        {
            viewModel.EnterCharCommand.Execute(e.Text);
            e.Handled = true;
            return;
        }

        base.OnPreviewTextInput(e);
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (DataContext is not ViewModel viewModel)
        {
            base.OnPreviewKeyDown(e);
            return;
        }

        switch (e.Key)
        {
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
            case Key.OemPlus:
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    viewModel.EnterCharCommand.Execute("+");
                }
                else
                {
                    viewModel.ReplaceCommand.Execute(null);
                }

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
        }

        base.OnPreviewKeyDown(e);
    }
}
