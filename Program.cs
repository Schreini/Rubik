using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rubik
{
    class Program
    {
        private static long AnzahlZuege = -1;
        private static char[][] Spielfeld;
        private static char[][] Cards;
        private static bool[] Used;
        private static int[] Rotation;
        private static DateTime LastTime = DateTime.Now;
        private static int AnzahlCards = 0;

        static void Main(string[] args)
        {
            Cards = CreateCards().ToArray();
            AnzahlCards = Cards.Count();
            Spielfeld =  new char[AnzahlCards][];
            Used = new bool[AnzahlCards];
            Rotation = new int[AnzahlCards];

            foreach (var card in Cards)
            {
                Console.WriteLine(card);
            }
            PrintCards(Cards);

            LegeKarte(0);
            Console.WriteLine("keine lösung gefunden.");
            Console.ReadLine();
        }

        private static void PrintCards(char[][] cards)
        {
            Console.Write("Used: ");
            new List<bool>(Used).ForEach(b => Console.Write(b ? "1" : " "));
            Console.WriteLine();
            Console.Write("Rota: ");
            new List<int>(Rotation).ForEach(r => Console.Write(r));
            Console.WriteLine();

            for (int row = 0; row < 25; row += 5)
            {
                for (int col = 0; col < 5; col++)
                {
                    Colored(String.Format(" {0}{1} |", cards[row + col][0], cards[row + col][1]));
                }
                Console.WriteLine();
                for (int col = 0; col < 5; col++)
                {
                    Colored(String.Format("{0}{1} {2}|", cards[row + col][7], Rotation[row + col], cards[row + col][2]));
                }
                Console.WriteLine();
                for (int col = 0; col < 5; col++)
                {
                    Colored(String.Format("{0}  {1}|", cards[row + col][6], cards[row + col][3]));
                }
                Console.WriteLine();
                for (int col = 0; col < 5; col++)
                {
                    Colored(String.Format(" {0}{1} |", cards[row + col][5], cards[row + col][4]));
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------");
            }
        }

        private static void Colored(string text)
        {
            foreach (char t in text)
            {
                switch(t)
                {
                    case 'a':
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case 'b':
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case 'c':
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case 'd':
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                Console.Write(t);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void LegeKarte(int spielfeldPos)
        {
            AnzahlZuege += 1;
            if(AnzahlZuege%100000==0)
            {
                TimeSpan timeSpan = DateTime.Now - LastTime;
                Console.WriteLine(string.Format("{0:mmss}\tZüge: {1}\tPosition: {2}", timeSpan, AnzahlZuege, spielfeldPos));
                LastTime = DateTime.Now;
            }
            if(spielfeldPos == AnzahlCards)
            {
                //fertig?
                Console.WriteLine("Fertig; Züge: " + AnzahlZuege);
                PrintCards(Spielfeld);
                Console.ReadLine();
            }
            for(int i = 0; i < AnzahlCards; i++)
            {
                if(Used[i]) continue;
                for (int rotations = 0; rotations < 4; rotations++)
                {
                    if (Compare(spielfeldPos, Cards[i]))
                    {
                        Spielfeld[spielfeldPos] = Cards[i];
                        Used[i] = true;
                        //var newCards = new List<char[]>(cards);
                        //newCards.RemoveAt(i);
                        LegeKarte(spielfeldPos + 1);
                        Spielfeld[spielfeldPos + 1] = null;
                        Used[i] = false;
                    }
                    Rotate(i);
                }
            }
        }

        private static void Rotate(int pos)
        {
            AnzahlZuege++;
            var rotatedCard = new char[8];
            Array.Copy(Cards[pos], 2, rotatedCard, 0, 6);
            Array.Copy(Cards[pos], 0, rotatedCard, 6, 2);
            Cards[pos] = rotatedCard;
            Rotation[pos] = (Rotation[pos]+1)%4;
        }

        private static bool Compare(int pos, char[] card)
        {
            int posLinks = pos - 1;
            bool vglLinks = true;
            bool vglOben  = true;
            if (pos % 5 != 0 && posLinks >= 0)
            {
                var links = Spielfeld[posLinks];
                vglLinks = links[2] == card[7] && links[3] == card[6];
            }
            int posOben = pos - 5;
            if(posOben >= 0)
            {
                var oben = Spielfeld[posOben];
                vglOben = oben[5] == card[0] && oben[4] == card[1];
            }
            return vglLinks && vglOben;
        }

        private static List<char[]> CreateCards()
        {
            var result = new List<char[]>();
            foreach(char c1 in new []{'a', 'b', 'c', 'd'})
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
                            result.Add(new []{c1,c2,c3,c2,c4,c1,c3,c4});
                            if(result.Count == 1 ) // ein teil ist doppelt
                                result.Add(new[] { c1, c2, c3, c2, c4, c1, c3, c4 });
                        }
                    }
                }
            }
            return result;
        }
    }
}
