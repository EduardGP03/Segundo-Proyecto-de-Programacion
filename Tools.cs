static class Tools
{
    //es una letra
    public static bool IsLetter(char character) => 'a' <= character && character <= 'z';

    //leer palabra
    public static string ReadWord(string text, ref int position, int large)
    {
        string word = "";

        while (position < large && IsValidLetter(text[position]))
            word += text[position++];

        return word;
    }

    //letra valida
    public static bool IsValidLetter(char character) => IsLetter(char.ToLower(character)) || character == '_' || IsNumber(character);

    //es un numero
    public static bool IsNumber(char character) => '0' <= character && character <= '9';

    //es una palbra clave
    public static bool IsKeyWord(string word) => Table.table.Contains(word);

    //convertir cadena a tipo de token
    public static TypeToken Convert(string word)
    {
        switch (word)
        {
            case "goto":
                return TypeToken.GoTo;

            case "true":
                return TypeToken.True;

            case "false":
                return TypeToken.False;

            case "spawn":
                return TypeToken.Spawn;

            case "color":
                return TypeToken.Color;

            case "size":
                return TypeToken.Size;

            case "drawline":
                return TypeToken.DrawLine;

            case "drawcircle":
                return TypeToken.DrawCircle;

            case "drawrectangle":
                return TypeToken.DrawRectangle;

            case "fill":
                return TypeToken.Fill;

            case "getactualx":
                return TypeToken.GetActualX;

            case "getactualy":
                return TypeToken.GetActualY;

            case "getcanvassize":
                return TypeToken.GetCanvasSize;

            case "getcolorcount":
                return TypeToken.GetColorCount;

            case "isbrushcolor":
                return TypeToken.IsBrushColor;

            case "isbrushsize":
                return TypeToken.IsBrushSize;

            default:
                return TypeToken.IsCanvasColor;
        }
    }

    //es una etiqueta
    public static bool IsLabel(string text, int position, int large, Token previousToken)
    {
        char character = NextCharacter(text, position, large);

        return previousToken.Type == TypeToken.NewLine && character == '\n' || previousToken.Type == TypeToken.OpenBracket && character == ']';
    }

    //buscar proximo caracter
    public static char NextCharacter(string text, int position, int large)
    {
        while (position < large && (text[position] == ' ' || text[position] == '\r' || text[position] == '\t'))
            position++;

        if (position < large)
            return text[position];

        return '\n';
    }

    //leer numero
    public static string ReadNumber(string text, ref int position, int large)
    {
        string number = "";

        while (position < large && IsNumber(text[position]))
            number += text[position++];

        return number;
    }

    //es una comilla
    public static string ReadLine(string text, ref int position, int large)
    {
        string line = "";

        while (position < large && text[position] != '"' && text[position] != '\n')
            line += text[position++];
        if(position < large && text[position] == '"')  {
            line += '"';
            position++;
        }

        return line;
    }

    //ver si el tipo de token esta en la lista
    public static bool Search(TypeToken[] typeToken, Token token)
    {
        for (int i = 0; i < typeToken.Length; i++)
            if (typeToken[i] == token.Type)
                return true;

        return false;
    }
}