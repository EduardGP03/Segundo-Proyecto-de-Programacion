using System;

public class NodeVariable : Node
{
    public int Position {get; private set;}
    public SimbolTable Table;

    public NodeVariable(int position, SimbolTable table, Type nodeType, Token nodeToken) : base(nodeType, nodeToken)
    {
        Position = position;
        Table = table;
    }

    public override int Evaluate(ref int iterator)
    {
        return ((SimbolVariableLabel)Table.Simbols[Position]).Value;
    }

    public override void Show()
    {
        Console.WriteLine(Position);
    }

}