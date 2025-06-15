using System;

public class SintacticException : Exception
{
    public Token TokenException {get; private set;}

    public SintacticException(Token tokenException, string message) : base (message)
    {
        TokenException = tokenException;
    }
}