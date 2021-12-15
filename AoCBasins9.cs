using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace AdventOfCode_2021
{
    class AoCBasins9 : AdventOfCode
    {
        private Tilemap tileMap;

        public override void Init()
        {
            tileMap = new Tilemap(inputFile);
        }

        readonly List<int> _lowPoints = new List<int>();
        public override void Run1()
        {
            int result = 0;
            for (int i = 0; i < tileMap.Size; i++)
            {
                int local = tileMap[i];
                using var fourNeighbors = tileMap.Get4Neighbors(i);
                bool lower = true;
                for (int j = 0; j < fourNeighbors.Length; j++) {
                    if (tileMap[fourNeighbors[j]] <= local) {
                        lower = false;
                        break;
                    }
                }
                if (lower)
                {
                    result += local + 1;
                    _lowPoints.Add(i);
                }
            }
            WriteLine($"Low point result is {result}");
        }

        int[] _largestBasins = { int.MinValue, int.MinValue, int.MinValue };
        int _minBasin = int.MinValue;
        void StoreLargestBasins(int size)
        {
            if (size <= _minBasin) return;
            for (int j = 0; j < _largestBasins.Length; j++)
            {
                if (_largestBasins[j] != _minBasin) continue;
                _largestBasins[j] = size;
                _minBasin = _largestBasins.Min();
                return;
            }
        }

        public override void Run2()
        {
            for (int i = 0; i < _lowPoints.Count; i++)
            {
                int basinSize = tileMap.BFS_4(_lowPoints[i], (id,height) => height < 9);
                StoreLargestBasins(basinSize);
            }

            int result = 1;
            for (int j = 0; j < _largestBasins.Length; j++)
                result *= _largestBasins[j];
            
            WriteLine($"Basin result is {result}");
        }
    }
}