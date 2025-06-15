public class Token
{
    public TypeToken Type { get; private set; }
    public int Row { get; private set; }
    public int Col { get; private set; }

    public Token(int row, int col, TypeToken type)
    {
        Row = row;
        Col = col;
        Type = type;
    }
}