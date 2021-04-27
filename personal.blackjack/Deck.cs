using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    class Deck
    {
        public Deck(int num = 1)
        {
            NumDecks = num;
            NumCards = 52* NumDecks;

            Init();
        }

        public void Shuffle()
        {
            Init();

            int x;
            Card[] temp = new Card[NumCards];

            bool[] used = new bool[NumCards];
            for (x = 0; x < NumCards; x++) used[x] = false;

            x = 0;
            while (cards.Count > 0)
            {
                temp[x++] = cards.Pop();
            }

            x = 0;
            Random rand = new Random(); // (int)DateTime.Now.Ticks);
            while (x < NumCards)
            {
                int randIndex = rand.Next(NumCards);
                if (used[randIndex] == false)
                {
                    cards.Push(temp[randIndex]);
                    used[randIndex] = true;
                    x++;
                }
            }

        }

        public Card getCard()
        {
            Card c = cards.Pop();
            Count += c.CountValue();

            return c;
        }

        public int getRemainingCards()
        {
            return cards.Count;
        }

        public int getNumHighCards()
        {
            int retVal = 0;
            foreach (Card c in cards)
            {
                if (c.CountValue() == -1) retVal++;
            }
            return retVal;
        }

        public int getNumLowCards()
        {
            int retVal = 0;
            foreach (Card c in cards)
            {
                if (c.CountValue() == 1) retVal++;
            }
            return retVal;
        }

        public void Print(bool bLong = false)
        {
            foreach (Card c in cards)
            {
                if (bLong)
                {
                    Console.WriteLine(c);
                }
                else
                {
                    Console.Write("{0}, ", c.ToShortString());
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public void PrintCountStats()
        {
            Console.WriteLine("Deck Count: {0}", Count);
            Console.WriteLine("Rem Cards: {0}", getRemainingCards());
            Console.WriteLine("Num High Cards: {0}", getNumHighCards());
            Console.WriteLine("Num Low Cards: {0}", getNumLowCards());
        }

        public int Count { get; set; }




        protected void Init()
        {
            cards.Clear();
            for (int d = 0; d < NumDecks; d++)
            {
                for (int x = (int)SuitType.Hearts; x <= (int)SuitType.Spades; x++)
                {
                    for (int y = (int)CardType.Two; y <= (int)CardType.Ace; y++)
                    {
                        SuitType s = (SuitType)x;
                        CardType c = (CardType)y;
                        cards.Push(new Card(s, c));
                    }
                }
            }
            Count = 0;
        }

        protected Stack<Card> cards = new Stack<Card>();
        protected int NumDecks { get; set; }
        protected int NumCards { get; set; }
        
    }
}
