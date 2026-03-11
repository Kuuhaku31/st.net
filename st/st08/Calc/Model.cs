
namespace Calc;

// 模型层类
internal class Model
{
    private int _value = 0;
    public void Clear()
    {
        _value = 0;
    }

    public int Calc(Operator op, int num)
    {
        switch (op)
        {
            case Operator.Empty:
                // First operation is just to set the value
                _value = num;
                break;
            case Operator.Plus:
                _value += num;
                break;
            case Operator.Minus:
                _value -= num;
                break;
            case Operator.Multiply:
                _value *= num;
                break;
            case Operator.Equal:
                // Do nothing
                break;
        }
        return _value;
    }
}