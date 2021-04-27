using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    public enum SuitType { Hearts, Diamonds, Clubs, Spades };
    public enum CardType { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace};


    class Card
    {
        public Card(SuitType s, CardType c)            
        {
            type = c;
            suit = s;
        }

        public int Value()
        {
            switch (type)
            {
                case CardType.Ace: 
                    return 1;
                case CardType.King:
                case CardType.Queen: 
                case CardType.Jack: 
                case CardType.Ten:
                    return 10;
                case CardType.Nine:
                    return 9;
                case CardType.Eight:
                    return 8;
                case CardType.Seven:
                    return 7;
                case CardType.Six:
                    return 6;
                case CardType.Five:
                    return 5;
                case CardType.Four:
                    return 4;
                case CardType.Three:
                    return 3;
                case CardType.Two:
                    return 2;
            }
            throw new Exception("Unknown Card");
        }
        public int CountValue()
        {
            switch (type)
            {
                case CardType.Ten:
                case CardType.Jack:
                case CardType.Queen:
                case CardType.King:
                case CardType.Ace:
                    return -1;
                case CardType.Two:
                case CardType.Three:
                case CardType.Four:
                case CardType.Five:
                case CardType.Six:
                    return 1;
            }
            return 0;
        }
        public override string ToString()
        {
            string suitStr = "Unk";
            string cardStr = "Unk";
            switch (suit)
            {
                case SuitType.Hearts: suitStr = "Hearts"; break;
                case SuitType.Clubs: suitStr = "Clubs"; break;
                case SuitType.Diamonds: suitStr = "Diamonds"; break;
                case SuitType.Spades: suitStr = "Spades"; break;
            }
            switch (type)
            {
                case CardType.Ace: cardStr = "Ace"; break;
                case CardType.King: cardStr = "King"; break;
                case CardType.Queen: cardStr = "Queen"; break;
                case CardType.Jack: cardStr = "Jack"; break;
                case CardType.Ten: cardStr = "Ten"; break;
                case CardType.Nine: cardStr = "Nine"; break;
                case CardType.Eight: cardStr = "Eight"; break;
                case CardType.Seven: cardStr = "Seven"; break;
                case CardType.Six: cardStr = "Six"; break;
                case CardType.Five: cardStr = "Five"; break;
                case CardType.Four: cardStr = "Four"; break;
                case CardType.Three: cardStr = "Three"; break;
                case CardType.Two: cardStr = "Two"; break;
            }

            return string.Format("{0} of {1}", cardStr, suitStr);
        }
        public string ToShortString()
        {
            string suitStr = "Unk";
            string cardStr = "Unk";
            switch (suit)
            {
                case SuitType.Hearts: suitStr = "H"; break;
                case SuitType.Clubs: suitStr = "C"; break;
                case SuitType.Diamonds: suitStr = "D"; break;
                case SuitType.Spades: suitStr = "S"; break;
            }
            switch (type)
            {
                case CardType.Ace: cardStr = "A"; break;
                case CardType.King: cardStr = "K"; break;
                case CardType.Queen: cardStr = "Q"; break;
                case CardType.Jack: cardStr = "J"; break;
                case CardType.Ten: cardStr = "10"; break;
                case CardType.Nine: cardStr = "9"; break;
                case CardType.Eight: cardStr = "8"; break;
                case CardType.Seven: cardStr = "7"; break;
                case CardType.Six: cardStr = "6"; break;
                case CardType.Five: cardStr = "5"; break;
                case CardType.Four: cardStr = "4"; break;
                case CardType.Three: cardStr = "3"; break;
                case CardType.Two: cardStr = "2"; break;
            }

            return string.Format("{0}{1}", suitStr, cardStr);
        }


        public CardType type { get; set; }
        public SuitType suit { get; }
    }
}
