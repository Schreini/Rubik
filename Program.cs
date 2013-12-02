using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rubik
{
    class Program
    {
        static char[][] spielfeld = new char[24][];
        

        static void Main(string[] args)
        {
            List<char[]> cards = CreateCards();
            foreach (var card in cards)
            {
                Console.WriteLine(card);
            }
            spielfeld[0] = cards[0];
            cards.RemoveAt(0);
            LegeKarte(1, cards);
            Console.ReadLine();
        }

        private static void LegeKarte(int spielfeldPos, List<char[]> cards)
        {
            if(cards.Count == 0)
            {
                //fertig?
                Console.WriteLine("Fertig");
                Console.ReadLine();
            }
            for(int i = 0; i < cards.Count; i++)
            {
                for (int rotations = 0; rotations < 4; rotations++)
                {
                    if (CompareRechts(spielfeld[spielfeldPos - 1], cards[i]))
                    {
                        spielfeld[spielfeldPos] = cards[i];
                        var newCards = new List<char[]>(cards);
                        newCards.RemoveAt(i);
                        LegeKarte(spielfeldPos+1, newCards);
                    }
                    else
                    {
                        Rotate(cards, i);
                    }
                }
            }
        }

        private static void Rotate(List<char[]> cards, int pos)
        {
            var rotatedCard = new char[8];
            Array.Copy(cards[pos], 2, rotatedCard, 0, 6);
            Array.Copy(cards[pos], 0, rotatedCard, 6, 2);
            cards[pos] = rotatedCard;
        }

        private static bool CompareRechts(char[] links, char[] rechts)
        {
            return links[2] == rechts[7] && links[3] == rechts[6];
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
                            result.Add(new []{c1,c2,c2,c3,c4,c1,c4,c3});
                        }
                    }
                }
            }
            return result;
        }
    }
}
