using System;
using System.Collections;

namespace personal.blackjack
{
    class Program
    {
        static int Debug_Level = 2;
        static int MAXGAME = 1000;
        static int NUMPLAYERS = 2;
        static int NUMDECKS = 6;

        static void TestCount()
        {
            Deck deck = new Deck(NUMDECKS);
            deck.Shuffle();

            while (deck.getRemainingCards() > 0)
            {
                Card c = deck.getCard();
                Console.WriteLine("{0} - Count:{1}", c.ToShortString(), deck.Count);
            }
        }


        static void Main(string[] args)
        {
            //TestCount();
            Game game = new Game(NUMPLAYERS, NUMDECKS, Debug_Level);
            game.PlayMultiple(MAXGAME);
        }
    }
}
