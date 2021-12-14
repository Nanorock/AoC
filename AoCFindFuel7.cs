using System;
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
            Array.Sort(_crabs);
            var mid = _crabs.Length / 2;
            var minFuel = CalculateFuel(mid, FindFuel2);
            WriteLine($"Min fuel cost pos is {mid} with {minFuel} fuel");
        }

        public int FindFuel1(int linearCost)
        {
            return linearCost;
        }
        static int[] _factorials;
        static int[] Factorials
        {
            get
            {
                if (_factorials == null)
                {
                    _factorials = new int[2000];
                    for (int i = 1; i < _factorials.Length; i++)
                        _factorials[i] = i + _factorials[i - 1];
                }
                return _factorials;
            }
        }
        int FindFuel2(int linearCost)
        {
            return Factorials[linearCost];
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