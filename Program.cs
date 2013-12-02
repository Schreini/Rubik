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
        private static DateTime LastTime = DateTime.Now;

        static void Main(string[] args)
        {
            List<char[]> cards = CreateCards();
            Spielfeld =  new char[cards.Count][];

            foreach (var card in cards)
            {
                Console.WriteLine(card);
            }

            LegeKarte(0, cards);
            Console.WriteLine("keine lösung gefunden.");
            Console.ReadLine();
        }

        private static void LegeKarte(int spielfeldPos, List<char[]> cards)
        {
            AnzahlZuege += 1;
            if(AnzahlZuege%1000000==0)
            {
                TimeSpan timeSpan = DateTime.Now - LastTime;
                Console.WriteLine(string.Format("{0:mmss}\tZüge: {1}\tRestkarten: {2}", timeSpan, AnzahlZuege, cards.Count));
                LastTime = DateTime.Now;
            }
            if(cards.Count == 0)
            {
                //fertig?
                Console.WriteLine("Fertig; Züge: " + AnzahlZuege);
                Console.ReadLine();
            }
            for(int i = 0; i < cards.Count; i++)
            {
                for (int rotations = 0; rotations < 4; rotations++)
                {
                    if (Compare(spielfeldPos, cards[i]))
                    {
                        Spielfeld[spielfeldPos] = cards[i];
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

        private static bool Compare(int pos, char[] card)
        {
            int posLinks = pos - 1;
            bool vglLinks = true;
            bool vglOben  = true;
            if (posLinks % 5 != 0 && posLinks >= 0)
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
