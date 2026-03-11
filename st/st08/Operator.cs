using System.Globalization;
using System.Windows.Data;
namespace Calc
{
    public enum Operator
    {

        Plus,
        Minus,
        Multiply,
        Equal,
        Empty
    }
    public class OperatorToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch (value)
            {
                case Operator.Plus:
                    return "+";
                case Operator.Minus:
                    return "-";
                case Operator.Multiply:
                    return "x";
                case Operator.Equal:
                    return "=";
                case Operator.Empty:
                    return "";
                default:
                    return "";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 今回は逆変換は不要です。
            throw new NotImplementedException();

        }
    }
}