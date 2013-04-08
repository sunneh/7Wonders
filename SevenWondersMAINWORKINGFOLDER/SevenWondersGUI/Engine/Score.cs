using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class Score
    {
        private int[,] finalScore;

        public Score(GameState g)
        {
            finalScore = new int[g.getPlayers().Count,7];
            for(int i = 0; i < g.getPlayers().Count; i++){
                if (g.players[i].getPlayedCards().Count < 10) {
                    g.players[i].setScore(0, 0);
                    g.players[i].setScore(0, 1);
                    g.players[i].setScore(0, 2);
                    g.players[i].setScore(0, 3);
                    g.players[i].setScore(0, 4);
                    g.players[i].setScore(0, 5);
                    g.players[i].setScore(0, 6);
                }
                else
                {
                    g.players[i].setScore(militaryScore(g.getPlayerNum(i)), 0);
                    g.players[i].setScore(treasuryScore(g.getPlayerNum(i)), 1);
                    g.players[i].setScore(WonderScore(g.getPlayerNum(i)), 2);
                    g.players[i].setScore(civScore(g.getPlayerNum(i)), 3);
                    g.players[i].setScore(sciScore(g.getPlayerNum(i)), 4);
                    g.players[i].setScore(comScore(g.getPlayerNum(i)), 5);
                    g.players[i].setScore(0, 6);
                    //g.players[i].setScore(,7);
                }
            }
        }
        private int militaryScore(PlayerState p) { return p.getMilitaryPoints(); }
        
        private int treasuryScore(PlayerState p) { return (p.getCoins() / 3); }
        
        private int WonderScore(PlayerState p) { return p.getBoard().getVictoryPoints(); }

        private int civScore(PlayerState p)
        {
            List<CivilianCard> civlist;
            //Civilian Structures
            civlist = new List<CivilianCard>();
            int score = 0;
            //Cycle through played cards
            //Add all civilian cards to list
            for (int i = 0; i < p.getPlayedCards().Count; i++) {
                if (p.getPlayedCards()[i].getType() == 3) { civlist.Add((CivilianCard)p.getPlayedCards()[i]); }
            }
            //Cycle through civ card list to add points
            for (int i = 0; i < civlist.Count; i++) {
                score = score + civlist[i].getVictoryPoints();
            }
            return score;
        }

        private int sciScore(PlayerState p)
        {
            List<ScienceCard> scilist;
            scilist = new List<ScienceCard>();
            int score = 0;
            //Cycle through played cards
            for (int i = 0; i < p.getPlayedCards().Count; i++) {
                if (p.getPlayedCards()[i].getType() == 3) { scilist.Add((ScienceCard)p.getPlayedCards()[i]); }
            }
            //Cycle through science card list to add up sciencetype totals
            int[] sciTotals = new int[3] { 0, 0, 0 };
            for (int i = 0; i < scilist.Count; i++) { sciTotals[scilist[i].getSciType()]++; }

            //Set points
            score = score + (sciTotals.Min() * 7);
            //Points for types
            score = score + (sciTotals[0] * sciTotals[0]);
            score = score + (sciTotals[1] * sciTotals[1]);
            score = score + (sciTotals[2] * sciTotals[2]);

            return score;
        }

        private int comScore(PlayerState p) {
            int score = 0;

            List<CommerceCard> comlist;
            
            //Commercial Structure
            comlist = new List<CommerceCard>();

            //Cycle through played cards
            //Add all Commerce cards to list
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                if ((p.getPlayedCards()[i].getType() == 4) && (p.getPlayedCards()[i].getAct() == 3))
                {
                    comlist.Add((CommerceCard)p.getPlayedCards()[i]);
                }
            }
            for (int i = 0; i < comlist.Count; i++){
                //Victorypoints per wonder
                if (comlist[i].getPerWonder() == 2) { score = score + p.getBoard().getCurrentWonderLevel(); }
                //Victorypoint per card
                if((comlist[i].getCollect()[2] > 0)){
                    //crawl through all played cards of player
                    for (int j = 0; j < p.getPlayedCards().Count; j++)
                    {
                        //if card type matches victorypoint condition of commercial card
                        if (p.getPlayedCards()[j].getType() == comlist[i].getCollect()[0])
                        {
                            //add points appropriate to commercial card parameters
                            score = score + comlist[i].getCollect()[2];
                        }
                    }
                }
            }
            return score;

            //Guilds

        }
    }
}
