using AdventOfCode_2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC._2022
{
    class AoCRockPaperScissors2 : AdventOfCodes.AdventOfCode
    {
        public override void Run1()
        {
            base.Run1();

            int predictedScore = 0;
            for (int i = 0; i < inputFile.Length; i++)
            {
                var opponent = inputFile[i][0] - 'A' + 1;
                var suggestion = inputFile[i][2] - 'X' + 1;
                predictedScore+=Score(suggestion, opponent);
            }
            Console.WriteLine($"Answer is {predictedScore}");
        }

        const int SCORE_LOSE = 0;
        const int SCORE_DRAW = 3;
        const int SCORE_WIN = 6;        

        const int LOSE = 0;
        const int DRAW = 1;
        const int WIN = 2;

        int Score(int mine, int opponent)
        {
            if(mine == opponent)
                return mine + SCORE_DRAW;
            return mine + (LoseMove(mine) == opponent ? SCORE_WIN : SCORE_LOSE);
        }

        int WinMove(int opponent) => (3 + (opponent-1) + 1) % 3 + 1;
        int LoseMove(int opponent) => (3 + (opponent-1) - 1) % 3 + 1;

        public override void Run2()
        {
            base.Run2();

            int predictedScore = 0;
            for (int i = 0; i < inputFile.Length; i++)
            { 
                var opponent = inputFile[i][0] - 'A' + 1;
                var desiredResult = inputFile[i][2] - 'X';
                if(desiredResult == DRAW)
                    predictedScore+=Score(opponent,opponent);
                else if(desiredResult == WIN)
                {
                    predictedScore+=Score(WinMove(opponent),opponent);
                }
                else
                    predictedScore+=Score(LoseMove(opponent),opponent);
            }
            Console.WriteLine($"Answer is {predictedScore}");
        }
    }
}
