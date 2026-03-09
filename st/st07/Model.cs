
using System.IO;

namespace st07
{
    // 模型层
    internal class
    Model
    {
        public static double
        Calc(double meterHeight, double kgWeight)
        {
            double bmi = kgWeight / (meterHeight * meterHeight);

            File.WriteAllText("bmi.txt", bmi.ToString());

            return bmi;
        }
    }
}
