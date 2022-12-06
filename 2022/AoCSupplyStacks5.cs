using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodes;

namespace AoC._2022
{
    class AoCSupplyStacks5 : AdventOfCodes.AdventOfCode
    {
        Stack<char>[] _stacks;
        int _commandIndex;
        public override void Run1()
        {
            ParseInput(out _commandIndex);
            for (int i = _commandIndex; i < inputFile.Length; i++)
                ParseCommand(inputFile[i], MoveCommand9000);
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _stacks.Length; i++)
                sb.Append(_stacks[i].Peek());
            Console.WriteLine($"Answer is {sb.ToString()}");
        }
        void ParseInput(out int finishedParsingIndex)
        {
            int stackLength = (inputFile[0].Length+1) / 4;
            _stacks = new Stack<char>[stackLength];
            for (int i = 0; i < _stacks.Length; i++)
                _stacks[i] = new Stack<char>(16);

            finishedParsingIndex = -1;
            for (int i = 0; i < inputFile.Length; i++)
            {
                if (char.IsDigit(inputFile[i][1])) // numbers
                {
                    finishedParsingIndex = i;
                    break;
                }
            }

            for (int i = finishedParsingIndex-1; i >= 0; --i)
            {
                var input = inputFile[i];
                int stackIndex = 0;
                int charIndex = 1;
                while (charIndex < input.Length)
                {
                    char character = input[charIndex];
                    if(character != ' ')
                        _stacks[stackIndex].Push(character);
                    ++stackIndex;
                    charIndex += 4;
                }
            }
            finishedParsingIndex += 2;
        }

        public override void Run2()
        {
            ParseInput(out _commandIndex);
            for (int i = _commandIndex; i < inputFile.Length; i++)
                ParseCommand(inputFile[i], MoveCommand9001);
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _stacks.Length; i++)
                sb.Append(_stacks[i].Peek());
            Console.WriteLine($"Answer is {sb.ToString()}");
        }

        void ParseCommand(string command, Action<int,int,int> moveCommand)
        {
            if (command.StartsWith("move"))
                MoveCommand(command.Substring(5), moveCommand);
        }
        void MoveCommand(string input, Action<int,int,int> moveCommand)
        {
            int countIndex = input.IndexOf(" ");
            int count = int.Parse(input.Substring(0, countIndex));
            
            int fromStartIndex = input.IndexOf(" ", countIndex + 2);
            int fromEndIndex = input.IndexOf(" ", fromStartIndex + 1);
            int from = int.Parse(input.Substring(fromStartIndex+1, fromEndIndex-fromStartIndex-1));

            int toIndex = input.IndexOf(" ", fromEndIndex + 2);
            int to = int.Parse(input.Substring(toIndex));
            moveCommand(count, from-1, to-1);
        }
        void MoveCommand9000(int count, int from, int to)
        {
            for (int i = 0; i < count; i++)
                _stacks[to].Push(_stacks[from].Pop());
        }
        void MoveCommand9001(int count, int from, int to)
        {
            var cache = StackCache<char>.Value;
            cache.Clear();
            for (int i = 0; i < count; i++)
                cache.Push(_stacks[from].Pop());
            while(cache.Count > 0)
                _stacks[to].Push(cache.Pop());
        }
        
    }
}
