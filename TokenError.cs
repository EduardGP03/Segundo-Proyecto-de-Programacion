public class TokenError : Token
{
    public string Mistake { get; private set;}

    public TokenError(int row, int col, string mistake, TypeToken type = TypeToken.Error) : base(row, col, type)
    {
        Mistake = mistake;
    }
}