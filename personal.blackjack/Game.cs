using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace personal.blackjack
{
    class Game
    {

        public Game (Strategy s)
        {
            Player.PlayerNum = 0;
            strat = s;
            deck = new Deck(strat.NumDecks);
            dealer = new Dealer(strat, deck);

            players = new ArrayList();
            for (int x = 0; x < strat.NumPlayers; x++)
            {
                players.Add(new Player(strat, dealer));
            }
        }

        protected void Deal()
        {
            for (int x=0; x< players.Count; x++)
            {
                Player player = (Player)players[x];
                player.ClearHand();
            }
            dealer.ClearHand();

            // deal 2 cards to the player and dealer
            for (int n = 0; n < 2; n++)
            {
                for (int x = 0; x < players.Count; x++)
                {
                    Player player = (Player)players[x];
                    if (player.AllowedToPlay)
                    {
                        // the original deal is always to hand1
                        player.DealCard(deck.getCard(), player.hand1);
                    }
                }
                Card c = deck.getCard();
                dealer.DealCard(c);
                if (n==1)
                {
                    dealer.VisibleCard = c;
                }
            }
        }

        protected void Play()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                player.Play();
            }
            dealer.Play();
        }

        protected void Evaluate()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                player.Evaluate();
            }

            if (strat.DebugLevel == 2)
            {
                Console.WriteLine();
            }
        }

        protected void Wager()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                player.Wager(deck.Count);
            }
        }

        protected void InteractiveWager()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                bool bPlayerPayed = player.InteractiveWager();
                player.AllowedToPlay = bPlayerPayed;
                Console.WriteLine();
            }
        }

        protected int NumPlayersPlaying()
        {
            int num = 0;
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                if (player.AllowedToPlay) num++;
            }
            return num;
        }

        protected void InteractiveEvaluate()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                if (player.AllowedToPlay)
                {
                    player.EvalInteractive();
                }
            }
        }

        protected void InteractivePlay()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                if (player.AllowedToPlay)
                {
                    player.PlayInteractive();
                }
            }
            dealer.Play();
        }

        protected void InteractiveStats()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                player.stats.total.PrintResults();
            }
        }

        public void PlayInteractive()
        {
            GameNum = 0;
            deck.Shuffle();
            bool bNewShuffle = true;
            while (true)
            {
                int cardsInDeck = deck.getRemainingCards();
                if (cardsInDeck <= (1+NumPlayersPlaying()) * 5)
                {
                    deck.Shuffle();
                    bNewShuffle = true;
                }

                if (GameNum > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Play again? [Y/N]");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.KeyChar == 'n' || key.KeyChar == 'N') break;
                }
                Console.Clear();
                Console.WriteLine("===============================================");
                Console.WriteLine("Game #{0}", GameNum);
                if (bNewShuffle) Console.WriteLine("FULL DECK");
                Console.WriteLine("===============================================");

                InteractiveWager();
                if (NumPlayersPlaying() > 0)
                {
                    Deal();
                    bNewShuffle = false;

                    if (dealer.hand.isBlackJack())
                    {
                        Console.WriteLine("Dealer has Blackjack");
                    }
                    else
                    {
                        InteractivePlay();
                    }
                    InteractiveEvaluate();
                    GameNum++;
                }
                else
                {
                    Console.WriteLine("There are no players");
                }

            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Final Statistics");
            Console.WriteLine("----------------");
            InteractiveStats();
        }

        public void PlayMultiple()
        {
            GameNum = 0;
            while (GameNum < strat.NumGames)
            {
                deck.Shuffle();
                while (deck.getRemainingCards() > (players.Count+1)*5 && GameNum < strat.NumGames)
                {
                    if (strat.DebugLevel==2)
                    {
                        Console.WriteLine("===============================================");
                        Console.WriteLine("Game #{0} - Count is {1}", GameNum, deck.Count);
                        Console.WriteLine("===============================================");
                    }

                    try
                    {
                        Wager();
                        Deal();
                        Play();
                        Evaluate();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception - {0}", ex.Message);
                    }
                    GameNum++;
                }
            }

            PrintResults();
        }

        public void PrintResults()
        {
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                player.PrintResults();
            }
        }


        public ArrayList players;
        public Dealer dealer;
        Strategy strat;
        Deck deck;

        int GameNum = 0;
    }
}
