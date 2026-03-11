
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace st07
{
    // 视图模型层
    internal partial class
    ViewModel : ObservableObject
    {
        // 模型层实例
        private readonly Model _model;

        // 输入的身高（厘米）
        [ObservableProperty]
        // private double _cmHeight;
        private double _feetHeight;

        // 输入的体重（千克）
        [ObservableProperty]
        // private double _kgWeight;
        private double _poundsWeight;

        // 计算得到的BMI值
        [ObservableProperty]
        private double _bmi;

        // 构造函数，初始化模型层实例
        public
        ViewModel()
        {
            _model = new Model();
        }

        // 计算BMI的命令方法
        [RelayCommand]
        private void CalculateBMI()
        {
            // 将输入的身高从厘米转换为米
            // double meterH = CmHeight / 100;
            double meterH = FeetHeight / 3.2808;
            double kgWeight = PoundsWeight / 2.2046;

            // 调用模型层的计算方法
            // double originalBmi = Model.Calc(meterH, KgWeight);
            double originalBmi = Model.Calc(meterH, kgWeight);

            // 将计算得到的BMI值保留两位小数
            Bmi = Math.Round(originalBmi, 2);
        }
    }
}
