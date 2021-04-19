using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    class Deck
    {
        public Deck()
        {
            Init();
        }

        public void Shuffle()
        {
            Init();

            int x;
            Card[] temp = new Card[52];

            bool[] used = new bool[52];
            for (x = 0; x < 52; x++) used[x]= false;
            
            x = 0;
            while (cards.Count > 0)
            {
                temp[x++] = cards.Pop();
            }

            x = 0; 
            Random rand = new Random((int)DateTime.Now.Ticks);
            while (x<52)
            {
                int randIndex = rand.Next(52);
                if (used[randIndex] == false)
                {
                    cards.Push(temp[randIndex]);
                    used[randIndex] = true;
                    x++;
                }
            }

        }

        public void Print(bool bLong = true)
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

        protected void Init()
        {
            cards.Clear();
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

        protected Stack<Card> cards= new Stack<Card>();
    }
}
