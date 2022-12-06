
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC._2022
{
    class AocTuningTrouble6 : AdventOfCodes.AdventOfCode
    {
        public override void Run1()
        {
            Console.WriteLine($"Answer is {GetNextPacketStart(0, 4, inputFile[0])}");
        }
        public override void Run2()
        {
            Console.WriteLine($"Answer is {GetNextPacketStart(0, 14, inputFile[0])}");
        }

        int GetNextPacketStart(int start, int distinctCharacter, string input)
        {
            CharBuffer cb = new CharBuffer(distinctCharacter);
            for (int charIndex = start; charIndex < input.Length; ++charIndex)
            {
                cb.AddChar(input[charIndex]);
                if(cb.IsFullyDistinct())
                    return charIndex + 1;
            }
            return -1;
        }

        class CharBuffer
        {
            public int Size;
            int _pointer = 0;
            int _count = 0;
            readonly char[] _buffer;
            readonly Dictionary<char, int> _redundancy;
            public CharBuffer(int size) {
                Size = size;
                _buffer = new char[Size];
                _redundancy = new Dictionary<char, int>(Size);
            }
            public void AddChar(char c)
            {
                _redundancy.TryGetValue(c, out var count);
                _redundancy[c] = count + 1;
                
                if(_count < Size)
                    _buffer[_count++] = c;
                else
                {
                    char removed = _buffer[_pointer];
                    if (--_redundancy[removed] == 0)
                        _redundancy.Remove(removed);

                    _buffer[_pointer++] = c;
                    if (_pointer >= Size) 
                        _pointer = 0;
                }
            }
            public bool IsFullyDistinct() => _redundancy.Count == Size;
        }
    }
}
