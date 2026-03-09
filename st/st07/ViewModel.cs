using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BMI
{
    internal partial class ViewModel: ObservableObject
    {
        private Model _model;

        [ObservableProperty]
        private double _cmHeight;

        [ObservableProperty]
        private double _kgWeight;

        [ObservableProperty]
        private double _bmi;

        public ViewModel()
        {
            _model = new Model();
        }

        [RelayCommand]
        private void CalculateBMI()
        {
            double meterH = CmHeight / 100;

            double originalBmi = _model.Calc(meterH, KgWeight);
            
            // Round the BMI to 2 decimal places
            Bmi = Math.Round(originalBmi, 2);
        }
    }
}
