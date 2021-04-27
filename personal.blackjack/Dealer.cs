using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    class Dealer
    {
        public Dealer (Strategy s, Deck d)
        {
            strat = s;
            deck = d;
            hand = new Hand();
        }

        public Card getCard()
        {
            return deck.getCard();
        }

        public void ClearHand()
        {
            hand.Clear();
        }

        public void DealCard(Card c)
        {
            hand.putCard(c);
        }

        public void Play()
        {
            /////////////////////////////////////////////////////////////////
            /// The dealer is going to ask for cards until he hits a 17
            /////////////////////////////////////////////////////////////////
            bool done = false;
            while (!done)
            {
                if (strat.DebugLevel == 2)
                {
                    hand.Print(false, "Dealer", VisibleCard);

                }

                int handValue = hand.getValue();
                if (hand.isBlackJack())
                {
                    done = true;
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer has Blackjack");
                    }
                }
                else if (handValue > 21)
                {
                    done = true;
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer Busted");
                    }
                }
                else if (handValue >= 17)
                {
                    done = true;
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer is Done - {0} points", handValue);
                    }
                }
                else
                {
                    // hit me
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer requests a card");
                    }
                    hand.putCard(deck.getCard());
                }
            }
            if (strat.DebugLevel == 2)
            {
                Console.WriteLine();
            }

        }

        public void PrintInteractiveHand()
        {
            Console.Write("Dealer Hand: "); 
            for (int x=0; x<hand.getNumCards(); x++)
            {
                Card c = hand.getCard(x);
                if (x > 0) Console.Write(" ");
                if (c == this.VisibleCard)
                {
                    Console.Write("*");
                }    
                else
                {
                    Console.Write("{0}", c.ToShortString());
                }
            }
            Console.WriteLine();
        }

        public Card VisibleCard { get; set; }
        public Hand hand { get; set; }
        public Deck deck { get; set; }

        protected Strategy strat;
    }
}
