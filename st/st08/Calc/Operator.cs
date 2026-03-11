
using System.Globalization;
using System.Windows.Data;

namespace Calc
{
    public enum
    Operator
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Equal,
        Empty
    }

    public class
    OperatorToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                Operator.Plus => "+",
                Operator.Minus => "-",
                Operator.Multiply => "x",
                Operator.Divide => "/",
                Operator.Equal => "=",
                Operator.Empty => "",
                _ => "",
            };
        }

        public object
        ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 今回は逆変換は不要です。
            throw new NotImplementedException();
        }
    }
}