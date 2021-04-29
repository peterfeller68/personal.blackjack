using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    class Statistics
    {
        public enum Result { Unk, Push, Win, Loss };
        public StatisticsData total { get; set; }
        SortedDictionary<int, StatisticsData> countStats = new SortedDictionary<int, StatisticsData>();
        SortedDictionary<int, StatisticsData> cardValStats = new SortedDictionary<int, StatisticsData>();

        public Statistics(Strategy s)
        {
            strat = s;
            total = new StatisticsData(s);
        }

        public void PrintResults()
        {
            if (strat.opVisibleDealerCard == -1)
            {
                Console.WriteLine("Overall Stats");
                Console.WriteLine("-------------");
                total.PrintResults();
                Console.WriteLine();
            }

            if (strat.opCountStats)
            {
                Console.WriteLine("Count Stats");
                Console.WriteLine("-----------");
                foreach (int count in countStats.Keys)
                {
                    StatisticsData stats = countStats[count];

                    Console.Write("Count = {0} || ", count);
                    stats.PrintResults(true);
                }
                Console.WriteLine();
            }

            if (strat.opVisDealerCardStats)
            {
                if (strat.opVisibleDealerCard == -1)
                {
                    Console.WriteLine("Dealer Card Stats");
                    Console.WriteLine("-----------------");
                    foreach (int count in cardValStats.Keys)
                    {
                        StatisticsData stats = cardValStats[count];

                        Console.Write("CardVal = {0} || ", count);
                        stats.PrintResults(true);
                    }
                    Console.WriteLine();
                }
                else
                {
                    StatisticsData stats = cardValStats[strat.opVisibleDealerCard];
                    Console.Write("CardVal = {0} || ", strat.opVisibleDealerCard);
                    stats.PrintResults(true);
                }
            }
        }

        public void Update(GameData res
            , int DeckCount
            , int DealerVisCardVal
            , bool playerDoubleDown
            , bool DealerBlackJack
            , bool HasSplit
            , double gameWager)
        {
            double winAmount = 0;

            if (!countStats.ContainsKey(DeckCount))
            {
                countStats.Add(DeckCount, new StatisticsData(strat));
            }
            StatisticsData countstats = countStats[DeckCount];

            if (!cardValStats.ContainsKey(DealerVisCardVal))
            {
                cardValStats.Add(DealerVisCardVal, new StatisticsData(strat));
            }
            StatisticsData dealercardstats = cardValStats[DealerVisCardVal];

            if (playerDoubleDown)
            {
                total.DoubleDown++;
                countstats.DoubleDown++;
                dealercardstats.DoubleDown++;
            }

            if (HasSplit)
            {
                total.Split++;
                countstats.Split++;
                dealercardstats.Split++;
            }

            if (res.blackJack)
            {
                total.BlackJack++;
                countstats.BlackJack++;
                dealercardstats.BlackJack++;
            }

            if (DealerBlackJack)
            {
                total.DealerBlackJack++;
                countstats.DealerBlackJack++;
                dealercardstats.DealerBlackJack++;
            }
            //////////////////////////////////
            ///Update the overall stats
            //////////////////////////////////
            if (res.gameResult == Statistics.Result.Loss)
            {
                total.Lost++;
                total.TotDeckCountAtLoss += DeckCount;

                countstats.Lost++;
                countstats.TotDeckCountAtLoss += DeckCount;

                dealercardstats.Lost++;
                dealercardstats.TotDeckCountAtLoss += DeckCount;

                winAmount -= gameWager;
            }
            if (res.gameResult == Statistics.Result.Win)
            {
                total.Win++;
                total.TotDeckCountAtWin += DeckCount;

                countstats.Win++;
                countstats.TotDeckCountAtWin += DeckCount;

                dealercardstats.Win++;
                dealercardstats.TotDeckCountAtWin += DeckCount;

                if (res.blackJack)
                {
                    winAmount += gameWager*1.5;
                }
                else
                {
                    winAmount += gameWager;
                }

            }
            if (res.gameResult == Statistics.Result.Push)
            {
                total.Push++;
                countstats.Push++;
                dealercardstats.Push++;
            }

            total.Winnings += winAmount;
            countstats.Winnings += gameWager;
            dealercardstats.Winnings += gameWager;

            total.Purse += winAmount;
            countstats.Purse += gameWager;
            dealercardstats.Purse += gameWager;

            total.NumGames++;
            countstats.NumGames++;
            dealercardstats.NumGames++;
        }

        protected Strategy strat;
    }

    class StatisticsData
    {
        public StatisticsData(Strategy s)
        {
            strat = s;
            Init();
        }

        public void Init()
        {
            NumGames = 0;
            TotDeckCountAtWin = 0;
            TotDeckCountAtLoss = 0;
            BlackJack = 0;
            DealerBlackJack = 0;
            Win = 0;
            Push = 0;
            Lost = 0;
            Split = 0;
            Winnings = 0;
            Purse = strat.PlayerPurse;
        }
        public void PrintResults(bool bShort = false)
        {
            string Adv = "    ";
            double PushPerc = (double)Push / NumGames * 100;
            double WinPerc = (double)Win / NumGames * 100;
            double LossPerc = (double)Lost / NumGames * 100;
            double DoubleDownPerc = (double)DoubleDown / NumGames * 100;
            double NetWinPerc = WinPerc - LossPerc; // 
            if (WinPerc > LossPerc) Adv = "YES ";
            double BJPerc = (double)BlackJack / NumGames * 100;
            double SplitPerc = (double)Split / NumGames * 100;
            if (bShort)
            {
                Console.Write("Games: {0} {6}- W:{1:0.#}%, L:{2:0.#}%, P:{3:0.#}%, BJ:{4:0.#}%, DD:{8:0.#}%, Sp:{5:0.#}%, NWP:{7:0.#}%", NumGames, WinPerc, LossPerc, PushPerc, BJPerc, SplitPerc, Adv, NetWinPerc, DoubleDownPerc);
                if (strat.opVisibleDealerCard != -1)
                {
                    Console.Write("- hit until {0}", strat.PlayerHitLimit[strat.opVisibleDealerCard]);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Games Played: {0}", NumGames);
                Console.WriteLine("BLACKJACK:    {0} [{1:0.#}%]", BlackJack, BJPerc);
                Console.WriteLine("Games Pushed: {0} [{1:0.#}%]", Push, PushPerc);
                Console.WriteLine("Games Won:    {0} [{1:0.#}%]", Win, WinPerc);
                Console.WriteLine("Games Lost:   {0} [{1:0.#}%]", Lost, LossPerc);
                Console.WriteLine("Net Win Perc: [{0:0.#}%]", NetWinPerc);
                Console.WriteLine("Hands Split:  {0} [{1:0.#}%]", Split, SplitPerc);
                Console.WriteLine("Double Down:  {0} [{1:0.#}%]", DoubleDown, DoubleDownPerc);
                Console.WriteLine("Dealer Blackjack:   {0} [{1:0.#}%]", DealerBlackJack, (double)DealerBlackJack / NumGames * 100);
                Console.WriteLine("Avg Deck Count at Win: [{0:0.##}]", (double)TotDeckCountAtWin / Win);
                Console.WriteLine("Avg Deck Count at Loss: [{0:0.##}]", (double)TotDeckCountAtLoss / Lost);
            }
        }


        public int NumGames { get; set; }
        public int TotDeckCountAtWin { get; set; }
        public int TotDeckCountAtLoss { get; set; }
        public int BlackJack { get; set; }
        public int Split { get; set; }
        public int DealerBlackJack { get; set; }
        public int Win { get; set; }
        public int Push { get; set; }
        public int Lost { get; set; }
        public int DoubleDown { get; set; }
        public double Winnings { get; set; }
        public double Purse { get; set; }

        protected Strategy strat;
    }

    class GameData
    {
        public GameData()
        {
            gameResult = Statistics.Result.Unk;
            blackJack = false;
        }

        public Statistics.Result gameResult { get; set; }
        public bool blackJack { get; set; }
    };
}

