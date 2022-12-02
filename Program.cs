
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using AdventOfCode_2021;

namespace AdventOfCodes
{
    class Program
    {
        static void Main(string[] args)
        {
            //WatchRun(18);
            //WatchRun(19);
            //RunLatest(false);
            WatchRunAll(2022);
            while(true)
                Console.ReadLine();
        }

        static void WatchRunAll(int year)
        {
            double totalTime = 0;
            for (int i = 1; i <= AdventOfCode.LatestDay(year); i++)
                totalTime+=WatchRun(year, i);
            ColorConsole.RainbowLine($"########################");
            ColorConsole.RainbowLine($"###TOTAL TIME:{totalTime:F2}ms###");
            ColorConsole.RainbowLine($"########################");
        }
        static double WatchRun(int year, int day)
        {
            ColorConsole.PrintLine($"<Blue>Running {AdventOfCode.GetName(year, day)}");
            var sw = Stopwatch.StartNew();
            Run(year, day);
            sw.Stop();
            ColorConsole.PrintLine($"<Blue>Ran in <Red>{sw.Elapsed.TotalMilliseconds}ms");
            Console.WriteLine();
            return sw.Elapsed.TotalMilliseconds;
        }
        
        static void RunLatest(int year, bool example = false, ERunPart runPart = ERunPart.Both) { Run(year, AdventOfCode.LatestDay(year), example, runPart); }
        static void Run(int year, int day, bool example = false, ERunPart runPart = ERunPart.Both)
        {
            var aoc = AdventOfCode.Create(year, day, example);
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
        static Dictionary<int,Dictionary<int, string>> _aocNames;
        static Dictionary<int,Dictionary<int, AdventOfCode>> _aocTypes;
        static Dictionary<int,Dictionary<int, AdventOfCode>> AoCs
        {
            get
            {
                if (_aocTypes == null)
                {
                    _aocTypes = new Dictionary<int,Dictionary<int, AdventOfCode>>();
                    _aocNames = new Dictionary<int,Dictionary<int, string>>();
                    var types = Assembly.GetCallingAssembly().GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i].BaseType != typeof(AdventOfCode)) continue;
                        int year = Year(types[i]);
                        var day = int.Parse(new string(types[i].Name.Where(char.IsDigit).ToArray()));

                        if (!AoCs.TryGetValue(year, out var yearTypes))
                            AoCs[year] = yearTypes = new Dictionary<int, AdventOfCode>();
                        yearTypes[day] = Activator.CreateInstance(types[i]) as AdventOfCode;

                        if (!_aocNames.TryGetValue(year, out var yearNames))
                            _aocNames[year] = yearNames = new Dictionary<int, string>();
                        yearNames[day] = types[i].Name;
                    }
                }
                return _aocTypes;
            }
        }
        public static int LatestDay(int year) => AoCs[year].Select(kvp => kvp.Key).Max();
        public static string GetName(int year, int day) => AoCs != null && _aocNames.TryGetValue(year, out var yearName) && yearName.TryGetValue(day, out var name) ? name : "Unknown";
        public static AdventOfCode Create(int year, int aocDay, bool example)
        {
            if (AoCs.TryGetValue(year, out var yearAoc) && yearAoc.TryGetValue(aocDay, out var aoc))
            {
                aoc.SetInput(aocDay, example);
                return aoc;
            }
            return null;
        }

        int _day;
        bool _example;
        protected string[] inputFile;
        void SetInput(int day,bool example = false)
        {
            _day = day;
            _example = example;
            inputFile = File.ReadAllLines(GetFilePath());
        }
        protected string GetFilePath()
        {
            int year = Year(GetType());
            string file = $"C:/Users/Nanorock/source/repos/AoC/{year}/INPUT/aoc_{_day}";
            if (_example) file += "_ex";
            return $"{file}.txt";
        }

        static int Year(Type t)=>int.Parse(t.Namespace.Where(char.IsDigit).ToArray());

        public virtual void Init() { }
        public virtual void Run1() { }
        public virtual void Run2() { }

        protected static void WriteLine(string line) => Console.WriteLine(line);
    }
}
