using System;

public class NodeOperator : Node
{
    Node Left;
    Node Right;
    SimbolTable Table;

    public NodeOperator(Node left, Node right, SimbolTable table, Type nodeType, Token nodeToken) : base(nodeType, nodeToken)
    {
        Left = left;
        Right = right;
        Table = table;
    }

    public override int Evaluate(ref int iterator)
    {
        if (NodeToken.Type == TypeToken.GoTo)
        {
            int value = Left.Evaluate(ref iterator);

            if(((SimbolVariableLabel)Table.Simbols[value]).Value == -1)
                throw new Exception("etiqueta <" + Table.Simbols[value].Identifier + "> no definida");

            iterator++;

            if (Right.Evaluate(ref iterator) == 1)
                iterator = ((SimbolVariableLabel)Table.Simbols[value]).Value;

            return 0;
        }

        else if(NodeToken.Type == TypeToken.Arrow)
        {
            int pos = ((NodeVariable)Left).Position;
            int value = Right.Evaluate(ref iterator);
            ((SimbolVariableLabel)Table.Simbols[pos]).Value = value;
            Table.Simbols[pos].SimbolType = Right.NodeType;

            iterator++;

            return 0;   
        }

        else if (NodeToken.Type == TypeToken.Plus)
            return Left.Evaluate(ref iterator) + Right.Evaluate(ref iterator);

        else if (NodeToken.Type == TypeToken.Minus)
            return Left.Evaluate(ref iterator) - Right.Evaluate(ref iterator);

        else if (NodeToken.Type == TypeToken.Product)
            return Left.Evaluate(ref iterator) * Right.Evaluate(ref iterator);

        else if (NodeToken.Type == TypeToken.Division)
            return Left.Evaluate(ref iterator) / Right.Evaluate(ref iterator);

        else if (NodeToken.Type == TypeToken.Power)
            return (int)Math.Pow(Left.Evaluate(ref iterator), Right.Evaluate(ref iterator));

        else if (NodeToken.Type == TypeToken.Modulo)
            return Left.Evaluate(ref iterator) % Right.Evaluate(ref iterator);

        else if (NodeToken.Type == TypeToken.And)
        {
            if (Left.Evaluate(ref iterator) == 1 && Right.Evaluate(ref iterator) == 1)
                return 1;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.Or)
        {
            if (Left.Evaluate(ref iterator) == 0 && Right.Evaluate(ref iterator) == 0)
                return 0;

            return 1;
        }

        else if (NodeToken.Type == TypeToken.Less)
        {
            if (Left.Evaluate(ref iterator) < Right.Evaluate(ref iterator))
                return 1;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.LessEquals)
        {
            if (Left.Evaluate(ref iterator) <= Right.Evaluate(ref iterator))
                return 1;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.EqualsEquals)
        {
            if (Left.Evaluate(ref iterator) == Right.Evaluate(ref iterator))
                return 1;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.GreaterEquals)
        {
            if (Left.Evaluate(ref iterator) >= Right.Evaluate(ref iterator))
                return 1;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.Greater)
        {
            if (Left.Evaluate(ref iterator) > Right.Evaluate(ref iterator))
                return 1;

            return 0;
        }

        else
        {
            int value = Right.Evaluate(ref iterator);

            iterator++;

            ((SimbolVariableLabel)Table.Simbols[Left.Evaluate(ref iterator)]).Value = value;

            return 0;
        }
    }

    public override void Show()
    {
        Console.WriteLine(NodeToken.Type);
        Left.Show();
        Right.Show();
    }

}