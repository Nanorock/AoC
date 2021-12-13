using System;

namespace AdventOfCode_2021
{
    class AoCNavigation2 : AdventOfCode
    {
        int _forward = 0;
        int _depth = 0;
        int _aim = 0;
        public override void Run1() { }
        public override void Run2()
        {
            for (int i = 0; i < inputFile.Length; i++)
            {
                var values = inputFile[i].Split(' ');
                Parse(values[0], int.Parse(values[1]));
            }

            Console.WriteLine($"Forward:{_forward}, depth:{_depth}");
            Console.WriteLine($"Answer is {_forward * _depth}");
            Console.ReadKey();
        }
        void Parse(string action, int value)
        {
            switch (action)
            {
                case "forward": Forward(value); break;
                case "down": Aim(value); break;
                case "up": Aim(-value); break;
            }
        }
        void Forward(int value)
        {
            _forward += value;
            _depth += _aim * value;
        }
        void Aim(int value)
        {
            _aim += value;
        }
    }
}