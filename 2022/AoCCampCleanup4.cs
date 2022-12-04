using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC._2022
{
    class AoCCampCleanup4 : AdventOfCodes.AdventOfCode
    {
        int _overlaps;
        public override void Run1()
        {
            var elfPairs = new ElfPair[inputFile.Length];
            int containsTheOther = 0;
            for (int i = 0; i < inputFile.Length; i++)
            {
                var pair = elfPairs[i] = new ElfPair(inputFile[i]);
                if (pair.OneContainsTheOther())
                    ++containsTheOther;
                if (pair.Overlaps())
                    ++_overlaps;
            }
            Console.WriteLine($"Answer is {containsTheOther}");
        }
        public override void Run2()
        {
            Console.WriteLine($"Answer is {_overlaps}");
        }
    }

    struct ElfPair
    {
        SectionRange Elf1, Elf2;
        public ElfPair(string input)
        {
            int separatorId = input.IndexOf(",");
            Elf1 = new SectionRange(input.Substring(0, separatorId));
            Elf2 = new SectionRange(input.Substring(separatorId+1));
        }
        public bool OneContainsTheOther() => Elf1.Contains(Elf2) || Elf2.Contains(Elf1);
        public bool Overlaps() => Elf1.Overlaps(Elf2);
    }
    struct SectionRange
    {
        public int Min,Max;
        public SectionRange(string range)
        {
            int separatorId = range.IndexOf("-");
            Min = int.Parse(range.Substring(0, separatorId));
            Max = int.Parse(range.Substring(separatorId+1));
        }
        public bool Contains(SectionRange other) => other.Min >= Min && other.Max <= Max;
        public bool Overlaps(SectionRange other) => Min <= other.Max && other.Min <= Max;
    }
}
