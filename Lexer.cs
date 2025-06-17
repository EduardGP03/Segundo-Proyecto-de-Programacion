using System.IO;

public class Lexer
{
    int Position = 0;
    string Text;
    int Row = 1;
    int Col = 1;
    int startCol = 0;
    public Token PreviousToken { get; private set; }
    int large;
    SimbolTable Table = new SimbolTable();

    public Lexer(string text, SimbolTable table)
    {
        Text = text;
        Text = Text.ToLower();
        large = Text.Length;
        Table = table;
        PreviousToken = new Token(1, 1, TypeToken.End);
    }

    public Token TakeToken()
    {
        PreviousToken = SearchToken();
        return PreviousToken;
    }

    public Token SearchToken()
    {
        while (Position < large)
        {
            Col = Position - startCol + 1;

            if (Text[Position] == ' ' || Text[Position] == '\t' || Text[Position] == '\r')
            {
                Position++;
                continue;
            }

            if (Tools.IsLetter(Text[Position]))
            {
                string word = Tools.ReadWord(Text, ref Position, large);

                int pos = Table.SearchPosition(word);
                if (pos != -1)
                {
                    if (((SimbolVariableLabel)Table.Simbols[pos]).SimbolToken.Type == TypeToken.Label)
                        return new TokenVariableLabel(word, Row, Col, TypeToken.Label);
                    else
                        return new TokenVariableLabel(word, Row, Col, TypeToken.Variable);
                }

                if (Tools.IsKeyWord(word))
                    return new Token(Row, Col, Tools.Convert(word));

                if (Tools.IsLabel(Text, Position, large, PreviousToken))
                {
                    TokenVariableLabel varLabel0 = new TokenVariableLabel(word, Row, Col, TypeToken.Label);
                    Table.Simbols.Add(new SimbolVariableLabel(-1, word, varLabel0, Type.Null));
                    return varLabel0;
                }

                TokenVariableLabel varLabel = new TokenVariableLabel(word, Row, Col, TypeToken.Variable);
                Table.Simbols.Add(new SimbolVariableLabel(-1, word, varLabel, Type.Null));
                return new TokenVariableLabel(word, Row, Col, TypeToken.Variable);
            }

            if (Tools.IsNumber(Text[Position]))
            {
                string number = Tools.ReadNumber(Text, ref Position, large);

                return new TokenNumber(int.Parse(number), Row, Col, TypeToken.Number);
            }

            if (Text[Position] == '"')
            {
                Position++;
                string line = Tools.ReadLine(Text, ref Position, large);

                if (line[line.Length - 1] == '"')
                {
                    string lineFinal = "";
                    for (int i = 0; i < line.Length - 1; i++)
                        lineFinal += line[i];
                    Token tokenLine = new Token(Row, Col, TypeToken.Line);
                    Table.Simbols.Add(new SimbolLine(lineFinal, '0' + lineFinal, tokenLine));
                    return new Token(Row, Col, TypeToken.Line);
                }

                return new TokenError(Row, Col, "No termina la cadena en \" ", TypeToken.Error);
            }

            if (Text[Position] == '+')
            {
                Position++;
                return new Token(Row, Col, TypeToken.Plus);
            }

            if (Text[Position] == '-')
            {
                if (PreviousToken.Type != TypeToken.Number && PreviousToken.Type != TypeToken.Variable && PreviousToken.Type != TypeToken.CloseParenthesis)
                    return new TokenNumber(0, Row, Col, TypeToken.Number);

                Position++;
                return new Token(Row, Col, TypeToken.Minus);
            }

            if (Text[Position] == '*')
            {
                Position++;

                if (Position < large && Text[Position] == '*')
                {
                    Position++;
                    return new Token(Row, Col, TypeToken.Power);
                }

                return new Token(Row, Col, TypeToken.Product);
            }

            if (Text[Position] == '/')
            {
                Position++;
                return new Token(Row, Col, TypeToken.Division);
            }

            if (Text[Position] == '%')
            {
                Position++;
                return new Token(Row, Col, TypeToken.Modulo);
            }

            if (Text[Position] == '&')
            {
                Position++;

                if (Position < large && Text[Position] == '&')
                {
                    Position++;
                    return new Token(Row, Col, TypeToken.And);
                }

                return new TokenError(Row, Col, "No puede ser un solo & tienen que ser 2 ", TypeToken.Error);
            }

            if (Text[Position] == '|')
            {
                Position++;

                if (Position < large && Text[Position] == '|')
                {
                    Position++;
                    return new Token(Row, Col, TypeToken.Or);
                }

                return new TokenError(Row, Col, "No puede ser un solo | tienen que ser 2 ", TypeToken.Error);
            }

            if (Text[Position] == '(')
            {
                Position++;
                return new Token(Row, Col, TypeToken.OpenParenthesis);
            }

            if (Text[Position] == ')')
            {
                Position++;
                return new Token(Row, Col, TypeToken.CloseParenthesis);
            }

            if (Text[Position] == '[')
            {
                Position++;
                return new Token(Row, Col, TypeToken.OpenBracket);
            }

            if (Text[Position] == ']')
            {
                Position++;
                return new Token(Row, Col, TypeToken.CloseBracket);
            }

            if (Text[Position] == ',')
            {
                Position++;
                return new Token(Row, Col, TypeToken.Comma);
            }

            if (Text[Position] == '<')
            {
                Position++;

                if (Position < large && Text[Position] == '=')
                {
                    Position++;
                    return new Token(Row, Col, TypeToken.LessEquals);
                }

                if (Position < large && Text[Position] == '-')
                {
                    Position++;
                    return new Token(Row, Col, TypeToken.Arrow);
                }

                return new Token(Row, Col, TypeToken.Less);
            }

            if (Text[Position] == '=')
            {
                Position++;

                if (Position < large && Text[Position] == '=')
                {
                    Position++;
                    return new Token(Row, Col, TypeToken.EqualsEquals);
                }

                return new TokenError(Row, Col, "No puede ser un solo = tienen que ser 2 ", TypeToken.Error);
            }

            if (Text[Position] == '>')
            {
                Position++;

                if (Position < large && Text[Position] == '=')
                {
                    Position++;
                    return new Token(Row, Col, TypeToken.GreaterEquals);
                }

                return new Token(Row, Col, TypeToken.Greater);
            }

            if (Text[Position] == '\n')
            {
                Row++;
                Position++;
                startCol = Position;
                return new Token(Row - 1, Col, TypeToken.NewLine);
            }

            if (Text[Position] == '!')
            {
                if (Position + 1 < Text.Length && Text[Position + 1] == '=')
                {
                    Position += 2;
                    return new Token(Row, Col, TypeToken.Distint);
                }

                return new TokenError(Row, Col, "Despues de ! debe venir un = ");
            }

            Position++;
            return new TokenError(Row, Col, $"Caracter {Text[Position - 1]} invalido", TypeToken.Error);
        }

        //Position++;
        return new Token(Row, Col, TypeToken.End);
    }


}