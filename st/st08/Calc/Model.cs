
namespace Calc;

// 模型层类
internal static class Model
{
    /// 解析并计算字符串形式的表达式
    /// 仅处理符号: 0~9 . + - * / ( )
    /// 如果无法计算，返回 null 表示错误
    ///
    /// 原理:
    /// 使用递归下降解析器实现表达式解析和计算
    /// 
    /// 1. 表达式（Expression）由加减运算符连接的项组成
    /// 2. 项（Term）由乘除运算符连接的因子组成
    /// 3. 因子（Factor）可以是一个数字或者一个括号内的表达式
    /// 
    public static double? Calc(string expression)
    {
        // 如果表达式为空，返回 null 表示无法计算
        if(string.IsNullOrWhiteSpace(expression)) return null;

        // 解析表达式的当前位置
        var pos = 0;

        // 解析表达式
        double? ParseExpression()
        {
            // 表达式的第一个一定是项
            var value = ParseTerm();
            if(value == null) return null;

            // 继续解析后续的加减运算，直到没有更多的加减运算符为止
            while(true)
            {
                SkipSpaces();

                // 解析加减运算符
                Operator op;
                if(Match('+')) op = Operator.Plus;
                else if(Match('-')) op = Operator.Minus;
                else break;

                // 解析符号后面的项
                var right = ParseTerm();
                if(right == null) return null;

                // 根据运算符进行计算
                if(op == Operator.Plus) value += right;
                else if(op == Operator.Minus) value -= right;
            }

            // 返回解析和计算的结果
            return value;
        }

        // 解析项
        double? ParseTerm()
        {
            // 项的第一个一定是因子
            var value = ParseFactor();
            if(value == null) return null;

            // 继续解析后续的乘除运算，直到没有更多的乘除运算符为止
            while(true)
            {
                SkipSpaces();

                // 解析乘除运算符
                Operator op;
                if(Match('*')) op = Operator.Multiply;
                else if(Match('/')) op = Operator.Divide;
                else break;

                // 解析符号后面的因子
                var right = ParseFactor();
                if(right == null) return null;

                // 根据运算符进行计算
                if(op == Operator.Multiply) value *= right;
                else if(op == Operator.Divide)
                {
                    if(right == 0) return null; // 除数不能为零
                    value /= right;
                }
            }

            // 返回解析和计算的结果
            return value;
        }

        // 解析因子
        double? ParseFactor()
        {
            SkipSpaces();

            // 如果因子以左括号开头，则解析括号内的表达式，并确保有对应的右括号
            if(Match('('))
            {
                // 解析括号内的表达式
                var val = ParseExpression();

                // 如果没有匹配到右括号，返回 null 表示错误
                SkipSpaces();
                return !Match(')') ? null : val;
            }

            // 否则，因子应该是一个数字，尝试解析数字并返回结果
            else return ParseNumber();
        }

        // 解析数字，支持整数和小数
        double? ParseNumber()
        {
            SkipSpaces();

            var start = pos;
            var hasDot = false;

            // 解析数字部分，直到遇到非数字字符
            while(pos < expression.Length)
            {
                var c = expression[pos];
                if(char.IsDigit(c)) pos++;
                else if(c == '.' && !hasDot)
                {
                    hasDot = true;
                    pos++;
                }
                else break;
            }
            if(start == pos) return null; // 如果没有解析到任何数字，返回 null 表示错误

            // 尝试将数字字符串解析为 double 类型，如果成功返回解析结果，否则返回 null 表示错误
            string num = expression[start..pos]; // 截取出数字字符串
            return double.TryParse(num, out double value) ? value : null;
        }

        // 匹配当前字符并前进位置
        bool Match(char c)
        {
            // 如果当前字符是 c，则前进位置并返回 true，否则返回 false
            if(pos < expression.Length && expression[pos] == c)
            {
                pos++;
                return true;
            }
            return false;
        }

        // 跳过空白字符
        void SkipSpaces()
        {
            while(pos < expression.Length && char.IsWhiteSpace(expression[pos])) pos++;
        }

        var result = ParseExpression();
        SkipSpaces();

        return pos != expression.Length ? null : result;
    }
}
