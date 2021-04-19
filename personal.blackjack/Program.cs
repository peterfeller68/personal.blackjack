using System;

namespace personal.blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            //deck.Print();
            deck.Print(false);
            deck.Shuffle();
            deck.Print(false);
        }
    }
}
