using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC._2022
{
    class AoCRucksackReorganization3 : AdventOfCodes.AdventOfCode
    {
        public override void Run1()
        {
            int sumOfPriorities = 0;
            for (int i = 0; i < inputFile.Length; i++)
            {
                var ruckSack = inputFile[i];
                sumOfPriorities += GetSamePriorityInCompartments(ruckSack);
            }

            Console.WriteLine($"Answer is {sumOfPriorities}");
        }

        public int CharToPriority(char c) => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;

        readonly HashSet<int> _elements = new HashSet<int>();
        public int GetSamePriorityInCompartments(string ruckSack)
        {
            _elements.Clear();
            int compartimentLength = ruckSack.Length / 2;
            for (int i = 0; i < compartimentLength; i++)
                _elements.Add(CharToPriority(ruckSack[i]));
            for (int i = compartimentLength; i < ruckSack.Length; i++)
            {
                var priority = CharToPriority(ruckSack[i]);
                if (_elements.Contains(priority))
                    return priority;
            }
            throw new Exception("Invalid input file for AoC3 Part1");
        }

        readonly HashSet<int> _intersect = new HashSet<int>();
        public override void Run2()
        {
            int sumOfPriorities = 0;
            for (int i = 0; i < inputFile.Length; i+=3)
            {
                var ruckSack1 = inputFile[i];
                _elements.Clear();
                for (int j = 0; j < ruckSack1.Length; j++)
                    _elements.Add(CharToPriority(ruckSack1[j]));

                var ruckSack2 = inputFile[i+1];
                _intersect.Clear();
                for (int j = 0; j < ruckSack2.Length; j++)
                {
                    var priority = CharToPriority(ruckSack2[j]);
                    if (_elements.Contains(priority))
                        _intersect.Add(priority);
                }

                var ruckSack3 = inputFile[i+2];
                for (int j = 0; j < ruckSack3.Length; j++)
                {
                    var priority = CharToPriority(ruckSack3[j]);
                    if (_intersect.Contains(priority))
                    {
                        sumOfPriorities += priority;
                        break;
                    }
                }
            }
            Console.WriteLine($"Answer is {sumOfPriorities}");
        }
    }
}
