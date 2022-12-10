using System.Diagnostics.CodeAnalysis;
using AdventOfCodes;

namespace AoC._2022
{
    class AoCRopeBridge9 : AdventOfCodes.AdventOfCode
    {
        readonly HashSet<int> _visited = new HashSet<int>();
        public override void Run1() => SimulateRope(2);
        public override void Run2() => SimulateRope(10);

        void SimulateRope(int count)
        {
            var head = new Rope(count);
            _visited.Clear();
            _visited.Add(0);
            for (int i = 0; i < inputFile.Length; i++)
            {
                var input = inputFile[i];
                Vector2i dir = input[0] switch
                {
                    'R' => new Vector2i(1),
                    'L' => new Vector2i(-1),
                    'U' => new Vector2i(0, 1),
                    'D' => new Vector2i(0, -1),
                };
                int length = int.Parse(input[2..]);
                for (int j = 0; j < length; j++)
                    head.Update(dir, _visited);
            }
            Console.WriteLine($"Answer is {_visited.Count}");
        }

        class Rope
        {
            Vector2i _position;
            [MaybeNull] readonly Rope _previous = null;
            public Rope(int count) { if (count > 1) _previous = new Rope(count - 1); }
            const int MaxHorizontalMovement = 10000;
            public void Update(Vector2i move, HashSet<int> visited)
            {
                _position += move;
                if (_previous == null)
                {
                    visited.Add(_position.x + _position.y * MaxHorizontalMovement);//Record Tail visits
                    return;
                }

                var diff = _position - _previous._position;
                if (Math.Abs(diff.x) <= 1 && Math.Abs(diff.y) <= 1) return;
                int mx = diff.x / (diff.x == 0 ? 1 : Math.Abs(diff.x));
                int my = diff.y / (diff.y == 0 ? 1 : Math.Abs(diff.y));
                _previous.Update(new Vector2i(mx, my), visited);
            }
        }
    }
}
