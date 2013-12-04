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
        private static DateTime CalcStart;
        private static BigInteger MaxAnzahlZüge;
        private static BigInteger AnzahlZügeÜbersprungen;
        private static int AnzahlCards = 0;

        static void Main(string[] args)
        {
            Cards = CardSet.CreateCards();
            AnzahlCards = Cards.Count();
            PrintCards(Cards);
            MaxAnzahlZüge = CalculatePossibilities(25);
            Console.WriteLine("MaxAnzahlZüge: " + MaxAnzahlZüge);
            Spielfeld =  new CardSet[AnzahlCards];
            CalcStart = DateTime.Now;
            LegeKarte(0);
            Console.WriteLine("Alles durchprobiert.");
            Console.ReadLine();
        }

        private static BigInteger CalculatePossibilities(int cardCount)
        {
            var result = new BigInteger(1);
            for (int i = cardCount * 4; i != 0; i-=4)
            {
                result *= i;
            }
            return result;
        }

        private static void LegeKarte(int spielfeldPos)
        {
            AnzahlZuege += 1;
            bool recurse = false;

            if (spielfeldPos == AnzahlCards)
            {
                //Lösung gefunden?
                Console.WriteLine("#####################################################################################");
                Console.WriteLine("Lösung gefunden:");
                PrintCards(Spielfeld);
                Console.WriteLine();
                Console.WriteLine(string.Format("Dauer            : {0:mmss}", CalcStart - DateTime.Now));
                Console.WriteLine(string.Format("Anzahl Züge      : {0,45}", AnzahlZuege));
                Console.WriteLine(string.Format("Züge Übersprungen: {0,45}", AnzahlZügeÜbersprungen));
                Console.WriteLine(string.Format("Max Züge         : {0,45}", MaxAnzahlZüge));
                Console.WriteLine(string.Format("Züge offen       : {0,45}", MaxAnzahlZüge - AnzahlZügeÜbersprungen - AnzahlZuege));
                CalcStart = DateTime.Now;
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
                        recurse = true;
                        LegeKarte(spielfeldPos + 1);
                        Cards[i].Used = false;
                    }
                    Cards[i].Rotate();
                }
                if(!recurse)
                {
                    int unused = GetUnusedCardCount() - 1;
                    AnzahlZügeÜbersprungen += CalculatePossibilities(unused);
                }
            }
        }

        private static int GetUnusedCardCount()
        {
            return Cards.Where(c => !c.Used).Count();
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
