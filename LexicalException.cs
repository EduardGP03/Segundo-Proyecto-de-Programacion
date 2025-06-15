using System;

public class LexicalException : Exception
{
    public Token TokenException {get; private set;}

    public LexicalException(Token tokenException, string message) : base(message)
    {
        TokenException = tokenException;
    }

}