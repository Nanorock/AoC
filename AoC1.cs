using System;
using System.Linq;

namespace AdventOfCode_2021
{
    class AoC1 : AdventOfCode
    {
        public override void Run1() { }
        public override void Run2()
        {
            var inputData = inputFile.Select(float.Parse).ToList();

            int slidingWindow = 3;

            float GetWindowSum(int windowStart)
            {
                float sum = 0;
                int windowLength = Math.Min(windowStart + slidingWindow, inputData.Count);
                for (int i = windowStart; i < windowLength; i++)
                    sum += inputData[i];
                return sum;
            }

            int inc = 0;
            for (int i = 1; i < inputData.Count; i++)
                if (GetWindowSum(i) > GetWindowSum(i - 1))
                    ++inc;

            Console.WriteLine($"Answer is {inc}");
            Console.ReadKey();
        }

        
    }
}