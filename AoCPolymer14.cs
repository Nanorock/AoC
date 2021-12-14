using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2021
{
    class AoCPolymer14 : AdventOfCode
    {
        readonly Dictionary<(char, char), char> _insertions = new Dictionary<(char, char), char>();
        Dictionary<(char, char), ulong> _templatePairCounts = new Dictionary<(char, char), ulong>();
        Dictionary<(char, char), ulong> _pairLatest = new Dictionary<(char, char), ulong>();
        
        readonly ulong[] _charCounts = new ulong[26];
        void AddChar(char c, ulong count = 1) => _charCounts[c-'A'] += count;
        void ResetChars() => Array.Clear(_charCounts,0, _charCounts.Length);

        public override void Init()
        {
            for (int i = 2; i < inputFile.Length; i++)
            {
                var line = inputFile[i];
                _insertions[(line[0], line[1])] = line[6];
            }
        }
        public override void Run1() { Run(10); }
        public override void Run2() { Run(40); }

        public void Run(int count)
        {
            InitData();

            for (int i = 0; i < count; i++)
                RunProcess();

            Array.Sort(_charCounts);
            int lowest = Array.FindIndex(_charCounts, c => c > 0);
            WriteLine($"Most occurrences {_charCounts[_charCounts.Length - 1]}, min {_charCounts[lowest]}, result:{(_charCounts[_charCounts.Length - 1] - _charCounts[lowest])}");
        }
        void RunProcess()
        {
            _pairLatest.Clear();
            foreach(var templatePair in _templatePairCounts)
            {
                var pair = templatePair.Key;
                var pairCount = templatePair.Value;
                if (!_insertions.TryGetValue(pair, out var insertChar)) continue;
                AddChar(insertChar, pairCount);
                AddPair(_pairLatest, (pair.Item1, insertChar), pairCount);
                AddPair(_pairLatest, (insertChar, pair.Item2), pairCount);
            }

            var swap = _templatePairCounts;
            _templatePairCounts = _pairLatest;
            _pairLatest = swap;
        }

        void InitData()
        {
            ResetChars();
            _templatePairCounts.Clear();
            var input = inputFile[0];
            for (int i = 1; i < input.Length; i++)
            {
                AddChar(input[i]);
                AddPair(_templatePairCounts, (input[i - 1], input[i]), 1);
            }
        }

        void AddPair(Dictionary<(char, char), ulong> dict, (char, char) pair, ulong count)
        {
            if (!dict.ContainsKey(pair))
                dict[pair] = 0;
            dict[pair] += count;
        }
    }
}