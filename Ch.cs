using System;

namespace Rubik
{
    public class Ch
    {
        public static void Colored(string text)
        {
            foreach (char t in text)
            {
                switch (t)
                {
                    case 'a':
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case 'b':
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case 'c':
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case 'd':
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                Console.Write(t);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}