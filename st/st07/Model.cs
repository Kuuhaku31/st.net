using System.IO;
using System.Windows.Media.Media3D;

namespace BMI
{
    internal class Model
    {
       public double Calc(double meterHeight, double kgWeight)
        {
            double bmi = kgWeight / (meterHeight * meterHeight);

            File.WriteAllText("bmi.txt", bmi.ToString());

            return bmi;
        }
    }
}
