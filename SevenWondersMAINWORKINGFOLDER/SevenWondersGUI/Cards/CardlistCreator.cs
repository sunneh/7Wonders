using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SevenWondersGUI
{
    class cardlistCreator
    {
        private List<Card> Cardlist = new List<Card>();

        public cardlistCreator()
        {
           // Uri uri = new System.Uri(@"pack://application:,,,/Cards/Cards.csv");
            //var path = uri.AbsoluteUri;
            var path = @"../../Cards/Cards2.csv";
            //@"pack://application:,,,/Images/startBackground.png"
            using (var reader = new StreamReader(path))
            {
                var lines = reader.ReadToEnd().Split(new char[] { ';' });
                //for (int tempInteger = 0; tempInteger < lines.Length; tempInteger++) { Console.WriteLine(lines[tempInteger]); }
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) { continue; }
                        
                    var tokens = line.Trim().Split(new char[] { ',' });

                    int[] temprCost = new int[7] { Int32.Parse(tokens[5]), Int32.Parse(tokens[6]), Int32.Parse(tokens[7]), Int32.Parse(tokens[8]), Int32.Parse(tokens[9]), Int32.Parse(tokens[10]), Int32.Parse(tokens[11]) };
                    int[] tempPrebuild = new int[4] { Int32.Parse(tokens[13]), Int32.Parse(tokens[14]), Int32.Parse(tokens[15]), Int32.Parse(tokens[16]) };

                    switch (Int32.Parse(tokens[4]))
                    {
                        case 1:
                            //Console.WriteLine("CASE 1 Resource Card");
                            int[] tempRProduce = { Int32.Parse(tokens[17]), Int32.Parse(tokens[18]), Int32.Parse(tokens[19]), Int32.Parse(tokens[20]), Int32.Parse(tokens[21]), Int32.Parse(tokens[22]), Int32.Parse(tokens[23]) };
                            Cardlist.Add(new ResourceCard(tokens[36], Int32.Parse(tokens[0]), Int32.Parse(tokens[3]), Int32.Parse(tokens[2]), Int32.Parse(tokens[4]), temprCost, Int32.Parse(tokens[12]), tokens[1], tempPrebuild, tempRProduce));
                            goto finish;
                        case 2:
                           // Console.WriteLine("CASE 2 Manufactured Goods Card");
                            int[] tempMG = { Int32.Parse(tokens[17]), Int32.Parse(tokens[18]), Int32.Parse(tokens[19]), Int32.Parse(tokens[20]), Int32.Parse(tokens[21]), Int32.Parse(tokens[22]), Int32.Parse(tokens[23]) };
                            Cardlist.Add(new ResourceCard(tokens[36], Int32.Parse(tokens[0]), Int32.Parse(tokens[3]), Int32.Parse(tokens[2]), Int32.Parse(tokens[4]), temprCost, Int32.Parse(tokens[12]), tokens[1], tempPrebuild, tempMG));
                            goto finish;
                        case 3:
                            //Console.WriteLine("CASE 3 Civilian Card");
                            Cardlist.Add(new CivilianCard(tokens[36], Int32.Parse(tokens[0]), Int32.Parse(tokens[3]), Int32.Parse(tokens[2]), Int32.Parse(tokens[4]), temprCost, Int32.Parse(tokens[12]), tokens[1], tempPrebuild, Int32.Parse(tokens[24])));
                            goto finish;
                        case 4:
                           // Console.WriteLine("CASE 4 Commerce Card");
                            int[] resourceT = new int[2] { Int32.Parse(tokens[25]), Int32.Parse(tokens[26]) };
                            int[] col = new int[4] { Int32.Parse(tokens[29]), Int32.Parse(tokens[30]), Int32.Parse(tokens[31]), Int32.Parse(tokens[32]) };
                            Cardlist.Add(new CommerceCard(tokens[36], Int32.Parse(tokens[0]), Int32.Parse(tokens[3]), Int32.Parse(tokens[2]), Int32.Parse(tokens[4]), temprCost, Int32.Parse(tokens[12]), tokens[1], tempPrebuild, resourceT, Int32.Parse(tokens[27]), Int32.Parse(tokens[28]), col, Int32.Parse(tokens[33])));
                            goto finish;
                        case 5:
                           // Console.WriteLine("CASE 5 Military Card");
                            Cardlist.Add(new MilitaryCard(tokens[36], Int32.Parse(tokens[0]), Int32.Parse(tokens[3]), Int32.Parse(tokens[2]), Int32.Parse(tokens[4]), temprCost, Int32.Parse(tokens[12]), tokens[1], tempPrebuild, Int32.Parse(tokens[34])));
                            goto finish;
                        case 6:
                           // Console.WriteLine("CASE 6 Science Card");
                            Cardlist.Add(new ScienceCard(tokens[36], Int32.Parse(tokens[0]), Int32.Parse(tokens[3]), Int32.Parse(tokens[2]), Int32.Parse(tokens[4]), temprCost, Int32.Parse(tokens[12]), tokens[1], tempPrebuild, Int32.Parse(tokens[35])));
                            goto finish;
                        case 7:
                            //Console.WriteLine("CASE 7 Guild Card");
                            Cardlist.Add(new GuildCard(tokens[36], Int32.Parse(tokens[0]), Int32.Parse(tokens[3]), Int32.Parse(tokens[2]), Int32.Parse(tokens[4]), temprCost, Int32.Parse(tokens[12]), tokens[1], tempPrebuild));
                            goto finish;
                    }finish:;
                }
            } Console.ReadLine();
        }

        public List<Card> getCardList(){return Cardlist;}

        
    }
}