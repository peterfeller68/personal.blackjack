using System;
using System.Collections;
using System.Text;

namespace personal.blackjack
{
    class Hand
    {
        public Hand()
        {
            myCards = new ArrayList();
        }

        public void putCard(Card c)
        {
            myCards.Add(c);
        }

        public int getNumCards()
        {
            return myCards.Count;
        }

        public void Clear()
        {
            myCards.Clear();
        }

        public int getNumAces()
        {
            int numAces = 0;
            int val = 0;
            for (int x = 0; x < myCards.Count; x++)
            {
                Card c = (Card)myCards[x];
                if (c.type == CardType.Ace)
                {
                    numAces++;
                }
                else
                {
                    val += c.Value();
                }
            }
            return numAces;
        }

        public int getValue()
        {
            ArrayList aVal = new ArrayList();
            int numAces = 0;
            int val = 0;
            for (int x = 0; x < myCards.Count; x++)
            {
                Card c = (Card)myCards[x];
                if (c.type == CardType.Ace)
                {
                    numAces++;
                }
                else
                {
                    val += c.Value();
                }
            }

            // for each ace add 1 and 11 to val
            if (numAces > 0)
            {
                if (numAces == 1)
                {
                    aVal.Add(val + 1);
                    aVal.Add(val + 11);
                }
                else if (numAces == 2)
                {
                    aVal.Add(val + 2);
                    aVal.Add(val + 12);
                    aVal.Add(val + 22);
                }
                else if (numAces == 3)
                {
                    aVal.Add(val + 3);
                    aVal.Add(val + 13);
                    aVal.Add(val + 23);
                    aVal.Add(val + 33);
                }
                else
                {
                    throw new Exception("Not handling more than one aces");
                }
            }
            else
            {
                aVal.Add(val);
            }

            // return the value closest to 21, which does not exceed 21
            int maxLegalVal = -1;
            for (int x=0; x<aVal.Count; x++)
            {
                int valMax = (int)aVal[x];
                if (valMax <= 21 && valMax > maxLegalVal )
                {
                    maxLegalVal = valMax;
                }
            }
            // if there is no total less than 21, return the total closest to 21
            if (maxLegalVal == -1)
            {
                maxLegalVal = (int)aVal[0];
                for (int x = 1; x < aVal.Count; x++)
                {
                    int valMax = (int)aVal[x];
                    if (valMax < maxLegalVal)
                    {
                        maxLegalVal = valMax;
                    }
                }
            }
            return maxLegalVal;
        }

        public bool isBlackJack()
        {
            if (myCards.Count != 2) return false;

            Card c1 = (Card)myCards[0];
            Card c2 = (Card)myCards[1];
            if (c1.type != CardType.Ace && c2.type != CardType.Ace) return false;
            if (c1.Value() != 10 && c2.Value() != 10) return false;

            return true;
        }

        public void Print(bool bLong = false, string Title="")
        {
            int Cnt = 0;
            if (Title != "") Console.Write("{0} - ", Title);
            Console.Write("[");
            foreach (Card c in myCards)
            {
                if (bLong)
                {
                    Console.WriteLine(c);
                }
                else
                {
                    if (Cnt > 0) Console.Write(" ");
                    Console.Write("{0}", c.ToShortString());
                }
                Cnt++;
            }
            Console.Write("] - {0}", getValue());
//            ArrayList aVal = getValues();

//            for (int x=0; x<aVal.Count; x++)
//                Console.Write("{0}, ", aVal[x]);
//            Console.WriteLine();
            Console.WriteLine();
        }

        protected ArrayList myCards { get; }
    }
}
