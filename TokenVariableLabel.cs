public class TokenVariableLabel : Token
{
    public string Name{get; private set;}

    public TokenVariableLabel(string name, int row, int col, TypeToken type) : base(row, col, type)
    {
        Name = name;
    }
}