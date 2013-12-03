using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Rubik
{
    class Program
    {
        private static long AnzahlZuege = -1;
        private static CardSet[] Spielfeld;
        private static CardSet[] Cards;
        private static DateTime LastTime = DateTime.Now;
        private static BigInteger MaxAnzahlZüge;
        private static int AnzahlCards = 0;

        static void Main(string[] args)
        {
            Cards = CardSet.CreateCards();
            AnzahlCards = Cards.Count();
            MaxAnzahlZüge = 1;

            PrintCards(Cards);
            for (int i = 100; i != 0; i-=4)
            {
                MaxAnzahlZüge *= i;
            }
            Console.WriteLine("MaxAnzahlZüge: " + MaxAnzahlZüge);
            Spielfeld =  new CardSet[AnzahlCards];

            LegeKarte(0);
            Console.WriteLine("keine lösung gefunden.");
            Console.ReadLine();
        }

        private static void LegeKarte(int spielfeldPos)
        {
            AnzahlZuege += 1;

            //if (AnzahlZuege % 100000 == 0)
            //{
            //    TimeSpan timeSpan = DateTime.Now - LastTime;
            //    Console.WriteLine(string.Format("{0:mmss}\tZüge: {1}\tPosition: {2}", timeSpan, AnzahlZuege, spielfeldPos));
            //    LastTime = DateTime.Now;
            //}

            if (spielfeldPos == AnzahlCards)
            {
                //fertig?
                Console.WriteLine("Fertig; Züge: " + AnzahlZuege);
                PrintCards(Spielfeld);
                return;
            }
            for (int i = 0; i < AnzahlCards; i++)
            {
                if (Cards[i].Used) 
                    continue;

                for (int rotations = 0; rotations < 4; rotations++)
                {
                    if (Compare(spielfeldPos, Cards[i]))
                    {
                        Spielfeld[spielfeldPos] = Cards[i];
                        Cards[i].Used = true;
                        LegeKarte(spielfeldPos + 1);
                        Cards[i].Used = false;
                    }
                    Cards[i].Rotate();
                }
            }
        }

        private static void PrintCards(CardSet[] cards)
        {
            for (int row = 0; row < 25; row += 5)
            {
                for (int col = 0; col < 5; col++)
                {
                    cards[row+col].Print(0);
                }
                Console.WriteLine();
                for (int col = 0; col < 5; col++)
                {
                    cards[row + col].Print(1);
                }
                Console.WriteLine();
                for (int col = 0; col < 5; col++)
                {
                    cards[row + col].Print(2);
                }
                Console.WriteLine();
                for (int col = 0; col < 5; col++)
                {
                    cards[row + col].Print(3);
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------");
            }
        }



        private static bool Compare(int pos, CardSet cardSet)
        {
            int posLinks = pos - 1;
            bool vglLinks = true;
            bool vglOben  = true;
            if (pos % 5 != 0 && posLinks >= 0)
            {
                var links = Spielfeld[posLinks];
                vglLinks = links.Card[2] == cardSet.Card[7] && links.Card[3] == cardSet.Card[6];
            }
            int posOben = pos - 5;
            if (posOben >= 0)
            {
                var oben = Spielfeld[posOben];
                vglOben = oben.Card[5] == cardSet.Card[0] && oben.Card[4] == cardSet.Card[1];
            }
            return vglLinks && vglOben;
        }

    }
}
