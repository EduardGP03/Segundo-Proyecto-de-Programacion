using System;

public class NodeLine : Node
{
    int Position;

    public NodeLine(int position, Type nodeType, Token nodeToken) : base(nodeType, nodeToken)
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
