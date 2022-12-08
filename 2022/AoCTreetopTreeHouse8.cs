using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode_2021;

namespace AoC._2022
{
    class AoCTreetopTreeHouse8 : AdventOfCodes.AdventOfCode
    {
        Tilemap _trees;
        public override void Run1()
        {
            _trees = new Tilemap(inputFile);
            int visibleCount = 2 * _trees.Width + 2 * (_trees.Height - 2);
            for (int y = 1; y < _trees.Height - 1; y++)
            for (int x = 1; x < _trees.Width - 1; x++)
            {
                if (IsVisibleInDir(x, y, -1, 0) || IsVisibleInDir(x, y, 1, 0)
                 || IsVisibleInDir(x, y, 0, -1) || IsVisibleInDir(x, y, 0, 1))
                {
                    ++visibleCount;
                }
            }
            Console.WriteLine($"Answer is {visibleCount}");
        }
        public bool IsVisibleInDir(int x, int y, int xDir, int yDir)
        {
            int height = _trees.Get(x, y);
            x += xDir; y += yDir;
            int newId = _trees.GetId(x, y);
            while (newId >= 0)
            {
                var nHeight = _trees.Get(x, y);
                if(nHeight >= height) return false;
                x += xDir; y += yDir;
                newId = _trees.GetId(x, y);
            }
            return true;
        }

        public override void Run2()
        {
            int bestScenicScore = 0;
            for (int y = 1; y < _trees.Height - 1; y++)
            for (int x = 1; x < _trees.Width - 1; x++)
            {
                int scenicScore = VisibleInDir(x, y, -1, 0)
                                * VisibleInDir(x, y, 1, 0)
                                * VisibleInDir(x, y, 0, -1)
                                * VisibleInDir(x, y, 0, 1);

                if(scenicScore > bestScenicScore)
                    bestScenicScore = scenicScore;
            }
            Console.WriteLine($"Answer is {bestScenicScore}");
        }

        public int VisibleInDir(int x, int y, int xDir, int yDir)
        {
            int height = _trees.Get(x, y);
            x += xDir; y += yDir;
            int newId = _trees.GetId(x, y);
            int visible = 0;
            while (newId >= 0)
            {
                var nHeight = _trees.Get(x, y);
                if(nHeight >= height) return ++visible;
                ++visible;
                x += xDir; y += yDir;
                newId = _trees.GetId(x, y);
            }
            return visible;
        }
        //363825 too high
    }
}
