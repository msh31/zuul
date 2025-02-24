// ReSharper disable InconsistentNaming 
// Inspired by https://github.com/tomakita/Colorful.Console
// When I used Colorful.Console, it didn't really work well within the terminal, so I wrote my own.

public class ColorfulTextWrapper
{
    private static readonly Random random = new();
    
    private static void SetTypeTextWithColor(string text, string type, ConsoleColor color, bool newLine)
    {
        Console.ForegroundColor = color;
        Console.Write($"[{type}] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        if (newLine)
            Console.WriteLine(text);
        else
            Console.Write(text);
    }

    public static void WriteTextWithColor(string text, ConsoleColor color, bool newLine, bool centered)
    {
        if (centered) Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
        
        Console.ForegroundColor = color;
        if (newLine)
            Console.WriteLine(text);
        else
            Console.Write(text);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void WriteAnimatedTextWithColor(string text, ConsoleColor color, bool newLine)
    {
        var delay = 50;
        foreach (var c in text)
        {
            Console.ForegroundColor = color;
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.Gray;
            Thread.Sleep(delay);
        }

        if (newLine)
            Console.WriteLine(string.Empty);
    }
    
    public static void WriteFormattedTextByType(string text, string type, bool newLine, bool centered)
    {
        if (centered) Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
        
        switch (type)
        {
            case "err":
                SetTypeTextWithColor(text, "err", ConsoleColor.Red, newLine);
                break;
            case "suc":
                SetTypeTextWithColor(text, "suc", ConsoleColor.Green, newLine);
                break;
            case "warn":
                SetTypeTextWithColor(text, "warn", ConsoleColor.Yellow, newLine);
                break;
            case "inf":
                SetTypeTextWithColor(text, "inf", ConsoleColor.Cyan, newLine);
                break;
            case "dbg":
                SetTypeTextWithColor(text, "dbg", ConsoleColor.Magenta, newLine);
                break;
            case "dbg-err":
                SetTypeTextWithColor(text, "dbg-err", ConsoleColor.Magenta, newLine);
                break;
            case "dbg-suc":
                SetTypeTextWithColor(text, "dbg-suc", ConsoleColor.Magenta, newLine);
                break;
            case "dbg-inf":
                SetTypeTextWithColor(text, "dbg-inf", ConsoleColor.Magenta, newLine);
                break;
        }
    }
    
    public static void WriteCenteredText(string text, bool newLine)
    {
        Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
        if (newLine)
            Console.WriteLine(text);
        Console.Write(text);
    }
    
    public static void HighlightWordInText(string text, ConsoleColor color, string word, bool newLine, bool centered)
    {
        var startIndex = text.IndexOf(word, StringComparison.Ordinal);

        if(centered) Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
        
        if (startIndex != -1)
        {
            Console.Write(text[..startIndex]);
            Console.ForegroundColor = color;
            Console.Write(word);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(text[(startIndex + word.Length)..]);
            if(newLine)
                Console.WriteLine(string.Empty);
        }
    }

    public static ConsoleColor GetRandomConsoleColor()
    {
        ConsoleColor[] colors =
        [
            ConsoleColor.Blue,
            ConsoleColor.Cyan,
            ConsoleColor.DarkBlue,
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkYellow,
            ConsoleColor.Green,
            ConsoleColor.Magenta,
            ConsoleColor.Red,
            ConsoleColor.Yellow
        ];

        var randomIndex = random.Next(colors.Length);
        return colors[randomIndex];
    }
}