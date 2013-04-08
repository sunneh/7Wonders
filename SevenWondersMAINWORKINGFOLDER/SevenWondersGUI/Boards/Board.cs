
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class Board
    {
        private string name;
        protected int maxWonderLevel;
    	protected int currentWonderLevel = 0;
        protected int victoryPoints = 0;
        protected int[,] buildCost;
        protected PlayerState player;
        protected int[] resources;

    	public Board(string n, int mwl, int[,] bc, int[] r){
            name = n;
            maxWonderLevel = mwl;
            buildCost = bc;
            resources = r;
    	}

        public virtual void incrementWonderLevel(PlayerState p) { }

        public string getName() { return name; }

        public virtual int[] getResources(){
            return resources;
        }

        public int getCurrentWonderLevel() { return currentWonderLevel; }

        public int getMaxWonderLevel() { return maxWonderLevel; }

        public int getVictoryPoints() { return victoryPoints; }

        public int[] getBuildCost(){
            int[] toReturn = new int[7];
            for (int j = 0; j < 7; j++){
                toReturn[j] = buildCost[currentWonderLevel, j];
            }
            return toReturn;
        }


        public bool notMaxYet() { return currentWonderLevel < maxWonderLevel; }

    }
}