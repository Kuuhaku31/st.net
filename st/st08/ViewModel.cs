using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace Calc
{
    internal partial class ViewModel : ObservableObject
    {
        private Model _model;

        [ObservableProperty]
        private Operator _nextOperator = Operator.Empty;

        [ObservableProperty]
        private int _result = 0;

        [ObservableProperty]
        private int _currentInput = 0;

        [RelayCommand]
        private void ClearNumber()
        {
            CurrentInput = 0;
        }

        [RelayCommand]
        private void Clear()
        {
            _model.Clear();
            NextOperator = Operator.Empty;
            Result = 0;
            CurrentInput = 0;

        }

        [RelayCommand]
        private void EnterNumber(string numStr)
        {
            CurrentInput = CurrentInput * 10 + Convert.ToInt32(numStr);
        }

        [RelayCommand]
        private void EnterOperator(Operator nextOp)
        {

            Result = _model.Calc(NextOperator, CurrentInput);
            NextOperator = nextOp;
            CurrentInput = 0;

        }

        public ViewModel()
        {
            _model = new Model();
        }

    }
}