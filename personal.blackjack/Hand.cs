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
            Clear();
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

            doubleDown = false;
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

        public bool isBust()
        {
            return getValue() > 21;
        }

        public bool possibleSplit()
        {
            if (myCards.Count != 2) return false;

            Card c1 = (Card)myCards[0];
            Card c2 = (Card)myCards[1];
            return c1.Value() == c2.Value();
        }

        public bool canSplit()
        {
            if (possibleSplit() == false) return false;

            Card c1 = (Card)myCards[0];
            // always split aces or eights
            if (c1.type == CardType.Ace || c1.type == CardType.Eight)
            {
                return true;
            }

            // never split fours or fives
            if (c1.type == CardType.Four || c1.type == CardType.Five)
            {
                return false;
            }

            return false;
        }

        public bool canDoubleDown(Card dealerVisCard)
        {
            if (myCards.Count == 2 && getValue() == 11)
                return true;
            if (dealerVisCard.type != CardType.Ace && dealerVisCard.Value() < 7)
            {
                // dealer shows low card nad hand is soft 16, 17, 18
                if (getNumAces() == 1 && getValue() >= 16 && getValue() <= 18)
                    return true;
                if (getNumAces() == 0 && getValue() >= 9 && getValue() <= 10)
                    return true;
            }
            return false;
        }

        public Card getCard(int index)
        {
            return (Card)myCards[index];
        }

        public void Print(bool bLong = false, string Title="", Card vc=null, string HandNr = "")
        {
            int Cnt = 0;
            if (Title != "")
            {
                if (HandNr !="")
                {
                    HandNr = string.Format("[{0}] ", HandNr);
                }
                Console.Write("{0} {1}- ", Title, HandNr);
            }
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
                    string strV = "";
                    if(vc == c) strV = "*";
                    Console.Write("{1}{0}", c.ToShortString(), strV);
                }
                Cnt++;
            }
            Console.Write("] - {0}", getValue());
            Console.WriteLine();
        }

        protected ArrayList myCards { get; }
        public bool doubleDown { get; set; }
    }
}
