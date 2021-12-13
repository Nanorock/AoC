using System.Collections.Generic;
namespace AdventOfCode_2021
{
    class AoCChunks10 : AdventOfCode
    {
        public override void Run1() { }
        public override void Run2()
        {
            int errorScore = 0;
            List<ulong> completionScores = new List<ulong>();
            Stack<char> chunks = new Stack<char>();
            for (int lineIndex = 0; lineIndex < inputFile.Length; lineIndex++)
            {
                var line = inputFile[lineIndex];
                bool corrupted = false;
                chunks.Clear();
                for (int charIndex = 0; charIndex < line.Length; charIndex++)
                {
                    var @char = line[charIndex];
                    if(ToClosings.TryGetValue(@char, out var closing))
                        chunks.Push(closing);
                    else if (chunks.Peek() == @char)
                        chunks.Pop();
                    else
                    {
                        errorScore += ToErrorScore[@char];
                        corrupted = true;
                        break;
                    }
                }
                if (corrupted) continue;
                ulong score = 0;
                foreach (var c in chunks)
                    score = score * 5 + ToCompletionScore[c];
                completionScores.Add(score);
            }
            WriteLine($"Incorrect char ending score:{errorScore}");
            completionScores.Sort();
            WriteLine($"AutoCompletion score:{completionScores[completionScores.Count / 2]}");
            
            Wait();
        }
        readonly Dictionary<char, char> ToClosings = new Dictionary<char, char>() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' }, };
        readonly Dictionary<char, int> ToErrorScore = new Dictionary<char, int>() { { ')', 3 }, { ']', 57}, { '}', 1197 }, { '>', 25137 }, };
        readonly Dictionary<char, ulong> ToCompletionScore = new Dictionary<char, ulong>() { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 }, };
    }
}