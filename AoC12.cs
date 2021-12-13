using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_2021
{
    class AoC12 : AdventOfCode
    {
        Cave _start;
        public override void Init() { _start = BuildCaveMap(); }
        public override void Run1() { }
        public override void Run2() { Run(); }
        void Run()
        {
            var sw = Stopwatch.StartNew();
            int pathCount = SpecialCaveMapper.CaveMap(_start);
            sw.Stop();
            WriteLine($"Number of path found:{pathCount} in {sw.Elapsed.TotalMilliseconds}ms");
        }
        Cave BuildCaveMap()
        {
            var caves = new Dictionary<string, Cave>();
            for (int i = 0; i < inputFile.Length; i++)
            {
                var caveNames = inputFile[i].Split('-');
                if (!caves.TryGetValue(caveNames[0], out var c1)) caves[caveNames[0]] = c1 = new Cave(caveNames[0]);
                if (!caves.TryGetValue(caveNames[1], out var c2)) caves[caveNames[1]] = c2 = new Cave(caveNames[1]);
                if (!IsExtremity(caveNames[1])) Add(c1, c2); if (IsEnd(caveNames[1])) c1.HasEnd = true;
                if (!IsExtremity(caveNames[0])) Add(c2, c1); if (IsEnd(caveNames[0])) c2.HasEnd = true;
            }
            return caves["start"];
        }
        bool IsEnd(string s) => s == "end";
        bool IsStart(string s) => s == "start";
        bool IsExtremity(string s) => IsEnd(s) || IsStart(s);
        void Add(Cave from, Cave c)
        {
            Array.Resize(ref from.Connections, from.Connections.Length + 1);
            from.Connections[from.Connections.Length - 1] = c;
        }

        class Cave
        {
            public Cave(string name) { IsLarge = char.IsUpper(name[0]); }
            public readonly bool IsLarge;
            public bool HasEnd;
            public int VisitCount;
            public Cave[] Connections = new Cave[0];
        }
        class SpecialCaveMapper
        {
            public static int CaveMap(Cave start) => new SpecialCaveMapper().Map(start);
            int _pathFound;
            int Map(Cave start)
            {
                _pathFound = 0;
                Expand(start);
                return _pathFound;
            }
            Cave _doubleCave;
            void Expand(Cave fromCave)
            {
                ++fromCave.VisitCount;
                for (int i = 0; i < fromCave.Connections.Length; ++i)
                {
                    var nextCave = fromCave.Connections[i];
                    if (!nextCave.IsLarge && nextCave.VisitCount != 0)
                    {
                        if (_doubleCave != null) continue;
                        _doubleCave = nextCave;
                        Expand(nextCave);
                    }
                    else
                    {
                        Expand(nextCave);
                    }
                }
                if (fromCave.HasEnd)
                    ++_pathFound;
                --fromCave.VisitCount;
                if (fromCave == _doubleCave)
                    _doubleCave = null;
            }
        }
    }
}