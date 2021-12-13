using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace AdventOfCode_2021
{
    class AoC9 : AdventOfCode
    {
        private Tilemap tileMap;

        public override void Start()
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
                int neighborCount = tileMap.Get4Neighbors(i, out var fourNeighbors);
                bool lower = true;
                for (int j = 0; j < neighborCount; j++) {
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
        public override void Run2()
        {
            int[] largestBasins = {int.MinValue, int.MinValue, int.MinValue};
            int minBasin = int.MinValue;
            void StoreLargestBasins(int size)
            {
                if (size <= minBasin) return;
                for (int j = 0; j < largestBasins.Length; j++)
                {
                    if (largestBasins[j] != minBasin) continue;
                    largestBasins[j] = size;
                    minBasin = largestBasins.Min();
                    return;
                }
            }
            
            for (int i = 0; i < _lowPoints.Count; i++)
            {
                int basinSize = tileMap.BFS(_lowPoints[i], height => height < 9);
                StoreLargestBasins(basinSize);
            }

            int result = 1;
            for (int j = 0; j < largestBasins.Length; j++)
                result *= largestBasins[j];
            
            WriteLine($"Basin result is {result}");
        }
    }
}