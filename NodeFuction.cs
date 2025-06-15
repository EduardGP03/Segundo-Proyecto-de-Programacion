using System;
using System.Collections.Generic;

public class NodeFunction : Node
{
    public List<Node> Params;
    public Canvas Screen;
    public SimbolTable Table;

    public NodeFunction(List<Node> paramss, Canvas screen, SimbolTable table, Type nodeType, Token nodeToken) : base(nodeType, nodeToken)
    {
        Params = paramss;
        Screen = screen;
        Table = table;
    }

    public override int Evaluate(ref int iterator)
    {
        if (NodeToken.Type == TypeToken.Spawn)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            int paramss_1 = Params[1].Evaluate(ref iterator);

            Screen.Spawn(paramss_0, paramss_1);

            iterator++;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.Color)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            string line = ((SimbolLine)Table.Simbols[paramss_0]).Value;

            Screen.Color(line);

            iterator++;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.Size)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);

            Screen.Size(paramss_0);

            iterator++;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.DrawLine)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            int paramss_1 = Params[1].Evaluate(ref iterator);
            int paramss_2 = Params[2].Evaluate(ref iterator);

            Screen.DrawLine(paramss_0, paramss_1, paramss_2);

            iterator++;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.DrawCircle)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            int paramss_1 = Params[1].Evaluate(ref iterator);
            int paramss_2 = Params[2].Evaluate(ref iterator);

            Screen.DrawCircle(paramss_0, paramss_1, paramss_2);

            iterator++;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.DrawRectangle)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            int paramss_1 = Params[1].Evaluate(ref iterator);
            int paramss_2 = Params[2].Evaluate(ref iterator);
            int paramss_3 = Params[3].Evaluate(ref iterator);
            int paramss_4 = Params[4].Evaluate(ref iterator);


            Screen.DrawRectangle(paramss_0, paramss_1, paramss_2, paramss_3, paramss_4);

            iterator++;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.Fill)
        {
            Screen.Fill();

            iterator++;

            return 0;
        }

        else if (NodeToken.Type == TypeToken.GetActualX)
        {
            return Screen.GetActualX();
        }

        else if (NodeToken.Type == TypeToken.GetActualY)
        {
            return Screen.GetActualY();
        }

        else if (NodeToken.Type == TypeToken.GetCanvasSize)
        {
            return Screen.GetCanvasSize();
        }

        else if (NodeToken.Type == TypeToken.GetColorCount)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            string line = ((SimbolLine)Table.Simbols[paramss_0]).Value;

            int paramss_1 = Params[1].Evaluate(ref iterator);
            int paramss_2 = Params[2].Evaluate(ref iterator);
            int paramss_3 = Params[3].Evaluate(ref iterator);
            int paramss_4 = Params[4].Evaluate(ref iterator);

            return Screen.GetColorCount(line, paramss_1, paramss_2, paramss_3, paramss_4);
        }

        else if (NodeToken.Type == TypeToken.IsBrushColor)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            string line = ((SimbolLine)Table.Simbols[paramss_0]).Value;

            return Screen.IsBrushColor(line);
        }

        else if (NodeToken.Type == TypeToken.IsBrushSize)
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);

            return Screen.IsBrushSize(paramss_0);
        }

        else
        {
            int paramss_0 = Params[0].Evaluate(ref iterator);
            string line = ((SimbolLine)Table.Simbols[paramss_0]).Value;
            int paramss_1 = Params[1].Evaluate(ref iterator);
            int paramss_2 = Params[2].Evaluate(ref iterator);


            return Screen.IsCanvasColor(line, paramss_1, paramss_2);
        }
    }

    public override void Show()
    {
        Console.WriteLine(NodeToken.Type);
        foreach(var x in Params)
        {
            x.Show();
        }
    }

}