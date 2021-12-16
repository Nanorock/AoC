using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2021
{
    class AoCPathfind15 : AdventOfCode
    {
        Tilemap _board;
        public override void Init()
        {
            _board = new Tilemap(inputFile);
        }

        public override void Run1()
        {
            var aStartPath = new List<int>();
            _board.AStar(0, _board.Size - 1, aStartPath);
            aStartPath.RemoveAt(0);
            int sumCost = 0;
            for (int i = 0; i < aStartPath.Count; i++)
                sumCost += _board[aStartPath[i]];

            /*var printer = _board.GetPrinter();
            foreach (var line in printer.PrintStateLines((id, v) =>
                aStartPath.Contains(id) ? $"<Green>{v}" :
                _board.Visited.Contains(id) ? $"<Red>{v}" : $"<White>{v}"))
                ColorConsole.PrintLine(line);*/

            WriteLine($"Path found at cost {sumCost}");

        }

        
        public override void Run2()
        {
            const int boardMultiply = 5;
            Tilemap board2 = new Tilemap(_board.Width * boardMultiply, _board.Height * boardMultiply);
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    var v = _board.Get(x, y);
                    for (int i = 0; i < boardMultiply; i++)
                    {
                        for (int j = 0; j < boardMultiply; j++)
                        {
                            int x2 = x + _board.Width * i;
                            int y2 = y + _board.Height * j;
                            var newValue = v + i + j;
                            newValue -= 1;
                            newValue %= 9;
                            newValue += 1;

                            board2.Set(x2, y2, newValue);
                        }
                    }
                }
            }
            
            var aStartPath = new List<int>();
            board2.AStar( 0, board2.Size - 1, aStartPath);
            aStartPath.RemoveAt(0);
            int sumCost = 0;
            for (int i = 0; i < aStartPath.Count; i++)
                sumCost += board2[aStartPath[i]];

            /*var printer = board2.GetPrinter();
            int lineY = -1;
            string hLine = "   ";
            string fillLine = "---";
            for (int i = 0; i < board2.Width; i++)
            {
                hLine += i.ToString("D2")+"|";
                fillLine += "---";
            }
            Console.WriteLine(hLine);
            Console.WriteLine(fillLine);
            foreach (var line in printer.PrintStateLines((id, v) =>
                aStartPath.Contains(id) ? $"<Green>{v:D2}<White>|" :
                board2.Visited.Contains(id) ? $"<Red>{v:D2}<White>|" : $"<White>{v:D2}<White>|"))
            {
                ColorConsole.PrintLine((++lineY).ToString("D2")+"|"+ line);
                Console.WriteLine(fillLine);
            }*/

            WriteLine($"Path found at cost {sumCost}");
        }
    }
}