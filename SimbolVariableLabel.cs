public class SimbolVariableLabel : Simbol
{
    public int Value { get; set; }
    public bool IsNull = true;


    public SimbolVariableLabel(int value, string identifier, Token simbolToken, Type simbolType) : base(identifier, simbolToken, simbolType)
    {
        Value = value;
    }
}