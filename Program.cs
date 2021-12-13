
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode_2021
{
    class Program
    {
        static void Main(string[] args)
        {
            RunLatest();
            //Run(13);
            Console.ReadKey();
        }
        
        static void RunLatest(bool example = false, ERunPart runPart = ERunPart.Both) { Run(AdventOfCode.LatestDay(), example, runPart); }
        static void Run(int day, bool example = false, ERunPart runPart = ERunPart.Both)
        {
            var aoc = AdventOfCode.Create(day, example);
            if (aoc == null) {
                Console.WriteLine($"No aoc class for day {day}");
                return;
            }
            Run(aoc, runPart);
        }
        static void Run(AdventOfCode aoc, ERunPart runPart = ERunPart.Both)
        {
            aoc.Init();
            if (runPart != ERunPart.Part2)
                aoc.Run1();
            if (runPart != ERunPart.Part1)
                aoc.Run2();
        }
    }
    enum ERunPart { Both, Part1, Part2 }
    abstract class AdventOfCode
    {
        static Dictionary<int, AdventOfCode> _aocTypes;
        static Dictionary<int, AdventOfCode> AoCs
        {
            get
            {
                if (_aocTypes == null)
                {
                    _aocTypes = new Dictionary<int, AdventOfCode>();
                    var types = Assembly.GetCallingAssembly().GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i].BaseType != typeof(AdventOfCode)) continue;
                        var day = int.Parse(new string(types[i].Name.Where(char.IsDigit).ToArray()));
                        AoCs[day] = Activator.CreateInstance(types[i]) as AdventOfCode;
                    }
                }
                return _aocTypes;
            }
        }
        public static int LatestDay() => AoCs.Select(kvp => kvp.Key).Max();
        public static AdventOfCode Create(int aocDay, bool example)
        {
            if (AoCs.TryGetValue(aocDay, out var aoc))
            {
                aoc.SetInput(example);
                return aoc;
            }
            return null;
        }
        
        protected string[] inputFile;
        void SetInput(bool example = false)
        {
            string number = "";
            string name = GetType().Name;
            for (int i = 0; i < name.Length; i++)
                if (char.IsDigit(name[i]))
                    number += name[i];

            string file = $"C:/Users/Nanorock/source/repos/AoC2021/INPUT/aoc_{number}";
            if (example) file += "_ex";
            inputFile = File.ReadAllLines($"{file}.txt");
        }


        public virtual void Init() { }
        public virtual void Run1() { }
        public virtual void Run2() { }

        protected static void WriteLine(string line) => Console.WriteLine(line);
        protected static void Wait() => Console.ReadKey();
    }
}
