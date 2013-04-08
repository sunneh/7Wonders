using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class CreateBoards
    {

        List<Board> boards = new List<Board>();

        public CreateBoards()
        {
            boards.Add(new WB1());
            boards.Add(new WB2());
            boards.Add(new WB3());
            boards.Add(new WB4());
            boards.Add(new WB5());
            boards.Add(new WB6());
            boards.Add(new WB13());
            boards.Add(new WB14());
            boards.Add(new WB11());
            boards.Add(new WB12());
            boards.Add(new WB9());
            boards.Add(new WB10());
            boards.Add(new WB7());
            boards.Add(new WB8());
        }

        public List<Board> getBoards(){  return boards; }



    }
}
