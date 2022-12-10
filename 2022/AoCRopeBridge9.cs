using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodes;

namespace AoC._2022
{
    class AoCRopeBridge9 : AdventOfCodes.AdventOfCode
    {
        Vector2i H, T;
        HashSet<string> _visited = new HashSet<string>();
        public override void Run1()
        {
            _visited.Add(T.ToString());
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
                {
                    H += dir;
                    var diff = H - T;
                    if(Math.Abs(diff.x) > 1 || Math.Abs(diff.y) > 1)
                    {
                        
                        int mx = diff.x / (diff.x == 0 ? 1 : Math.Abs(diff.x));
                        int my = diff.y / (diff.y == 0 ? 1 : Math.Abs(diff.y));
                        T += new Vector2i(mx, my);
                        _visited.Add(T.ToString());
                    }
                }
            }
            Console.WriteLine($"Answer is {_visited.Count}");
        }

        public override void Run2()
        {
            Rope head = new Rope();
            Rope rope = head;
            for (int i = 0; i < 9; i++)
                rope = rope.Previous = new Rope();

            _visited.Clear();
            _visited.Add(Vector2i.ZEROS.ToString());
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
            public Vector2i Position;
            public Rope Previous;
            public void Update(Vector2i move, HashSet<string> visited)
            {
                Position += move;
                if (Previous == null)
                {
                    visited.Add(Position.ToString());
                    return;
                }

                var diff = Position - Previous.Position;
                if(Math.Abs(diff.x) > 1 || Math.Abs(diff.y) > 1)
                {
                        
                    int mx = diff.x / (diff.x == 0 ? 1 : Math.Abs(diff.x));
                    int my = diff.y / (diff.y == 0 ? 1 : Math.Abs(diff.y));
                    Previous.Update(new Vector2i(mx, my), visited);
                }
            }
        }
    }
}
