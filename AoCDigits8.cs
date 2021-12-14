using System;
using System.Collections.Generic;

namespace AdventOfCode_2021
{
    class AoCDigits8 : AdventOfCode
    {
        public override void Run1()
        {
            List<string> digits = new List<string>();
            int oneFourSevenEightCount = 0;

            for (int i = 0; i < inputFile.Length; i++)
            {
                var outputs = inputFile[i].Split('|')[1].Trim().Split(' ');
                for (int j = 0; j < outputs.Length; j++)
                {
                    var digit = outputs[j];
                    digits.Add(digit);
                    if (digit.Length <= 4 || digit.Length == 7)
                        ++oneFourSevenEightCount;
                }
            }
            WriteLine($"1/4/7/8 count:{oneFourSevenEightCount}");
        }
        public override void Run2()
        {
            int totalResult = 0;
            for (int i = 0; i < inputFile.Length; i++)
                totalResult += Puzzle.Decode(inputFile[i]);
            WriteLine($"Result:{totalResult}");
        }

        class Puzzle
        {
            static readonly Puzzle _instance = new Puzzle();
            public static int Decode(string inputString) => _instance.Result(inputString);

            readonly List<string> _signalPattern = new List<string>();
            readonly List<string> _digitOutput = new List<string>();
            readonly Dictionary<string, int> _orderedPuzzleToNumber = new Dictionary<string, int>();

            void Setup(string puzzleInput)
            {
                var puzzleSplit = puzzleInput.Split('|');
                _signalPattern.Clear();
                _digitOutput.Clear();
                _orderedPuzzleToNumber.Clear();

                _signalPattern.AddRange(puzzleSplit[0].Trim().Split(' '));
                _digitOutput.AddRange(puzzleSplit[1].Trim().Split(' '));
            }

            int Result(string puzzleInput)
            {
                Setup(puzzleInput);

                var one = _signalPattern.Find(digit => digit.Length == 2).OrderAlpha();
                var seven = _signalPattern.Find(digit => digit.Length == 3).OrderAlpha();
                var four = _signalPattern.Find(digit => digit.Length == 4).OrderAlpha();
                var eight = _signalPattern.Find(digit => digit.Length == 7).OrderAlpha();
                
                var sevenStripped = seven.RemoveChars(one);
                var fourStripped = four.RemoveChars(one);
                var eightStripped = eight.RemoveChars(four).RemoveChars(seven);

                string[] segments = {
                    sevenStripped,
                    fourStripped,
                    one,
                    fourStripped,
                    eightStripped,
                    one,
                    eightStripped,
                };

                for (int i = 0; i < _signalPattern.Count; i++)
                {
                    var input = _signalPattern[i];
                    if (input.Length == 2 || input.Length == 3 || input.Length == 4 || input.Length == 7) continue;
                    input = input.OrderAlpha();
                    
                    string digitBuilt = "";
                    for (int j = 0; j < segments.Length; j++)
                        if (input.Has(segments[j]))
                            digitBuilt += (char)('a' + j);

                    var contenders = input.Length == 5 ? FiveDigits : SixDigits;

                    int matchIndex = -1;
                    for (int j = 0; j < contenders.Count; j++)
                        if (contenders[j].Has(digitBuilt))
                            matchIndex = matchIndex == -1 ? j : -2;
                    if (matchIndex >= 0) //One match
                    {
                        var guessedDigit = contenders[matchIndex];
                        int number = Array.FindIndex(Numbers, s => s == guessedDigit);
                        _orderedPuzzleToNumber[input] = number;
                    }
                }

                _orderedPuzzleToNumber[one] = 1;
                _orderedPuzzleToNumber[seven] = 7;
                _orderedPuzzleToNumber[four] = 4;
                _orderedPuzzleToNumber[eight] = 8;
                int result = 0;
                for (int i = 0; i < _digitOutput.Count; i++)
                {
                    var ordered = _digitOutput[i].OrderAlpha();
                    int digit = _orderedPuzzleToNumber[ordered];
                    for (int j = 0; j < _digitOutput.Count - 1 - i; j++)
                        digit *= 10;
                    result += digit;
                }
                
                return result;
            }

            List<string> _fiveDigits; List<string> FiveDigits => _fiveDigits ??= new List<string> {"acdeg", "acdfg", "abdfg"};
            List<string> _sixDigits;  List<string> SixDigits => _sixDigits ??= new List<string> { "abcefg", "abdefg", "abcdfg" };
            string[] Numbers = { "abcefg", "cf","acdeg","acdfg", "bcdf","abdfg","abdefg","acf","abcdefg","abcdfg" };
        }
    }
}