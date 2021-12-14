using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_2021
{
    class AoCFolding13 : AdventOfCode
    {
        Tilemap _paper;
        List<(bool horizontal, int coord)> _folds = new List<(bool horizontal, int coord)>();
        List<(int x, int y)> _coords = new List<(int x, int y)>();
        int _maxX = 0, _maxY = 0;
        public override void Init()
        {
            var setup = Stopwatch.StartNew();
            SetBoard();
            setup.Stop();
            WriteLine($"Setup in {setup.Elapsed.TotalMilliseconds}ms");

            Run();
        }

        void SetBoard()
        {
            bool readFolds = false;
            for (int i = 0; i < inputFile.Length; i++)
            {
                var line = inputFile[i];
                if (string.IsNullOrEmpty(line))
                {
                    readFolds = true;
                    continue;
                }
                if (!readFolds)
                    ReadCoords(inputFile[i]);
                else
                    ReadFolds(inputFile[i]);
            }

            _paper = new Tilemap(_maxX + 1, _maxY + 1);
            for (int i = 0; i < _coords.Count; i++)
            {
                var (x, y) = _coords[i];
                _paper.Set(x, y, 1);
            }
        }
        void ReadCoords(string input)
        {
            var coordsStr = input.Split(',');
            var x = int.Parse(coordsStr[0]);
            if (x > _maxX) _maxX = x;
            var y = int.Parse(coordsStr[1]);
            if (y > _maxY) _maxY = y;
            _coords.Add((x, y));
        }
        void ReadFolds(string input)
        {
            int eqIndex = input.IndexOf('=');
            bool horizontal = input[eqIndex - 1] == 'y';
            int number = int.Parse(input.Substring(eqIndex + 1));
            _folds.Add((horizontal, number));
        }

        void Run()
        {
            int width = _paper.Width;
            int height = _paper.Height;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < _folds.Count; i++)
            {
                var fold = _folds[i];
                if (fold.horizontal)
                {
                    _paper.FoldHorizontal(fold.coord, width, height);
                    height = fold.coord;
                }
                else
                {
                    _paper.FoldVertical(fold.coord, width, height);
                    width = fold.coord;
                }
            }
            sw.Stop();

            var swdraw = Stopwatch.StartNew();
            Console.WriteLine(_paper.GetPrinter().PrintState(width, height));
            swdraw.Stop();

            WriteLine($"After fold, dot count:{_paper.Count(x=>x>0)} in {sw.Elapsed.TotalMilliseconds}ms, draw in {swdraw.Elapsed.TotalMilliseconds}ms");
        }
    }
}