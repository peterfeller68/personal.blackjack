using System;
using System.Collections;

namespace personal.blackjack
{
    class Program
    {
        //static void TestHand()
        //{
        //    Deck deck = new Deck(6);
        //    deck.Shuffle();

        //    while (deck.getRemainingCards() > 0)
        //    {
        //        Card c = deck.getCard();
        //        Console.Write("{0} ", c.ToShortString());
        //        if (deck.Count == 10)
        //        {
        //            Console.WriteLine();
        //            deck.PrintCountStats();
        //            Console.WriteLine();
        //            deck.Print();
        //        }
        //        Console.WriteLine();
        //    }
        //}

        static void TestCount()
        {
            Deck deck = new Deck(6);
            deck.Shuffle();

            while (deck.getRemainingCards() > 0)
            {
                Card c = deck.getCard();
                Console.Write("{0} ", c.ToShortString());
                if (deck.Count == 10)
                {
                    Console.WriteLine();
                    deck.PrintCountStats();
                    Console.WriteLine();
                    deck.Print();
                }
                Console.WriteLine();
            }
        }

        static void RunTestOnHitTargetbasedOnVisibleDealerCard()
        {
            Strategy strat = new Strategy();

            for (int x = 1; x <= 10; x++)
            {
                strat.opVisibleDealerCard = x;
                for (int target = 11; target <= 21; target++)
                {
                    // initialize to hit 17
                    for (int y = 1; y <= 10; y++) strat.PlayerHitLimit[y] = 17;
                    strat.PlayerHitLimit[x] = target;

                    Game game = new Game(strat);
                    game.PlayMultiple();
                }
            }
        }

        static void PlaySingleStrategy()
        {
            Strategy strat = new Strategy();

            Game game = new Game(strat);
            game.PlayMultiple();
        }

        static void PlayInteractive()
        {
            Strategy strat = new Strategy();

            Game game = new Game(strat);
            game.PlayInteractive();
        }

        static void Main(string[] args)
        {
            //RunTestOnHitTargetbasedOnVisibleDealerCard();
            //PlaySingleStrategy();
            //TestCount();
            PlayInteractive();
        }
    }
}
