using System;
using System.Collections.Generic;
using Godot;

public class Canvas
{
    public Colors[,] Screen;
    private int currentX;
    private int currentY;
    private Colors currentColor = Colors.Transparent;
    private int brushSize = 1; // Tamaño por defecto 
    private int canvasSize;

    public Canvas(int size = 256) // Tamaño por defecto 100x100
    {
        if (size <= 0)
            throw new ArgumentException("El tamaño del canvas debe ser positivo");

        canvasSize = size;
        Screen = new Colors[canvasSize, canvasSize];
        ClearCanvas();
    }

    public void ClearCanvas()
    {
        for (int y = 0; y < canvasSize; y++)
        {
            for (int x = 0; x < canvasSize; x++)
            {
                Screen[x, y] = Colors.White;
            }
        }

        brushSize = 1;
    }

    public void Spawn(int x, int y)
    {
        if (x < 0 || x >= canvasSize || y < 0 || y >= canvasSize)
            throw new ArgumentOutOfRangeException("Posición inicial fuera de los límites del canvas ");

        currentX = x;
        currentY = y;
    }

    public void Color(string color)
    {
        color = color.ToLower();

        if (color == "red")
            currentColor = Colors.Red;

        else if (color == "blue")
            currentColor = Colors.Blue;

        else if (color == "green")
            currentColor = Colors.Green;

        else if (color == "yellow")
            currentColor = Colors.Yellow;

        else if (color == "orange")
            currentColor = Colors.Orange;

        else if (color == "purple")
            currentColor = Colors.Purple;

        else if (color == "black")
            currentColor = Colors.Black;

        else if (color == "white")
            currentColor = Colors.White;

        else
            throw new ArgumentException("Este color no esta registrado debe usar otro ");
    }

    public void Size(int k)
    {
        if (k <= 0)
            throw new ArgumentException("El tamaño del pincel debe ser positivo ");

        // Asegurar que el tamaño sea impar
        brushSize = k % 2 == 0 ? k - 1 : k;
    }

    private void DrawPoint(int sizePoint, int x, int y)
    {
        for (int i = x; i < sizePoint + x; i++)
        {
            for (int j = y; j < sizePoint + y; j++)
            {
                if (i < 0 || i >= canvasSize || j < 0 || j >= canvasSize)
                    throw new Exception("No se puede seguir dibujando ");

                Screen[i, j] = currentColor;
            }
        }
    }

    public void DrawLine(int dirX, int dirY, int distance)
    {
        if (dirX == 0 && dirY == 0)
            return;

        else if ((dirX == -1 || dirX == 0 || dirX == 1) && (dirY == -1 || dirY == 0 || dirY == 1))
        {
            for (int i = 0; i < distance; i++)
            {
                DrawPoint(brushSize, currentX, currentY);

                currentX += dirX;
                currentY += dirY;
            }
        }

        else
            throw new ArgumentException("Los valores de las direcciones no pueden ser distintos de -1, 0, 1 ");
    }

    public void DrawCircle(int dirX, int dirY, int radius)
    {
        if (currentColor == Colors.Transparent)
            return;

        if (radius <= 0)
            throw new ArgumentException("El radio debe ser positivo");

        if ((Math.Abs(dirX) > 1) || (Math.Abs(dirY) > 1))
            throw new ArgumentException("Las direcciones deben estar entre -1 y 1");

        // Calcular la posición central
        int centerX = currentX + dirX * radius;
        int centerY = currentY + dirY * radius;

        // Verificar límites del canvas
        if (centerX < 0 || centerX >= canvasSize || centerY < 0 || centerY >= canvasSize)
            throw new Exception("El centro del círculo está fuera del canvas");

        // Algoritmo mejorado para dibujar círculos
        int x = 0;
        int y = radius;
        int d = 3 - 2 * radius;

        // Dibujar los puntos iniciales
        DrawCirclePoints(centerX, centerY, x, y);

        while (y >= x)
        {
            x++;

            if (d > 0)
            {
                y--;
                d = d + 4 * (x - y) + 10;
            }
            else
            {
                d = d + 4 * x + 6;
            }

            DrawCirclePoints(centerX, centerY, x, y);
        }

        // Actualizar posición al centro del círculo
        currentX = centerX;
        currentY = centerY;
    }

    private void DrawCirclePoints(int cx, int cy, int x, int y)
    {
        try
        {
            // Los 8 puntos simétricos
            DrawBrushPixel(cx + x, cy + y);
            DrawBrushPixel(cx - x, cy + y);
            DrawBrushPixel(cx + x, cy - y);
            DrawBrushPixel(cx - x, cy - y);
            DrawBrushPixel(cx + y, cy + x);
            DrawBrushPixel(cx - y, cy + x);
            DrawBrushPixel(cx + y, cy - x);
            DrawBrushPixel(cx - y, cy - x);
        }

        catch (Exception ex)
        {
            throw new Exception("Error al dibujar los puntos del círculo: " + ex.Message);
        }
    }

    private void DrawBrushPixel(int x, int y)
    {
        // Validar coordenadas primero
        if (x < 0 || x >= canvasSize || y < 0 || y >= canvasSize)
            return;  // Silenciosamente ignoramos píxeles fuera del canvas

        if (brushSize == 1)
        {
            Screen[x, y] = currentColor;
            return;
        }

        // Manejo del tamaño del pincel
        int halfSize = brushSize / 2;

        try
        {
            for (int i = x - halfSize; i <= x + halfSize; i++)
            {
                for (int j = y - halfSize; j <= y + halfSize; j++)
                {
                    if (i >= 0 && i < canvasSize && j >= 0 && j < canvasSize)
                    {
                        double distance = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                        if (distance <= halfSize)
                        {
                            Screen[i, j] = currentColor;
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {
            throw new Exception("Error al dibujar con el pincel: " + ex.Message);
        }
    }
    public void DrawRectangle(int dirX, int dirY, int distance, int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("El ancho y alto del rectángulo deben ser positivos");

        if (dirX == 0 && dirY == 0)
            return;

        else if ((dirX == -1 || dirX == 0 || dirX == 1) && (dirY == -1 || dirY == 0 || dirY == 1))
        {
            int widthDistanceRight = width / 2;
            int widthDistanceLeft = width - widthDistanceRight;
            int heightDistanceDown = height / 2;
            int heightDistanceUP = height - heightDistanceDown;

            for (int i = 0; i < distance; i++)
            {
                if (currentX < 0 || currentX >= canvasSize || currentY < 0 || currentY >= canvasSize)
                    throw new Exception("El centro del rectangulo esta fuera de los limites ");

                currentX += dirX;
                currentY += dirY;
            }

            int midX = currentX, midY = currentY;

            currentX -= widthDistanceLeft; currentY -= heightDistanceUP;
            DrawLine(0, 1, height - 1);
            DrawLine(1, 0, width - 1);
            DrawLine(0, -1, height - 1);
            DrawLine(-1, 0, width - 1);

            currentX = midX; currentY = midY;
        }
    }

    public void Fill()
    {
        if (currentX < 0 || currentX >= canvasSize || currentY < 0 || currentY >= canvasSize)
            throw new Exception("La posición actual está fuera de los límites del canvas");

        if (currentColor == Colors.Transparent)
            return;

        Colors targetColor = Screen[currentX, currentY];
        if (targetColor == currentColor)
            return;

        // Algoritmo de relleno por inundación (Flood Fill)
        Queue<(int, int)> pixels = new Queue<(int, int)>();
        pixels.Enqueue((currentX, currentY));

        while (pixels.Count > 0)
        {
            var (x, y) = pixels.Dequeue();

            if (x < 0 || x >= canvasSize || y < 0 || y >= canvasSize)
                continue;

            if (Screen[x, y] != targetColor)
                continue;

            Screen[x, y] = currentColor;

            pixels.Enqueue((x + 1, y));
            pixels.Enqueue((x - 1, y));
            pixels.Enqueue((x, y + 1));
            pixels.Enqueue((x, y - 1));
        }
    }

    // Funciones del lenguaje
    public int GetActualX() => currentX;
    public int GetActualY() => currentY;
    public int GetCanvasSize() => canvasSize;

    public int GetColorCount(string color, int x1, int y1, int x2, int y2)
    {
        if (x1 < 0 || x1 >= canvasSize || y1 < 0 || y1 >= canvasSize || x2 < 0 || x2 >= canvasSize || y2 < 0 || y2 >= canvasSize)
            return 0;

        // Normalizar el formato del color (primera letra mayúscula, resto minúsculas)
        string normalizedColor = color.ToLower();
        normalizedColor = char.ToUpper(normalizedColor[0]) + normalizedColor.Substring(1);


        int col = y1 < y2 ? y1 : y2;
        int fil = col == y1 ? x1 : x2;
        int count = 0;

        for (int i = fil; i <= fil + Math.Abs(x2 - x1);)
        {
            if (x1 < x2)
                i++;
            else
                i--;

            for (int j = col; j <= col + Math.Abs(y2 - y1); j++)
            {
                if (Screen[i, j].ToString() == normalizedColor)
                    count++;
            }
        }

        return count;
    }

    public int IsBrushColor(string color)
    {
        if (Enum.TryParse(color, true, out Colors parsedColor))
        {
            return currentColor == parsedColor ? 1 : 0;
        }

        return 0;
    }

    public int IsBrushSize(int size) => brushSize == size ? 1 : 0;

    public int IsCanvasColor(string color, int v, int h)
    {
        if (!Enum.TryParse(color, true, out Colors targetColor))
            return 0;

        int x = currentX + h;
        int y = currentY + v;

        if (x < 0 || x >= canvasSize || y < 0 || y >= canvasSize)
            return 0;

        return Screen[x, y] == targetColor ? 1 : 0;
    }
}
