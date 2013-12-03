using System;
using System.Collections.Generic;

namespace Rubik
{
    public class CardSet
    {
        private char[] Chars { get; set; }
        private char[][] Cards { get; set; }

        public int Rotation { get; private set; }
        public char[] Card { get { return Cards[Rotation]; } }
        public bool Used { get; set; }

        public CardSet(params char[] chars)
        {
            if (chars.Length != 4)
                throw new ArgumentOutOfRangeException("chars");

            Chars = chars;

            Cards = new char[4][];
            Cards[0] = new[] { chars[0], chars[1], chars[2], chars[1], chars[3], chars[0], chars[2], chars[3] };
            Cards[1] = new[] { chars[2], chars[3], chars[0], chars[1], chars[2], chars[1], chars[3], chars[0] };
            Cards[2] = new[] { chars[3], chars[0], chars[2], chars[3], chars[0], chars[1], chars[2], chars[1] };
            Cards[3] = new[] { chars[2], chars[1], chars[3], chars[0], chars[2], chars[3], chars[0], chars[1] };
        }

        public void Rotate()
        {
            Rotation = (Rotation + 1)%4;
        }

        public void Print()
        {
            for (int i = 0; i < 4; i++, Rotate())
            {
                for (int line = 0; line < 4; line++)
                {
                    Print(line);
                    Console.WriteLine();
                }
                Console.WriteLine("-----");
            }
        }

        public void Print(int line)
        {
            switch(line)
            {
                case 0:
                    Ch.Colored(string.Format(" {0}{1} |", Card[0], Card[1]));
                    break;
                case 1:
                    Ch.Colored(string.Format("{0}{1} {2}|", Card[7], Rotation, Card[2]));
                    break;
                case 2:
                    Ch.Colored(string.Format("{0} {1}{2}|", Card[6], Used ? '1' : ' ', Card[3]));
                    break;
                case 3:
                    Ch.Colored(string.Format(" {0}{1} |", Card[5], Card[4]));
                    break;
                default:
                    Console.Write("    ");
                    break;
            }
        }

        public void PrintShort()
        {
            Console.WriteLine(Chars);
        }

        public static CardSet[] CreateCards()
        {
            var result = new List<CardSet>();
            foreach (char c1 in new[] { 'a', 'b', 'c', 'd' })
            {
                foreach (char c2 in new[] { 'a', 'b', 'c', 'd' })
                {
                    if (c1 == c2) continue;
                    foreach (char c3 in new[] { 'a', 'b', 'c', 'd' })
                    {
                        if (c1 == c3 || c2 == c3) continue;
                        foreach (char c4 in new[] { 'a', 'b', 'c', 'd' })
                        {
                            if (c1 == c4 || c2 == c4 || c3 == c4) continue;
                            result.Add(new CardSet(c1, c2, c3, c4));
                            if (result.Count == 1) // ein teil ist doppelt
                                result.Add(new CardSet(c1, c2, c3, c4));
                        }
                    }
                }
            }
            return result.ToArray();
        }

    }
}