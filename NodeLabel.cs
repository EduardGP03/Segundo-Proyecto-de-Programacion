using System;

public class NodeLabel : Node
{
    public int Position {get; private set;}

    public NodeLabel(int position, Type nodeType, Token nodeToken) : base(nodeType, nodeToken)
    {
        Position = position;
    }

    public override int Evaluate(ref int iterator)
    {
        return Position;
    }


    public override void Show()
    {
        Console.WriteLine(Position);
    }

}