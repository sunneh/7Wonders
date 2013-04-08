using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class Calculator
    {        
        public static readonly int _MILITARY_SCORE = 0;
        public static readonly int _TREASURY_SCORE = 1;
        public static readonly int _WONDER_SCORE = 2;
        public static readonly int _CIVILIAN_SCORE = 3;
        public static readonly int _COMMERCE_SCORE = 4;
        public static readonly int _GUILD_SCORE = 5; 
        public static readonly int _SCIENCE_SCORE = 6; 

        private List<int> scores;
        private bool scienceguildCard = false;
        private static Calculator _instance = null;

        private Calculator()
        {
        }

        public static Calculator getInstance()
        {
            if (_instance == null)
                _instance = new Calculator();
            return _instance;
        }

        public List<int> getScores(PlayerState p, GameState g)
        {
            /*
            SetDefaults();
            scores.Insert(_MILITARY_SCORE,MilitaryScore(p));
            scores.Insert(_TREASURY_SCORE, TreasuryScore(p));
            scores.Insert(_WONDER_SCORE, WonderScore(p));
            scores.Insert(_CIVILIAN_SCORE, CivilianScore(p));
            scores.Insert(_SCIENCE_SCORE, ScienceScore(p));
            scores.Insert(_COMMERCE_SCORE, CommerceScore(p));
            scores.Insert(_GUILD_SCORE, GuildScore(p));
            */
            scores = new List<int>();
            scores.Add(MilitaryScore(p));
            scores.Add(TreasuryScore(p));
            scores.Add(WonderScore(p));
            scores.Add(CivilianScore(p));
            scores.Add(CommerceScore(p));
            scores.Add(GuildScore(p, g));
            scores.Add(ScienceScore(p));
            return scores;
        }

        private int MilitaryScore(PlayerState p) 
        { 
            return p.getMilitaryPoints(); 
        }
        
        private int TreasuryScore(PlayerState p) 
        { 
            return (p.getCoins() / 3); 
        }
        
        private int WonderScore(PlayerState p) 
        { 
            return p.getBoard().getVictoryPoints(); 
        }

        private int CivilianScore(PlayerState p)
        {
            List<CivilianCard> civlist;
            //Civilian Structures
            civlist = new List<CivilianCard>();
            int score = 0;
            //Cycle through played cards
            //Add all civilian cards to list
            for (int i = 0; i < p.getPlayedCards().Count; i++) {
                if (p.getPlayedCards()[i].getType() == Card._CIVILIAN) 
                { 
                    civlist.Add((CivilianCard)p.getPlayedCards()[i]); 
                }
            }
            //Cycle through civ card list to add points
            for (int i = 0; i < civlist.Count; i++) {
                score = score + civlist[i].getVictoryPoints();
            }
            return score;
        }

        private int ScienceScore(PlayerState p)
        {
            //System.Console.WriteLine("sciencescore start");
            List<ScienceCard> scilist = new List<ScienceCard>();
            //Cycle through played cards
            for (int i = 0; i < p.getPlayedCards().Count; i++) {
                if (p.getPlayedCards()[i].getType() == Card._SCIENCE) 
                { 
                    scilist.Add((ScienceCard)p.getPlayedCards()[i]);
                }
            }
            //Cycle through science card list to add up sciencetype totals
            int[] sciTotals = new int[3] { 0, 0, 0 };
            for (int i = 0; i < scilist.Count; i++) 
            {
                //System.Console.WriteLine(scilist[i].getSciType());
                sciTotals[scilist[i].getSciType()]++; 
            }
                  
            int score = 0;
            if (scienceguildCard == true)
            {
                if (sciTotals.Max() > 5) { sciTotals[Array.IndexOf(sciTotals, sciTotals.Max())]++; }
                else { sciTotals[Array.IndexOf(sciTotals, sciTotals.Min())]++; }
            }


            //Set points
            score = score + (sciTotals.Min() * 7);
            //System.Console.WriteLine("Min Score" + sciTotals.Min());
            //Points for types
            //System.Console.WriteLine("Type 0" + sciTotals[0]);
            score = score + (sciTotals[0] * sciTotals[0]);
            //System.Console.WriteLine("Type 1" + sciTotals[1]);
            score = score + (sciTotals[1] * sciTotals[1]);
            //System.Console.WriteLine("Type 2" + sciTotals[2]);
            score = score + (sciTotals[2] * sciTotals[2]);
            System.Console.WriteLine(score);
            
            return score;
        }

        private int CommerceScore(PlayerState p) {
            int score = 0;

            List<CommerceCard> comlist;
            
            //Commercial Structure
            comlist = new List<CommerceCard>();

            //Cycle through played cards
            //Add all Commerce cards to list
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                if (( p.getPlayedCards()[i].getType() == Card._MERCHANT) 
                &&  ( p.getPlayedCards()[i].getAct() == Card._CIVILIAN))
                {
                    comlist.Add((CommerceCard)p.getPlayedCards()[i]);
                }
            }

            for (int i = 0; i < comlist.Count; i++)
            {
                //Victorypoints per wonder
                if (comlist[i].getPerWonder() == 2) 
                { 
                    score = score + p.getBoard().getCurrentWonderLevel(); 
                }

                //Victorypoint per card
                if((comlist[i].getCollect()[2] > 0))
                {
                    //crawl through all played cards of player
                    for (int j = 0; j < p.getPlayedCards().Count; j++)
                    {
                        //if card type matches victorypoint condition of commercial card
                        if (p.getPlayedCards()[j].getType() == comlist[i].getCollect()[0])
                        {
                            //add points appropriate to commercial card parameters
                            score = score + comlist[i].getCollect()[2];
                            System.Console.WriteLine(comlist[i].getCollect()[2]);
                        }
                    }
                }
            }
            return score;   
        }
        
        private int GuildScore(PlayerState p, GameState g)
        {
            int score = 0;
            List<GuildCard> glist = new List<GuildCard>();
            //GameState g = ;
            //Cycle through played cards
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                if (p.getPlayedCards()[i].getType() == Card._GUILD)
                {
                    glist.Add((GuildCard)p.getPlayedCards()[i]);
                }
            }
            //Cycle through guild cards
            for (int i = 0; i < glist.Count; i++)
            {
                //worker
                if (glist[i].getNumber() == 98) { score = score + (2 * (g.getLeftPlayer(p).countCardType(2) + g.getRightPlayer(p).countCardType(2)));}
                //craft
                if (glist[i].getNumber() == 99) { score = score + (g.getLeftPlayer(p).countCardType(1) + g.getRightPlayer(p).countCardType(1)); }
                //trader
                if (glist[i].getNumber() == 100) { score = score + (g.getLeftPlayer(p).countCardType(4) + g.getRightPlayer(p).countCardType(4)); }
                //Philospher
                if (glist[i].getNumber() == 101) { score = score + (g.getLeftPlayer(p).countCardType(6) + g.getRightPlayer(p).countCardType(6)); }
                //Spies
                if (glist[i].getNumber() == 102) { score = score + (g.getLeftPlayer(p).countCardType(5) + g.getRightPlayer(p).countCardType(5)); }
                //strat
                if (glist[i].getNumber() == 103) { score = score + (g.getLeftPlayer(p).getNumLosses()) + (g.getRightPlayer(p).getNumLosses());}
                //ship
                if (glist[i].getNumber() == 104) { score = score + (p.countCardType(1)) + (p.countCardType(2)) + (p.countCardType(7));}
                //Science
                if (glist[i].getNumber() == 105) { scienceguildCard = true; }
                //magistrat
                if (glist[i].getNumber() == 106) { score = score + (g.getLeftPlayer(p).countCardType(3) + g.getRightPlayer(p).countCardType(3)); }
                //builders
                if (glist[i].getNumber() == 107) {
                    score = score + (g.getRightPlayer(p).getBoard().getCurrentWonderLevel());
                    score = score + (g.getLeftPlayer(p).getBoard().getCurrentWonderLevel());
                    score = score + (p.getBoard().getCurrentWonderLevel());
                }
            }
            return score;
        }

        /* Old unused function
        private void SetDefaults()
        {
            scores = new List<int>(7);
            for (int i = 0; i < 7; i++)
                scores.Add(0);
        }*/

    }
}
