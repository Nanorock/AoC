using System;
using System.Linq;

namespace AdventOfCode_2021
{
    class AoCFishes6 : AdventOfCode
    {
        public override void Run1() { }
        public override void Run2()
        {
            SetupSchools();
            int reproductionScope = 256;
            RunReproduction(reproductionScope);
            WriteLine($"Fishes after {reproductionScope} days : {CountFishes()}");
        }

        const int DefaultNewReproductionTimer = 8;
        const int ResetReproductionTimer = 6;
        
        ulong[] _schools;
        void SetupSchools()
        {
            _schools = new ulong[DefaultNewReproductionTimer+1];
            var fishes = inputFile[0].Split(',').Select(int.Parse).ToList();
            for (int i = 0; i < fishes.Count; i++)
                ++_schools[fishes[i]];
        }
        void RunReproduction(int scope)
        {
            for (int i = 1; i < scope; i++)
            {
                ulong reproductionDay = _schools[0];
                for (int timer = 1; timer < _schools.Length; ++timer)
                    _schools[timer - 1] = _schools[timer];
                _schools[DefaultNewReproductionTimer] = reproductionDay;
                _schools[ResetReproductionTimer] += reproductionDay;
            }
        }
        ulong CountFishes()
        {
            ulong fishCount = _schools[0]; //babies count double
            for (int j = 0; j < _schools.Length; j++)
                fishCount += _schools[j];
            return fishCount;
        }
    }
}