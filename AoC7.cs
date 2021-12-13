using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2021
{
    class AoCFindFuel7 : AdventOfCode
    {
        int[] _crabs;
        int _minPos = int.MaxValue, _maxPos = int.MinValue;
        public override void Run1() { }
        public override void Run2()
        {
            _crabs = inputFile[0].Split(',').Select(int.Parse).ToArray();
            
            for (int i = 0; i < _crabs.Length; i++)
            {
                var crabPos = _crabs[i];
                if (crabPos < _minPos) _minPos = crabPos;
                if (crabPos > _maxPos) _maxPos = crabPos;
            }
            
            int minFuel = int.MaxValue;
            int bestPos = -1;
            for (int i = _minPos; i < _maxPos; i++)
            {
                var fuelCost = CalculateFuel(i, FindFuel2);
                if (fuelCost < minFuel)
                {
                    minFuel = fuelCost;
                    bestPos = i;
                }
            }
            WriteLine($"Min fuel cost pos is {bestPos} with {minFuel} fuel");
            Wait();
        }

        public int FindFuel1(int linearCost)
        {
            return linearCost;
        }
        public int FindFuel2(int linearCost)
        {
            int factorialCost = 0;
            for (int i = linearCost; i > 0; i--)
                factorialCost += i;
            return factorialCost;
        }


        int CalculateFuel(int pos, Func<int,int> fuelCost)
        {
            int fuel = 0;
            for (int i = 0; i < _crabs.Length; i++)
            {
                fuel += fuelCost(Math.Abs(_crabs[i] - pos));
            }
            return fuel;
        }

    }
}