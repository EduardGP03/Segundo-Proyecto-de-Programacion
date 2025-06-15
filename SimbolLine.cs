public class SimbolLine : Simbol
{
    public string Value {get; set; }

    public SimbolLine(string value, string identifier, Token simbolToken, Type simbolType = Type.Line) : base(identifier, simbolToken, simbolType)
    {
        Value = value;
    }
}