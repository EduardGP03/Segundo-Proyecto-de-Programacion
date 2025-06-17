using Godot;
using System.Collections.Generic;
using System;
using System.Threading;
using System.IO;

public partial class Node2d : Node2D
{
    Canvas screen = new Canvas();
    [Export] TileMapLayer Screen;
    [Export] Camera2D Camera;
    [Export] CodeEdit Code;
    [Export] Label WindowMessage;
    [Export] TextEdit direction1;
    [Export] TextEdit number;
    [Export] TextEdit direction2;
    [Export] Label Error;


    // Se llama cuando el nodo entra al árbol de escena por primera vez
    public override void _Ready()
    {
        Zoom(screen.Screen.GetLength(0));
        DrawScreen(screen.Screen);
    }

    // Se llama en cada fotograma. 'delta' es el tiempo transcurrido desde el fotograma anterior
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ejecutar"))
        {
            try
            {
                Thread.Sleep(500);
                screen.ClearCanvas();

                string text = Code.Text;
                string resultado = getCanvas(text);
                WindowMessage.Text = resultado;

                DrawScreen(screen.Screen);
            }

            catch (Exception e)
            {
                WindowMessage.Text = e.Message;
            }
        }


        // Función para guardar
        if (Input.IsActionPressed("guardar"))
        {
            try
            {
                // Verificar que hay código para guardar
                if (string.IsNullOrWhiteSpace(Code.Text))
                {
                    WindowMessage.Text = "No hay código para guardar";
                    Thread.Sleep(500);
                    return;
                }

                string filePath = direction1.Text.Trim();

                // Verificar si la ruta está vacía
                if (string.IsNullOrEmpty(filePath))
                {
                    WindowMessage.Text = "Debe especificar un nombre de archivo";
                    Thread.Sleep(500);
                    return;
                }

                // Manejo de la extensión del archivo
                if (!Path.HasExtension(filePath))
                {
                    filePath += ".pw"; // Agregar extensión por defecto
                }
                else
                {
                    // Verificar que la extensión sea válida
                    string extension = Path.GetExtension(filePath).ToLower();
                    if (extension != ".pw" && extension != ".txt")
                    {
                        WindowMessage.Text = "Extensión no válida. Use .pw o .txt";
                        Thread.Sleep(500);
                        return;
                    }
                }

                // Crear directorio si no existe
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Guardar el contenido
                File.WriteAllText(filePath, Code.Text);

                // Mostrar confirmación con ruta completa
                WindowMessage.Text = $"Código guardado en:\n{Path.GetFullPath(filePath)}";
            }
            catch (Exception ex)
            {
                WindowMessage.Text = $"Error al guardar: {ex.Message}";
            }
            Thread.Sleep(500);
        }

        //Funcion para redimensionar
        if (Input.IsActionPressed("redimensionar"))
        {
            try
            {
                string sizeText = number.Text;
                if (!int.TryParse(sizeText, out int newSize) || newSize <= 0)
                {
                    WindowMessage.Text = "Tamaño inválido. Debe ser un número positivo";
                    return;
                }

                screen = new Canvas(newSize);
                Zoom(newSize);
                DrawScreen(screen.Screen);
                WindowMessage.Text = $"Canvas redimensionado a {newSize}x{newSize}";
            }
            catch (Exception ex)
            {
                WindowMessage.Text = $"Error al redimensionar: {ex.Message}";
            }
            Thread.Sleep(500);
        }

        //Funcion para cargar
        if (Input.IsActionPressed("cargar"))
        {
            try
            {
                string filePath = direction2.Text.Trim();

                if (string.IsNullOrEmpty(filePath))
                {
                    WindowMessage.Text = "Debe especificar un archivo a cargar";
                    Thread.Sleep(500);
                    return;
                }

                // Verificar existencia del archivo
                if (!File.Exists(filePath))
                {
                    WindowMessage.Text = "El archivo no existe";
                    Thread.Sleep(500);
                    return;
                }

                // Verificar extensión al cargar (opcional)
                string extension = Path.GetExtension(filePath).ToLower();
                if (extension != ".pw" && extension != ".txt")
                {
                    WindowMessage.Text = "Solo se pueden cargar archivos .pw o .txt";
                    Thread.Sleep(500);
                    return;
                }

                // Cargar el contenido
                Code.Text = File.ReadAllText(filePath);
                WindowMessage.Text = $"Código cargado desde:\n{Path.GetFullPath(filePath)}";
            }
            catch (Exception ex)
            {
                WindowMessage.Text = $"Error al cargar: {ex.Message}";
            }
            Thread.Sleep(500);
        }
    }

    void DrawScreen(Colors[,] colors)
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                switch (colors[i, j])
                {
                    case Colors.Red:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(0, 0));
                        break;

                    case Colors.Blue:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(1, 0));
                        break;

                    case Colors.Green:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(2, 0));
                        break;

                    case Colors.Yellow:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(3, 0));
                        break;

                    case Colors.Orange:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(4, 0));
                        break;

                    case Colors.Purple:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(5, 0));
                        break;

                    case Colors.Black:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(6, 0));
                        break;

                    case Colors.White:
                        Screen.SetCell(new Vector2I(i, j), 0, new Vector2I(7, 0));
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void Zoom(int size)
    {
        Vector2 temp = GetViewportRect().Size;
        float zoom = temp.Y / size;
        Camera.Zoom = new Vector2(zoom, zoom);
    }

    void _on_button_pressed()
    {
        string text = Code.Text;
    }


    string getCanvas(string text)
    {
        string ret = "";
        SimbolTable Table = new SimbolTable();
        Lexer nuevoScanner = new Lexer(text, Table);

        try
        {
            Parser parser = new Parser(nuevoScanner, Table, screen);
            parser.Parse();
            List<Node> Tree = parser.GetTree();
            int iterator = 0;
            while(iterator < Tree.Count)    
                Tree[iterator].Evaluate(ref iterator);
        }
        catch (LexicalException e)
        {
            ret = "Error Lexicografico <" + e.TokenException.Row + "," + e.TokenException.Col + ">: " + e.Message;
        }
        catch (SintacticException e)
        {
            ret = "Error Sintactico <" + e.TokenException.Row + "," + e.TokenException.Col + ">: " + e.Message;
        }
        catch (SemanticException e)
        {
            ret = "Error Semantico <" + e.TokenException.Row + "," + e.TokenException.Col + ">: " + e.Message;
        }

        catch (Exception e)
        {
            ret = "Error Semantico: " + e.Message;
        }

        if (ret == "")
            ret = "Compilacion y ejecucion exitosa.";
        return ret;
    }

}

