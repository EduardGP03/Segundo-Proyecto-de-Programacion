public abstract class Simbol
{
    public string Identifier { get; private set; }
    public Token SimbolToken { get; private set; }
    public Type SimbolType {get ; set;}

    public Simbol(string identifier, Token simbolToken, Type simbolType)
    {
        Identifier = identifier;
        SimbolToken = simbolToken;
        SimbolType = simbolType;
    }

}

