using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    class Dealer
    {
        public Dealer (Deck d, int dbgLvl = 0)
        {
            deck = d;
            hand = new Hand();
            DebugLevel = dbgLvl;
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
                if (DebugLevel == 2)
                {
                    hand.Print(false, "Dealer");

                    int numAces = hand.getNumAces();
                    if (numAces > 0)
                    {
                        Console.WriteLine("ACES - {0}", numAces);
                    }
                }

                int handValue = hand.getValue();
                //int handValue = (int)handValues[0];
                //if (handValues.Count > 1 && dealerHand.getNumCards() > 2)
                //    throw new Exception("Not yet handling aces for dealer");

                if (hand.isBlackJack())
                {
                    done = true;
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer has Blackjack");
                    }
                }
                else if (handValue > 21)
                {
                    done = true;
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer Busted");
                    }
                }
                else if (handValue >= 17)
                {
                    done = true;
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer is Done - {0} points", handValue);
                    }
                }
                else
                {
                    // hit me
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Dealer requests a card");
                    }
                    hand.putCard(deck.getCard());
                }
            }
            if (DebugLevel == 2)
            {
                Console.WriteLine();
            }

        }

        public int DebugLevel { get; set; }
        public Hand hand { get; set; }
        public Deck deck { get; set; }
    }
}
