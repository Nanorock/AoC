using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCodes;

namespace AdventOfCode_2021
{
    class AoCRateAndComsumption3 : AdventOfCode
    {
        int _gammaRate, _epsilonRate;
        int PowerConsumption => _gammaRate * _epsilonRate;

        int _oxygenGeneratorRating;
        int _co2ScrubberRating;

        int LifeSupportRating => _oxygenGeneratorRating * _co2ScrubberRating;

        int _bitCount;

        public override void Run1() { }
        public override void Run2()
        {
            var values = inputFile.Select(str => Convert.ToInt32(str, 2)).ToArray();
            _bitCount = inputFile[0].Length;

            int[] commonBitSet = new int[_bitCount];
            for (int i = 0; i < values.Length; i++)
            {
                var bitValue = values[i];
                for (int bitIndex = 0; bitIndex < _bitCount; ++bitIndex)
                {
                    int bitMask = 1 << bitIndex;
                    bool isSet = (bitValue & bitMask) != 0;
                    commonBitSet[bitIndex] += isSet ? +1 : -1;
                }
            }

            for (int bitIndex = 0; bitIndex < commonBitSet.Length; bitIndex++)
            {
                if (commonBitSet[bitIndex] >= 0)
                    _gammaRate |= 1 << bitIndex;
                else 
                    _epsilonRate |= 1 << bitIndex;
            }
            Console.WriteLine($"GammaRate {_gammaRate}");
            Console.WriteLine($"EpsilonRate {_epsilonRate}");
            Console.WriteLine($"PowerConsumption is {PowerConsumption}");

            Console.WriteLine($"---------- Part 2 ----------------");

            FindRating(values, f => f >= 0, ref _oxygenGeneratorRating);
            FindRating(values, f => f < 0, ref _co2ScrubberRating);

            Console.WriteLine($"OxygenGeneratorRating {_oxygenGeneratorRating}");
            Console.WriteLine($"CO2ScrubberRating {_co2ScrubberRating}");
            Console.WriteLine($"LifeSupportRating is {LifeSupportRating}");
        }

        void FindRating(int[] allValues, Func<float, bool> passCriteria, ref int outputValue)
        {
            List<int> validValues = allValues.ToList();
            int bitIndex = _bitCount - 1;
            while (validValues.Count > 1)
            {
                var bitCriteria = passCriteria(GetOccurence(validValues, bitIndex));
                FilterCriteria(validValues, bitIndex, bitCriteria);
                outputValue = validValues[0];
                --bitIndex;
            }
        }
        
        int GetOccurence(List<int> values, int bitIndex)
        {
            int count = 0;
            int bitMask = 1 << bitIndex;
            for (int i = 0; i < values.Count; i++)
            {
                var value = values[i];
                count += (value & bitMask) != 0 ? +1 : -1;
            }
            return count;
        }
        void FilterCriteria(List<int> outValues, int bitIndex, bool isOn)
        {
            int bitMask = 1 << bitIndex;
            for (int i = outValues.Count - 1; i >= 0; i--)
            {
                var value = outValues[i];
                bool isSet = (value & bitMask) != 0;
                if (isSet != isOn)
                {
                    outValues.RemoveAt(i);
                    if (outValues.Count <= 1) 
                        return;
                }
            }
        }
    }
}