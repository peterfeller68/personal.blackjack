using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    class Game
    {

        public Game (int NumPlayers, int NumDecks, int dbgLvl =0)
        {
            deck = new Deck(NumDecks);
            dealer = new Dealer(deck, dbgLvl);

            players = new ArrayList();
            for (int x = 0; x < NumPlayers; x++)
            {
                players.Add(new Player(dealer, dbgLvl));
            }

            DebugLevel = dbgLvl;
            for (int x=0; x<1000; x++)
            {
                CountStats[x] = 0;
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
                    player.DealCard(deck.getCard());
                }
                dealer.DealCard(deck.getCard());
            }
        }

        protected void SetDebugLevel(int dbgLevel)
        {
            DebugLevel = dbgLevel; 
            
            for (int x = 0; x < players.Count; x++)
            {
                Player player = (Player)players[x];
                player.DebugLevel = dbgLevel;
            }
            dealer.DebugLevel =dbgLevel;
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

            if (DebugLevel == 2)
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

        public void PlayMultiple(int MaxNum)
        {
            while (GameNum < MaxNum)
            {
                deck.Shuffle();
                while (deck.getRemainingCards() > players.Count*5 && GameNum < MaxNum)
                {
                    CountStats[500 + deck.Count]++;

                    if (deck.Count >= 10)
                    {
                        SetDebugLevel(2);
                    }
                    else
                    {
                        SetDebugLevel(0);
                    }

                    if (DebugLevel==2)
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
                        Console.WriteLine("Exception - {0}", ex.Message);
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

            Console.WriteLine("Count Stats");
            for (int x=0; x<1000; x++)
            {
                if (CountStats[x] > 0)
                {
                    double percCount = (double)CountStats[x] / GameNum * 100;
                    if (percCount > 1)
                    {
                        Console.WriteLine("Count={0} Occ:{1} Perc:{2:0.#}%", x - 500, CountStats[x], percCount);
                    }
                }
            }
            CountStats[500 + deck.Count]++;
        }

        public ArrayList players;
        public Dealer dealer;
        Deck deck;
        public int DebugLevel { get; set; }
        int GameNum = 0;
        int[] CountStats = new int[1000];
    }
}
