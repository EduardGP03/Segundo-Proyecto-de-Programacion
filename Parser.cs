using System;
using System.Collections.Generic;

public class Parser
{
    Token CurrentToken;
    Lexer Lexer;
    List<Node> Tree = new List<Node>();
    SimbolTable Table;
    Canvas Screen;

    public List<Node> GetTree()
    {
        return Tree;
    }

    public Parser(Lexer lexer, SimbolTable table, Canvas screen)
    {
        Lexer = lexer;
        CurrentToken = lexer.TakeToken();
        Screen = screen;
        Table = table;
    }

    void Match(TypeToken type)
    {
        if (type == CurrentToken.Type)
        {
            CurrentToken = Lexer.TakeToken();

            if (CurrentToken is TokenError)
                throw new LexicalException(CurrentToken, "ERROR: Caracter No Valido");
        }

        else
            throw new SintacticException(CurrentToken, "ERROR: Token <" + CurrentToken.Type + "> erroneo. Se esperaba <" + type + ">");
    }

    public void Parse()
    {
        Language();
        Console.WriteLine("Compilacion exitosa ");
    }

    void Language()
    {
        if (CurrentToken.Type == TypeToken.Spawn)
        {
            Node spawn = Spawn();
            Tree.Add(spawn);
            ListInstruction();
        }

        else
            throw new SintacticException(CurrentToken, "El programa debe comenzar con Spawn ");
    }

    Node Spawn()
    {
        if (CurrentToken.Type == TypeToken.Spawn)
        {
            Token token = CurrentToken;
            Match(TypeToken.Spawn);
            return PositionSpawn(token);
        }

        else
            throw new SintacticException(CurrentToken, "El programa debe comenzar con Spawn ");
    }

    Node PositionSpawn(Token token)
    {
        if (CurrentToken.Type == TypeToken.OpenParenthesis)
        {
            Match(TypeToken.OpenParenthesis);
            Node expression1 = Expression();
            Match(TypeToken.Comma);
            Node expression2 = Expression();
            Match(TypeToken.CloseParenthesis);
            List<Node> list = new List<Node>();
            list.Add(expression1);
            list.Add(expression2);
            return new NodeFunction(list, Screen, Table, Type.Null, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba un parentecis ");
    }

    void ListInstruction()
    {
        if (CurrentToken.Type == TypeToken.NewLine)
        {
            Match(TypeToken.NewLine);
            Node instruction = Instruction();
            if (instruction is NodeLabel)
            {
                int pos = ((NodeLabel)instruction).Position;
                if (((SimbolVariableLabel)Table.Simbols[pos]).Value == -1)
                    ((SimbolVariableLabel)Table.Simbols[pos]).Value = Tree.Count;
                else
                    throw new SemanticException(instruction.NodeToken, "Etiqueta redefinida ");
            }
            else if (instruction == null) { }
            else
                Tree.Add(instruction);
            ListInstruction();
        }

        else if (CurrentToken.Type == TypeToken.End)
        {

        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba un salto de linea o fin del programa ");
    }

    Node Instruction()
    {
        TypeToken[] typeToken = { TypeToken.Size, TypeToken.Fill, TypeToken.Color, TypeToken.DrawLine, TypeToken.DrawCircle, TypeToken.DrawRectangle };

        if (Tools.Search(typeToken, CurrentToken))
            return Command();

        else if (CurrentToken.Type == TypeToken.Variable)
            return Asing();

        else if (CurrentToken.Type == TypeToken.Label)
            return Label();

        else if (CurrentToken.Type == TypeToken.GoTo)
            return GoTo();

        else if (CurrentToken.Type == TypeToken.End || CurrentToken.Type == TypeToken.NewLine)
        {
            return null;
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba un comando, variable, etiqueta, goto, neva linea o un fin del programa ");
    }

    Node Command()
    {
        Token token = CurrentToken;
        List<Node> list = new List<Node>();

        if (CurrentToken.Type == TypeToken.Color)
        {
            Match(TypeToken.Color);
            Match(TypeToken.OpenParenthesis);
            Node line = Line();
            Match(TypeToken.CloseParenthesis);

            list.Add(line);

            return new NodeFunction(list, Screen, Table, Type.Null, token);
        }

        else if (CurrentToken.Type == TypeToken.Size)
        {
            Match(TypeToken.Size);
            Match(TypeToken.OpenParenthesis);
            Node expression = Expression();
            Match(TypeToken.CloseParenthesis);

            list.Add(expression);

            return new NodeFunction(list, Screen, Table, Type.Null, token);
        }

        else if (CurrentToken.Type == TypeToken.DrawLine)
        {
            Match(TypeToken.DrawLine);
            Match(TypeToken.OpenParenthesis);
            Node expression_1 = Expression();
            Match(TypeToken.Comma);
            Node expression_2 = Expression();
            Match(TypeToken.Comma);
            Node expression_3 = Expression();
            Match(TypeToken.CloseParenthesis);

            list.Add(expression_1);
            list.Add(expression_2);
            list.Add(expression_3);

            return new NodeFunction(list, Screen, Table, Type.Null, token);
        }

        else if (CurrentToken.Type == TypeToken.DrawCircle)
        {
            Match(TypeToken.DrawCircle);
            Match(TypeToken.OpenParenthesis);
            Node expression_1 = Expression();
            Match(TypeToken.Comma);
            Node expression_2 = Expression();
            Match(TypeToken.Comma);
            Node expression_3 = Expression();
            Match(TypeToken.CloseParenthesis);

            list.Add(expression_1);
            list.Add(expression_2);
            list.Add(expression_3);

            return new NodeFunction(list, Screen, Table, Type.Null, token);
        }

        else if (CurrentToken.Type == TypeToken.DrawRectangle)
        {
            Match(TypeToken.DrawRectangle);
            Match(TypeToken.OpenParenthesis);
            Node expression_0 = Expression();
            Match(TypeToken.Comma);
            Node expression_1 = Expression();
            Match(TypeToken.Comma);
            Node expression_2 = Expression();
            Match(TypeToken.Comma);
            Node expression_3 = Expression();
            Match(TypeToken.Comma);
            Node expression_4 = Expression();
            Match(TypeToken.CloseParenthesis);

            list.Add(expression_0);
            list.Add(expression_1);
            list.Add(expression_2);
            list.Add(expression_3);
            list.Add(expression_4);

            return new NodeFunction(list, Screen, Table, Type.Null, token);
        }

        else if (CurrentToken.Type == TypeToken.Fill)
        {
            Match(TypeToken.Fill);
            Match(TypeToken.OpenParenthesis);
            Match(TypeToken.CloseParenthesis);

            return new NodeFunction(list, Screen, Table, Type.Null, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba un comando ");
    }

    Node Asing()
    {
        Token token = CurrentToken;

        if (CurrentToken.Type == TypeToken.Variable)
        {
            Match(TypeToken.Variable);
            string name = ((TokenVariableLabel)token).Name;

            int pos = Table.SearchPosition(name);
            ((SimbolVariableLabel)Table.Simbols[pos]).IsNull = false;

            Token token_1 = CurrentToken;
            Match(TypeToken.Arrow);
            Node expression = Expression();

            ((SimbolVariableLabel)Table.Simbols[pos]).SimbolType = expression.NodeType;

            NodeVariable nodeVariable = new NodeVariable(Table.SearchPosition(name), Table, Type.Null, token);
            return new NodeOperator(nodeVariable, expression, Table, expression.NodeType, token_1);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una variable ");
    }

    Node Expression()
    {
        TypeToken[] typeToken = { TypeToken.Variable, TypeToken.OpenParenthesis, TypeToken.Number, TypeToken.True, TypeToken.False, TypeToken.GetActualY, TypeToken.GetActualX, TypeToken.IsBrushSize, TypeToken.IsBrushColor, TypeToken.IsCanvasColor, TypeToken.GetCanvasSize, TypeToken.GetColorCount };

        if (Tools.Search(typeToken, CurrentToken))
        {
            Node term_1 = And();
            return ExpressionRight(term_1);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una variable, (, numero, true, false, funcion ");
    }

    Node ExpressionRight(Node term_1)
    {
        TypeToken[] typeToken = { TypeToken.And };
        TypeToken[] typeToken1 = { TypeToken.End, TypeToken.Comma, TypeToken.CloseParenthesis, TypeToken.NewLine };

        if (Tools.Search(typeToken, CurrentToken))
        {
            Token token = CurrentToken;
            Match(TypeToken.And);
            Node and = And();

            if (term_1.NodeType != Type.Boolean || and.NodeType != Type.Boolean)
                throw new SemanticException(token, "El operador && tiene que ser usado con variables buleanas ");

            NodeOperator operation = new NodeOperator(term_1, and, Table, Type.Boolean, token);

            return ExpressionRight(operation);
        }

        else if (Tools.Search(typeToken1, CurrentToken))
        {
            return term_1;
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba un &&, coma, parentesis cerrado, un cambio de linea o un final del programa ");
    }

    Node And()
    {
        TypeToken[] typeToken = { TypeToken.Variable, TypeToken.OpenParenthesis, TypeToken.Number, TypeToken.True, TypeToken.False, TypeToken.GetActualY, TypeToken.GetActualX, TypeToken.IsBrushSize, TypeToken.IsBrushColor, TypeToken.IsCanvasColor, TypeToken.GetCanvasSize, TypeToken.GetColorCount };

        if (Tools.Search(typeToken, CurrentToken))
        {
            Node or = Or();

            return AndRight(or);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una variable, (, true, false o una funcion ");
    }

    Node AndRight(Node node)
    {
        TypeToken[] typeToken = { TypeToken.End, TypeToken.Comma, TypeToken.And, TypeToken.OpenParenthesis, TypeToken.NewLine };
        TypeToken[] typeToken1 = { TypeToken.End, TypeToken.Comma, TypeToken.And, TypeToken.CloseParenthesis, TypeToken.NewLine};

        if (CurrentToken.Type == TypeToken.Or)
        {
            Token token = CurrentToken;
            Match(TypeToken.Or);
            Node or = Or();

            if (or.NodeType != Type.Boolean || node.NodeType != Type.Boolean)
                throw new SemanticException(token, "El || debe operar con buleanos ");

            NodeOperator node_1 = new NodeOperator(node, or, Table, Type.Boolean, token);

            return AndRight(node_1);
        }

        else if (Tools.Search(typeToken1, CurrentToken))
            return node;

        else
            throw new SintacticException(CurrentToken, "Se esperaba coma, ||, &&, (, salto de linea o final del programa");
    }

    Node Or()
    {
        TypeToken[] typeToken = { TypeToken.Variable, TypeToken.OpenParenthesis, TypeToken.Number, TypeToken.True, TypeToken.False, TypeToken.GetActualX, TypeToken.GetActualY, TypeToken.IsBrushSize, TypeToken.IsBrushColor, TypeToken.IsCanvasColor, TypeToken.GetCanvasSize, TypeToken.GetColorCount };

        if (Tools.Search(typeToken, CurrentToken))
        {
            Node node = ExpressionPart();

            return OrRight(node);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una variable, ( , numero, true, false o una funcion ");
    }

    Node OrRight(Node node)
    {
        TypeToken[] typeToken = { TypeToken.EqualsEquals, TypeToken.Greater, TypeToken.Less, TypeToken.Distint, TypeToken.GreaterEquals, TypeToken.LessEquals };
        TypeToken[] typeToken1 = { TypeToken.End, TypeToken.Comma, TypeToken.Or, TypeToken.And, TypeToken.CloseParenthesis, TypeToken.NewLine};

        if (Tools.Search(typeToken, CurrentToken))
        {

            Token token = CurrentToken;

            Node node_1 = Relation();
            Node node_2 = ExpressionPart();

            if ((node_1.NodeToken.Type == TypeToken.EqualsEquals || node_1.NodeToken.Type == TypeToken.Distint) && node.NodeType != node_2.NodeType)
                throw new SemanticException(token, "El == y != deben trabajar con el mismo tipo de variable ");

            if (!(node_1.NodeToken.Type == TypeToken.EqualsEquals || node_1.NodeToken.Type == TypeToken.Distint) && (node.NodeType != Type.Integer || node_2.NodeType != Type.Integer))
                throw new SemanticException(token, "Los operadores <, <=, >, >= deben operar con enteros ");

            NodeOperator node_3 = new NodeOperator(node, node_2, Table, node_1.NodeType, token);

            return OrRight(node_3);
        }

        else if(Tools.Search(typeToken1, CurrentToken))
            return node;

        else
            throw new SintacticException(CurrentToken, "Se esperaba una coma, ||, (, salto de linea o fin del programa ");
    }

    Node ExpressionPart()
    {
        TypeToken[] typeToken = { TypeToken.Variable, TypeToken.OpenParenthesis, TypeToken.Number, TypeToken.True, TypeToken.False, TypeToken.GetActualX, TypeToken.GetActualY, TypeToken.IsBrushSize, TypeToken.IsBrushColor, TypeToken.IsCanvasColor, TypeToken.GetCanvasSize, TypeToken.GetColorCount };
    
        if(Tools.Search(typeToken, CurrentToken))
        {
            Node node = Term();
            return ExpressionPartRight(node);
        }

        else 
            throw new SintacticException(CurrentToken, "Se esperaba variable, (, numero, true, false o una funcion ");
    }

    Node ExpressionPartRight(Node node)
    {
        TypeToken[] typeToken = {TypeToken.End,TypeToken.Or, TypeToken.Comma, TypeToken.And, TypeToken.CloseParenthesis, TypeToken.NewLine, TypeToken.EqualsEquals, TypeToken.Greater, TypeToken.Less, TypeToken.Distint,TypeToken.GreaterEquals, TypeToken.LessEquals};
        
        if(CurrentToken.Type == TypeToken.Plus || CurrentToken.Type == TypeToken.Minus)
        {
            Token token = CurrentToken;
            Node node_1 = PlusMinus
    ();
            Node node_2 = Term();
            
            if(node.NodeType != Type.Integer || node_2.NodeType != Type.Integer)
                throw new SemanticException(token, "El operador + y - trabajan con numeros enteros ");
        
            NodeOperator node_3 = new NodeOperator(node, node_2, Table, Type.Integer, token);
        
            return ExpressionPartRight(node_3);
        }

        else if(Tools.Search(typeToken, CurrentToken))
            return node;

        else 
            throw new SintacticException(CurrentToken, "Se esperaba ||, &&, (, salto de linea, <, <=, >, >=, ==, != o fin del programa ");
    }

    Node Term()
    {
        TypeToken[] typeToken = { TypeToken.Variable, TypeToken.OpenParenthesis, TypeToken.Number, TypeToken.True, TypeToken.False, TypeToken.GetActualX, TypeToken.GetActualY, TypeToken.IsBrushSize, TypeToken.IsBrushColor, TypeToken.IsCanvasColor, TypeToken.GetCanvasSize, TypeToken.GetColorCount };

        if (Tools.Search(typeToken, CurrentToken))
        {
            Node factor = Factor();
            return TermRight(factor);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba variable, (, numero, true, false o una funcion ");
    }

    Node TermRight(Node factor_1)
    {
        TypeToken[] typeToken = { TypeToken.Product, TypeToken.Division, TypeToken.Modulo };
        TypeToken[] typeToken1 = { TypeToken.End, TypeToken.Comma, TypeToken.And, TypeToken.Or, TypeToken.Plus, TypeToken.CloseParenthesis, TypeToken.NewLine, TypeToken.EqualsEquals, TypeToken.Distint, TypeToken.Greater, TypeToken.Less, TypeToken.Minus, TypeToken.GreaterEquals, TypeToken.LessEquals };

        if (Tools.Search(typeToken, CurrentToken))
        {
            Token token = CurrentToken;
            Node productDivisionOr = ProductDivision();
            Node factor_2 = Factor();

            if (factor_1.NodeType != Type.Integer || factor_2.NodeType != Type.Integer)
                throw new SemanticException(productDivisionOr.NodeToken, "El operadores *, / o % deben trabajar con enteros ");

            NodeOperator ope = new NodeOperator(factor_1, factor_2, Table, Type.Integer, token);
            return TermRight(ope);
        }

        else if (Tools.Search(typeToken1, CurrentToken))
        {
            return factor_1;
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una or, producto, division, coma, and, +, -, ), nueva linea, operador de comparacion o fin del programa ");
    }

    Node Factor()
    {
        TypeToken[] typeToken = { TypeToken.Variable, TypeToken.OpenParenthesis, TypeToken.Number, TypeToken.True, TypeToken.False, TypeToken.GetActualY, TypeToken.GetActualX, TypeToken.IsBrushSize, TypeToken.IsBrushColor, TypeToken.IsCanvasColor, TypeToken.GetCanvasSize, TypeToken.GetColorCount };

        if (Tools.Search(typeToken, CurrentToken))
        {
            Node data = Data();
            return FactorRight(data);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una variable, (, numero, true, false, funcion ");
    }

    Node FactorRight(Node data_1)
    {
        TypeToken[] typeToken = { TypeToken.End, TypeToken.Comma, TypeToken.Or, TypeToken.And, TypeToken.Plus, TypeToken.Product, TypeToken.Division, TypeToken.CloseParenthesis, TypeToken.NewLine, TypeToken.EqualsEquals, TypeToken.Greater, TypeToken.Less, TypeToken.Minus, TypeToken.GreaterEquals, TypeToken.LessEquals, TypeToken.Modulo, TypeToken.Distint };

        if (CurrentToken.Type == TypeToken.Power)
        {
            Token token = CurrentToken;
            Match(TypeToken.Power);
            Node data_2 = Data();

            if (data_1.NodeType != Type.Boolean || data_2.NodeType != Type.Boolean)
                throw new SemanticException(token, "El operador ** debe operar con valores enteros ");
            NodeOperator ope = new NodeOperator(data_1, data_2, Table, Type.Integer, token);
            return FactorRight(ope);
        }

        else if (Tools.Search(typeToken, CurrentToken))
        {
            return data_1;
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba potencia, coma, or, and,+,-,*,/,),nueva linea, operador de comparacion o final del programa ");
    }

    Node Data()
    {
        TypeToken[] typeToken = { TypeToken.GetActualX, TypeToken.GetActualY, TypeToken.IsBrushSize, TypeToken.IsBrushColor, TypeToken.IsCanvasColor, TypeToken.GetCanvasSize, TypeToken.GetColorCount };
        Token token = CurrentToken;

        if (CurrentToken.Type == TypeToken.Variable)
        {
            string name = ((TokenVariableLabel)token).Name;
            Match(TypeToken.Variable);


            int position = Table.SearchPosition(name);
            if (((SimbolVariableLabel)Table.Simbols[position]).IsNull == true)
                throw new SemanticException(token, "Variable no definida");

            return new NodeVariable(position, Table, Table.Simbols[position].SimbolType, token);
        }

        else if (CurrentToken.Type == TypeToken.Number)
        {
            int number = ((TokenNumber)token).Number;
            Match(TypeToken.Number);

            return new NodeNumber(number, Type.Integer, token);
        }

        else if (Tools.Search(typeToken, CurrentToken))
        {
            return Function();
        }

        else if (CurrentToken.Type == TypeToken.True || CurrentToken.Type == TypeToken.False)
        {
            return DataBool();
        }

        else if (CurrentToken.Type == TypeToken.OpenParenthesis)
        {

            Match(TypeToken.OpenParenthesis);
            Node expression = Expression();
            Match(TypeToken.CloseParenthesis);

            return expression;
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una funcion, variable, numero, true, false o ( ");
    }

    Node DataBool()
    {
        Token token = CurrentToken;

        if (CurrentToken.Type == TypeToken.True)
        {
            Match(TypeToken.True);
            return new NodeNumber(1, Type.Boolean, token);
        }

        else if (CurrentToken.Type == TypeToken.False)
        {
            Match(TypeToken.False);
            return new NodeNumber(0, Type.Boolean, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba true o false ");
    }

    Node PlusMinus()
    {
        Token token = CurrentToken;
        Node node = new NodeNumber(1, Type.Null, token);

        if (CurrentToken.Type == TypeToken.Plus)
        {
            Match(TypeToken.Plus);

            return new NodeOperator(node, node, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.Minus)
        {
            Match(TypeToken.Minus);

            return new NodeOperator(node, node, Table, Type.Integer, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba + o -");
    }

    Node ProductDivision()
    {
        Token token = CurrentToken;
        Node node = new NodeNumber(1, Type.Null, token);

        if (CurrentToken.Type == TypeToken.Product)
        {
            Match(TypeToken.Product);

            return new NodeOperator(node, node, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.Division)
        {
            Match(TypeToken.Division);

            return new NodeOperator(node, node, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.Modulo)
        {
            Match(TypeToken.Modulo);

            return new NodeOperator(node, node, Table, Type.Integer, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba *, / o %");
    }

    Node Relation()
    {
        Token token = CurrentToken;
        Node node = new NodeNumber(1, Type.Null, token);

        if (CurrentToken.Type == TypeToken.EqualsEquals)
        {
            Match(TypeToken.EqualsEquals);

            return new NodeOperator(node, node, Table, Type.Boolean, token);
        }

        else if (CurrentToken.Type == TypeToken.GreaterEquals)
        {
            Match(TypeToken.GreaterEquals);

            return new NodeOperator(node, node, Table, Type.Boolean, token);
        }

        else if (CurrentToken.Type == TypeToken.LessEquals)
        {
            Match(TypeToken.LessEquals);

            return new NodeOperator(node, node, Table, Type.Boolean, token);
        }

        else if (CurrentToken.Type == TypeToken.Greater)
        {
            Match(TypeToken.Greater);

            return new NodeOperator(node, node, Table, Type.Boolean, token);
        }

        else if (CurrentToken.Type == TypeToken.Less)
        {
            Match(TypeToken.Less);

            return new NodeOperator(node, node, Table, Type.Boolean, token);
        }

        else if (CurrentToken.Type == TypeToken.Distint)
        {
            Match(TypeToken.Distint);

            return new NodeOperator(node, node, Table, Type.Boolean, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba un operador de comparacion ");
    }

    Node Function()
    {
        List<Node> list = new List<Node>();
        Token token = CurrentToken;

        if (CurrentToken.Type == TypeToken.GetActualX)
        {
            Match(TypeToken.GetActualX);
            Match(TypeToken.OpenParenthesis);
            Match(TypeToken.CloseParenthesis);

            return new NodeFunction(list, Screen, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.GetActualY)
        {
            Match(TypeToken.GetActualY);
            Match(TypeToken.OpenParenthesis);
            Match(TypeToken.CloseParenthesis);

            return new NodeFunction(list, Screen, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.GetCanvasSize)
        {
            Match(TypeToken.GetCanvasSize);
            Match(TypeToken.OpenParenthesis);
            Match(TypeToken.CloseParenthesis);

            return new NodeFunction(list, Screen, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.GetColorCount)
        {
            Match(TypeToken.GetColorCount);
            Match(TypeToken.OpenParenthesis);
            Node node1 = Line();
            Match(TypeToken.Comma);
            Node node2 = Expression();
            Match(TypeToken.Comma);
            Node node3 = Expression();
            Match(TypeToken.Comma);
            Node node4 = Expression();
            Match(TypeToken.Comma);
            Node node5 = Expression();
            Match(TypeToken.CloseParenthesis);

            list.Add(node1);
            list.Add(node2);
            list.Add(node3);
            list.Add(node4);
            list.Add(node5);

            return new NodeFunction(list, Screen, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.IsBrushColor)
        {
            Match(TypeToken.IsBrushColor);
            Match(TypeToken.OpenParenthesis);
            Node node = Line();
            Match(TypeToken.CloseParenthesis);

            list.Add(node);

            return new NodeFunction(list, Screen, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.IsBrushSize)
        {
            Match(TypeToken.IsBrushSize);
            Match(TypeToken.OpenParenthesis);
            Node node = Expression();
            Match(TypeToken.CloseParenthesis);

            list.Add(node);

            return new NodeFunction(list, Screen, Table, Type.Integer, token);
        }

        else if (CurrentToken.Type == TypeToken.IsCanvasColor)
        {
            Match(TypeToken.IsCanvasColor);
            Match(TypeToken.OpenParenthesis);
            Node node1 = Line();
            Match(TypeToken.Comma);
            Node node2 = Expression();
            Match(TypeToken.Comma);
            Node node3 = Expression();
            Match(TypeToken.CloseParenthesis);

            list.Add(node1);
            list.Add(node2);
            list.Add(node3);

            return new NodeFunction(list, Screen, Table, Type.Integer, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba funcion");
    }

    Node GoTo()
    {
        Token token = CurrentToken;
        if (CurrentToken.Type == TypeToken.GoTo)
        {
            Match(TypeToken.GoTo);
            Match(TypeToken.OpenBracket);
            Node node1 = Label();
            Match(TypeToken.CloseBracket);
            Token token1 = CurrentToken;
            Node node2 = ConditionGoTo();
            if(node2.NodeType != Type.Boolean)
                throw new SemanticException(token1, "La condicion de goto debe retornar un valor booleano ");

            return new NodeOperator(node1, node2, Table, Type.Null, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba goto ");
    }

    Node ConditionGoTo()
    {
        if (CurrentToken.Type == TypeToken.OpenParenthesis)
        {
            Match(TypeToken.OpenParenthesis);
            Node node = Expression();
            Match(TypeToken.CloseParenthesis);

            return node;
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba parentecis abierto ");
    }

    Node Label()
    {
        Token token = CurrentToken;

        if (CurrentToken.Type == TypeToken.Label)
        {
            Match(TypeToken.Label);
            string name = ((TokenVariableLabel)token).Name;
            int position = Table.SearchPosition(name);

            return new NodeLabel(position, Type.Null, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una etiqueta ");
    }

    Node Line()
    {
        Token token = CurrentToken;

        if (CurrentToken.Type == TypeToken.Line)
        {
            Match(TypeToken.Line);

            return new NodeLine(Table.Simbols.Count - 1, Type.Line, token);
        }

        else
            throw new SintacticException(CurrentToken, "Se esperaba una cadena ");
    }
}