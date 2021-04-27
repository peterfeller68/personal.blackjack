using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{

    class Player
    {
        static public int PlayerNum = 0;

        public Player (Strategy s, Dealer d)
        {
            strat = s;
            playerNum = ++PlayerNum;
            hand1 = new Hand();
            hand2 = new Hand(); // in case of a split
            stats = new Statistics(strat);
            dealer = d;
        }

        protected string Name() { return string.Format("Player{0}", playerNum); }
        public void ClearHand()
        {
            hand1.Clear();
            hand2.Clear();
        }

        public void DealCard(Card c, Hand hand)
        {
            hand.putCard(c);
        }

        public void InteractiveWager()
        {
            Console.WriteLine("{0}: Please input your wager", Name());
            Console.Write("Wager Def=10: ");
            string wager = Console.ReadLine();
            if (wager == "") wager = "10";
            gameWager = int.Parse(wager);
        }

        public void Wager(int DeckCount)
        {
            if (DeckCount < 11)
            {
                gameWager = strat.PlayerMinBet * strat.BettingUnit;
            }
            else
            {
                gameWager = strat.PlayerMaxBet * strat.BettingUnit;
            }
        }

        protected void HitInteractive(Hand playHand)
        {
            playHand.putCard(dealer.getCard());
        }

        protected void DoubleDownInteractive(Hand playHand)
        {
            playHand.putCard(dealer.getCard());
            gameWager = gameWager * 2;
        }

        protected void SplitInteractive(Hand playHand)
        {
            Card c1 = (Card)playHand.getCard(0);
            Card c2 = (Card)playHand.getCard(1);

            hand1.Clear();
            hand1.putCard(c1);
            hand2.putCard(c2);
            hand1.putCard(dealer.getCard());
            hand2.putCard(dealer.getCard());
        }

        public void EvalInteractive()
        {
            bool bMultipleHands = hand2.getNumCards() > 0;
            
            Console.WriteLine("");
            dealer.hand.Print(false, "Final Dealer Hand");

            if (bMultipleHands)
            {
                Console.WriteLine(""); 
                Console.WriteLine("Evaluating Hand1");
                Console.WriteLine("----------------");
            }
            EvalInteractive(hand1);
            if (hand2.getNumCards() > 0)
            {
                if (bMultipleHands)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Evaluating Hand2");
                    Console.WriteLine("----------------");
                }
                EvalInteractive(hand2);
            }
        }

        public void EvalInteractive(Hand playerHand)
        {
            GameData res = Evaluate(playerHand);
            playerHand.Print(false, string.Format("Final Hand {0} - ", Name()));
            Console.WriteLine("");
            if (res.gameResult == Statistics.Result.Loss)
                Console.WriteLine("Player Lost");
            else if (res.gameResult == Statistics.Result.Win)
                Console.WriteLine("Player Won");
            else if (res.gameResult == Statistics.Result.Push)
                Console.WriteLine("Player Pushed");
            Console.WriteLine("Current Winnings: {0}", stats.total.Winnings);
        }

        public bool PlayInteractive()
        {
            bool playAgain = true;
            dealer.PrintInteractiveHand();

            playAgain = PlayInteractiveHand(hand1, true);
            //if (playAgain && hand2.getNumCards() > 0)
            //{
            //    playAgain = PlayInteractiveHand(hand2);
            //}
            return playAgain;
        }

        public bool PlayInteractiveHand(Hand playHand, bool injectHand=false)
        {
            ////////////////////////////////////////////////////////////
            /// inject a certain hand
            ////////////////////////////////////////////////////////////
            if (injectHand)
            {
                playHand.Clear();
                playHand.putCard(new Card(SuitType.Clubs, CardType.Eight));
                playHand.putCard(new Card(SuitType.Hearts, CardType.Eight));
            }

            bool playAgain = true;
            bool gameOver = playHand.isBlackJack();
            while (!gameOver)
            {
                PrintInteractiveHand(playHand);
                Console.WriteLine("");

                gameOver = playHand.isBlackJack() || playHand.isBust();
                if (gameOver) break;

                Console.Write("[H]-Hit");
                if (playHand.possibleSplit()) Console.Write(",   [S]-Split");
                Console.Write(",   [D]-Double Down");
                Console.Write(",   [X]-Stay");
                Console.Write(",   [Q]-Quit");

                string choice = "";
                bool bGoodChoice = false;
                while (!bGoodChoice)
                {
                    Console.Write("   ------> Choice: ");
                    choice = Console.ReadLine().ToUpper();
                    switch (choice)
                    {
                        case "H": bGoodChoice = true; break;
                        case "D": bGoodChoice = true; break;
                        case "S": bGoodChoice = playHand.possibleSplit(); break;
                        case "X": bGoodChoice = true; break;
                        case "Q": bGoodChoice = true; break;
                    }
                    if (!bGoodChoice)
                    {
                        Console.WriteLine("Illegal Input - [{0}]", choice);
                        Console.WriteLine();
                        break;
                    }
                }

                Console.WriteLine();
                switch (choice)
                {
                    case "H":
                        HitInteractive(playHand);
                        break;
                    case "D":
                        Console.WriteLine("Player doubled down");
                        Console.WriteLine();
                        DoubleDownInteractive(playHand);
                        gameOver = true;
                        break;
                    case "S":
                        Console.WriteLine("Player Split");
                        Console.WriteLine();
                        SplitInteractive(playHand);

                        Console.WriteLine("Playing Hand1");
                        Console.WriteLine("-------------");
                        playAgain = PlayInteractiveHand(hand1);
                        if (playAgain)
                        {
                            Console.WriteLine("Playing Hand2");
                            Console.WriteLine("-------------");
                            playAgain = PlayInteractiveHand(hand2);
                        }
                        gameOver = true;
                        break;
                    case "X":
                        gameOver = true;
                        break;
                    case "Q":
                        gameOver = true;
                        playAgain = false;
                        break;
                }
            }
            return playAgain;
        }

        public void Play()
        {
            Play(hand1);
            if (hand2.getNumCards() > 0)
            {
                Play(hand2, "Hand2");
            }
        }

        protected void Play(Hand hand, string HandNr="")
        {
            int visCardVal = dealer.VisibleCard.Value();
            int PlayerHitLimit = strat.PlayerHitLimit[visCardVal];

            if (strat.DebugLevel == 2)
            {
                Console.WriteLine("Hit until {0}", PlayerHitLimit);
            }

            bool done = false;
            while (!done)
            {
                if (strat.DebugLevel == 2)
                {
                    hand.Print(false, Name(), null, HandNr);
                }

                int handValue = hand.getValue();
                if (hand.isBlackJack())
                {
                    done = true;
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Player has Blackjack");
                    }
                }
                else if (handValue > 21)
                {
                    done = true;
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Player Busted");
                    }
                }
                // Split ?
                else if (hand2.getNumCards() == 0 && hand.canSplit())
                {
                    Card c1 = (Card)hand1.getCard(0);

                    Card c2 = (Card)hand1.getCard(1);
                    hand1.Clear();
                    hand1.putCard(c1);
                    hand2.putCard(c2);
                    hand1.putCard(dealer.getCard());
                    hand2.putCard(dealer.getCard());

                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Player Splits");
                    }
                    Play();
                }
                // Double Down ?
                else if (hand1.canDoubleDown(dealer.VisibleCard))
                {
                    hand1.doubleDown = true;
                    hand1.putCard(dealer.getCard());
                    gameWager = gameWager * 2;

                    done = true;

                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Player double down!");
                        hand1.Print(false, Name(), null, HandNr);
                    }
                }
                else if (handValue >= PlayerHitLimit)
                {
                    done = true;
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Player is Done - {0} points", handValue);
                    }
                }
                else
                {
                    // hit me
                    if (strat.DebugLevel == 2)
                    {
                        Console.WriteLine("Player requests a card");
                    }
                    hand.putCard(dealer.getCard());
                }
            }   
            if (strat.DebugLevel == 2)
            {
                Console.WriteLine();
            }
        }

        public void Evaluate()
        {
            Evaluate(hand1);
            if (hand2.getNumCards() > 0)
            {
                Evaluate(hand2);
            }
        }

        protected GameData Evaluate(Hand hand)
        {
            GameData res = new GameData();

            int playerHandValue = hand.getValue();
            int dealerHandValue = dealer.hand.getValue();

            // evaluate the game
            if (hand.isBlackJack() && dealer.hand.isBlackJack())
            {
                res.gameResult = Statistics.Result.Push;
            }
            else if (hand.isBlackJack() && !dealer.hand.isBlackJack())
            {
                res.gameResult = Statistics.Result.Win;
            }
            else if (dealer.hand.isBlackJack())
            {
                res.gameResult = Statistics.Result.Loss;
            }
            else if (playerHandValue > 21)
            {
                res.gameResult = Statistics.Result.Loss;
            }
            else if (dealerHandValue > 21)
            {
                res.gameResult = Statistics.Result.Win;
            }
            else if (playerHandValue == dealerHandValue)
            {
                res.gameResult = Statistics.Result.Push;
            }
            else if (playerHandValue > dealerHandValue)
            {
                res.gameResult = Statistics.Result.Win;
            }
            else if (playerHandValue < dealerHandValue)
            {
                res.gameResult = Statistics.Result.Loss;
            }
            else
            {
                throw new Exception("Case not handled");
            }

            string strRes = "";
            if (res.gameResult == Statistics.Result.Loss)
            {
                strRes = "Lost";
            }
            if (res.gameResult == Statistics.Result.Win)
            {
                strRes = "Won";
                res.blackJack = hand.isBlackJack();
            }
            if (res.gameResult == Statistics.Result.Push)
            {
                strRes = "Push";
            }

            if (strat.DebugLevel == 2)
            {
                Console.WriteLine("Wager:{0}", gameWager);
                Console.WriteLine("{0} {1}", Name(), strRes);
            }

            stats.Update(res
                , dealer.deck.Count
                , dealer.VisibleCard.Value()
                , hand.doubleDown
                , dealer.hand.isBlackJack()
                , hand2.getNumCards()>0
                , gameWager);

            return res;
        }

        public void PrintResults()
        {
            if (strat.opVisibleDealerCard == -1)
            {
                Console.WriteLine("Player: {0}", Name());
                stats.PrintResults();
                Console.WriteLine();
            }
            else
            {
                stats.PrintResults();
            }
        }

        public void PrintInteractiveHand(Hand hand)
        {
            string strHand = "";
            if(hand2.getNumCards()>0)
            {
                if (hand == hand1) strHand = "1";
                if (hand == hand2) strHand = "2";
            }
            Console.Write("{0} Hand{1}: ", Name(), strHand);
            for (int x = 0; x < hand.getNumCards(); x++)
            {
                Card c = hand.getCard(x);
                if (x > 0) Console.Write(" ");
                Console.Write("{0}", c.ToShortString());
            }
            Console.Write(" - {0}", hand.getValue());
            Console.WriteLine();
        }


        public Hand hand1 { get; set; }
        public Hand hand2 { get; set; } // in case of split
        public Dealer dealer { get; set; }
        public Statistics stats { get; set; }

        protected int playerNum = 0;
        public int gameWager { get; set; }
        protected Strategy strat;
    }
}
