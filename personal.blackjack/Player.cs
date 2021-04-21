using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{

    class Player
    {
        enum Result { Unk, Push, Win, Loss };
        static int PlayerNum = 0;
        static int TABLEMIN = 1;
        static int TABLEMAX = 1000;

        public Player (Dealer d, int dbgLvl = 0)
        {
            playerNum = ++PlayerNum;
            hand = new Hand();
            dealer = d;

            DebugLevel = dbgLvl;
        }

        protected string Name() { return string.Format("Player{0}", playerNum); }
        public void ClearHand()
        {
            hand.Clear();
        }

        public void DealCard(Card c)
        {
            hand.putCard(c);
        }

        public void Wager(int DeckCount)
        {
            if (DeckCount < 11)
            {
                gameWager = TABLEMIN;
            }
            else
            {
                gameWager = TABLEMAX;
            }
        }
        public void Play()
        {
            bool done = false;
            while (!done)
            {
                if (DebugLevel == 2)
                {
                    hand.Print(false, Name());
                    int numAces = hand.getNumAces();
                    if (numAces > 0)
                    {
                        Console.WriteLine("ACES - {0}", numAces);
                    }
                }

                //ArrayList handValues = playerHand.getValues();
                int handValue = hand.getValue();
                //if (handValues.Count > 1 && playerHand.getNumCards()>2)
                //    throw new Exception("Not yet handling aces for player");

                if (hand.isBlackJack())
                {
                    done = true;
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Player has Blackjack");
                    }
                }
                else if (handValue > 21)
                {
                    done = true;
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Player Busted");
                    }
                }
                else if (handValue >= 17)
                {
                    done = true;
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Player is Done - {0} points", handValue);
                    }
                }
                else
                {
                    // hit me
                    if (DebugLevel == 2)
                    {
                        Console.WriteLine("Player requests a card");
                    }
                    hand.putCard(dealer.getCard());
                }
            }
            if (DebugLevel == 2)
            {
                Console.WriteLine();
            }
        }

        public void Evaluate()
        {
            Result gameResult = Result.Unk;

            int playerHandValue = hand.getValue();
            int dealerHandValue = dealer.hand.getValue();

            // evaluate the game
            if (hand.isBlackJack() && dealer.hand.isBlackJack())
            {
                gameResult = Result.Push;

                playerAndDealerBlackJack++;
            }
            else if (hand.isBlackJack() && !dealer.hand.isBlackJack())
            {
                gameResult = Result.Win;

                playerBlackJack++;
            }
            else if (dealer.hand.isBlackJack())
            {
                gameResult = Result.Loss;
            }
            else if (playerHandValue > 21)
            {
                gameResult = Result.Loss;
            }
            else if (dealerHandValue > 21)
            {
                gameResult = Result.Win;
            }
            else if (playerHandValue == dealerHandValue)
            {
                gameResult = Result.Push;
            }
            else if (playerHandValue > dealerHandValue)
            {
                gameResult = Result.Win;
            }
            else if (playerHandValue < dealerHandValue)
            {
                gameResult = Result.Loss;
            }
            else
            {
                throw new Exception("Case not handled");
            }

            if (gameResult == Result.Loss)
            {
                playerLost++;
                TotDeckCountAtLoss += dealer.deck.Count;
                if (DebugLevel == 2)
                {
                    Console.WriteLine("{0} Lost", Name());
                }
                Winnings -= gameWager;
            }
            if (gameResult == Result.Win)
            {
                playerWin++;
                TotDeckCountAtWin += dealer.deck.Count;
                if (DebugLevel == 2)
                {
                    Console.WriteLine("{0} Won", Name());
                }
                if (hand.isBlackJack())
                    Winnings += gameWager*1.5;
                else
                    Winnings += gameWager;
            }
            if (gameResult == Result.Push)
            {
                playerPush++;
                if (DebugLevel == 2)
                {
                    Console.WriteLine("{0} Pushed", Name());
                }
            }

            NumGames++;
        }

        public void PrintResults()
        {
            Console.WriteLine("Player: {0}", Name());
            Console.WriteLine("Games Played: {0}", NumGames);
            Console.WriteLine("BLACKJACK:    {0} [{1:0.#}%]", playerBlackJack, (double)playerBlackJack / NumGames * 100);
            Console.WriteLine("Games Pushed: {0} [{1:0.#}%]", playerPush, (double)playerPush / NumGames * 100);
            Console.WriteLine("Games Won:    {0} [{1:0.#}%]", playerWin, (double)playerWin / NumGames * 100);
            Console.WriteLine("Games Lost:   {0} [{1:0.#}%]", playerLost, (double)playerLost / NumGames * 100);
            Console.WriteLine("BLACKJACK for Player and Dealer:   {0} [{1:0.#}%]", playerAndDealerBlackJack, (double)playerAndDealerBlackJack / NumGames * 100);
            Console.WriteLine("Avg Deck Count at Win: [{0:0.##}]", (double)TotDeckCountAtWin / playerWin);
            Console.WriteLine("Avg Deck Count at Loss: [{0:0.##}]", (double)TotDeckCountAtLoss / playerLost);
            Console.WriteLine("Winnings: {0:0.#}", Winnings);
            Console.WriteLine();
        }



        public Hand hand { get; set; }
        public Dealer dealer { get; set; }
        public int DebugLevel { get; set; }

        protected int playerNum = 0;
        protected int NumGames = 0;
        protected int TotDeckCountAtWin = 0;
        protected int TotDeckCountAtLoss = 0;
        protected int playerBlackJack = 0;
        protected int playerAndDealerBlackJack = 0;
        protected int playerWin = 0;
        protected int playerPush = 0;
        protected int playerLost = 0;
        protected int gameWager = 0;
        protected double Winnings = 0;
    }
}
