using System;

public class NodeNumber : Node
{
    int Number;

    public NodeNumber(int number, Type nodeType, Token nodeToken) : base(nodeType, nodeToken)
    {
        Number = number;
    }

    public override int Evaluate(ref int iterator)
    {
        return Number;
    }

    public override void Show()
    {
        Console.WriteLine(Number);
    }
}