public class TokenNumber : Token
{
    public int Number{get; private set;}

    public TokenNumber(int number, int row, int col,TypeToken type) : base(row, col, type)
    {
        Number = number;
    }

}