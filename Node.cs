public abstract class Node
{
    public Type NodeType { get; set; }
    public Token NodeToken { get; private set; }

    public abstract int Evaluate(ref int iterator);

    public Node(Type nodeType, Token nodeToken)
    {
        NodeType = nodeType;
        NodeToken = nodeToken;
    }

    public abstract void Show();

}