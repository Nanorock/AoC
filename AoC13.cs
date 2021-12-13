using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_2021
{
    class AoC13 : AdventOfCode
    {
        Tilemap _paper;
        List<(bool horizontal, int coord)> _folds;
        List<(int x, int y)> _coords;
        int _maxX = 0, _maxY = 0;
        public override void Start()
        {
            _coords = new List<(int, int)>();
            void ReadCoords(string input)
            {
                var coordsStr = input.Split(',');
                var x = int.Parse(coordsStr[0]);
                if (x > _maxX) _maxX = x;
                var y = int.Parse(coordsStr[1]);
                if (y > _maxY) _maxY = y;
                _coords.Add((x,y));
            }
            _folds = new List<(bool, int)>();
            void ReadFolds(string input)
            {
                int eqIndex = input.IndexOf('=');
                bool horizontal = input[eqIndex - 1] == 'y';
                int number = int.Parse(input.Substring(eqIndex + 1));
                _folds.Add((horizontal, number));
            }

            bool readFolds = false;
            for (int i = 0; i < inputFile.Length; i++)
            {
                var line = inputFile[i];
                if (string.IsNullOrEmpty(line))
                {
                    readFolds = true;
                    continue;
                }
                if(!readFolds)
                    ReadCoords(inputFile[i]);
                else 
                    ReadFolds(inputFile[i]);
            }

            SetBoard();

            Run();
        }

        void SetBoard()
        {
            _paper = new Tilemap(_maxX + 1, _maxY + 1);
            for (int i = 0; i < _coords.Count; i++)
            {
                var (x, y) = _coords[i];
                int id = _paper.GetId(x, y);
                _paper[id] = 1;
            }
        }
        


        public void Run()
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
            WriteLine(_paper.PrintState(width, height));
            WriteLine($"After fold, dot count:{_paper.Count(x=>x>0)} in {sw.Elapsed.TotalMilliseconds}ms");
        }
    }
}