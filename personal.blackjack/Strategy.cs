using System;
using System.Collections.Generic;
using System.Text;

namespace personal.blackjack
{
    class Strategy
    {
        public Strategy()
        {
            PlayerHitLimit = new int[11];

            // global game settings
            NumDecks = 5;
            NumPlayers = 1;
            NumGames = 100000;
            DebugLevel = 0;
            BettingUnit = 10; 

            // Dealer
            DealerHitLimit = 17;

            // Player
            PlayerMinBet = 1;
            PlayerMaxBet = 100;

            // hit until the value indicated based on the dealercard
            PlayerHitLimit[1] = 18;  /* Ace */
            PlayerHitLimit[2] = 13;  /* 2 */
            PlayerHitLimit[3] = 13;   /* 3 */
            PlayerHitLimit[4] = 13;   /* 4 */
            PlayerHitLimit[5] = 13;  /* 5 */
            PlayerHitLimit[6] = 13;  /* 6 */
            PlayerHitLimit[7] = 17;  /* 7 */
            PlayerHitLimit[8] = 17;  /* 8 */
            PlayerHitLimit[9] = 17;  /* 9 */
            PlayerHitLimit[10] = 17; /* 10 */

            // Output Options
            opVisibleDealerCard = -1;
            opCountStats = true;
            opVisDealerCardStats = true;
        }

        // Global Settings
        public int NumDecks { get; set; }
        public int NumPlayers { get; set; }
        public int NumGames { get; set; }
        public int DebugLevel { get; set; }
        public int BettingUnit { get; set; }


        // Dealer Strategy
        public int DealerHitLimit { get; set; }

        // Player Strategy
        public int PlayerMinBet { get; set; }
        public int PlayerMaxBet { get; set; }
        // set the player target based on the visible dealer card
        public int[] PlayerHitLimit { get; set; }

        // Output Options
        public int opVisibleDealerCard { get; set; }
        public bool opCountStats { get; set; }
        public bool opVisDealerCardStats { get; set; }
    }
}
