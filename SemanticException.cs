using System;

public class SemanticException : Exception
{
    public Token TokenException {get; private set;}

    public SemanticException(Token tokenException, string message) : base (message)
    {
        TokenException = tokenException;
    }
}