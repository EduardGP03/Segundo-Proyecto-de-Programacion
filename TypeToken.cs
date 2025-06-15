public enum TypeToken
{
    Variable,  //variable
    Label,  //etiqueta
    Number,  //numero
    Line,  //linea cadena
    GoTo,  //goto

    Plus, //mas  "+"
    Minus,  //menos  "-"
    Product, //producto  "*"
    Division,  //division  "/"
    Power,  //potencia  "**"
    Modulo,  //modulo  "%"

    True,  //verdadero  "true"
    False,  //falso  "false"

    And,  //y  "&&"
    Or,  //o  "||"

    OpenParenthesis,  //parentesis abierto  "("
    CloseParenthesis,  //parentesis cerrado  ")"
    OpenBracket,  //colchete abierto  "["
    CloseBracket,  //colchete cerrado  "]"
    Comma,  //coma  ","

    Less,  //menor  "<"
    LessEquals,  //menor igual  "<="
    EqualsEquals,  //igual igual  "=="
    GreaterEquals,  //mayor igual  ">="
    Greater,  //mayor  ">"
    Distint, //distinto  "!="

    Spawn,  //generar
    Color,  //color 
    Size,  //tamaño
    DrawLine,  //dibujar linea
    DrawCircle,  //dibujar circulo
    DrawRectangle,  //dibujar rectangulo
    Fill,  //rellenar
    GetActualX,  //obtener actual x
    GetActualY,  //obtener actual y
    GetCanvasSize,  //obtener tamaño canvas
    GetColorCount,  //obtener color canvas
    IsBrushColor,  //color de pincel
    IsBrushSize,  //tamaño de pincel
    IsCanvasColor,  //color de canvas

    Error,  //error
    
    Arrow,  //flecha  "<-"

    NewLine,  //nueva linea  "\n"

    End  //fin
}