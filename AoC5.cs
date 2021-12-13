using System;

namespace AdventOfCode_2021
{
    class AoC5 : AdventOfCode
    {
        Line[] _lines;
        Board _board;

        public override void Run1() { }
        public override void Run2()
        {
            int max = -1;
            _lines = new Line[inputFile.Length];
            for (int i = 0; i < inputFile.Length; i++)
            {
                var line = new Line(inputFile[i]);
                _lines[i] = line;
                if (line.MaxX > max) max = line.MaxX;
                if (line.MaxY > max) max = line.MaxY;
            }

            _board = new Board(max + 1);

            for (int i = 0; i < _lines.Length; i++)
            {
                var line = _lines[i];
                _board.Draw(line);
            }

            WriteLine($"Overlaps:{_board.CountOverlaps()}");
            Console.ReadKey();
        }
    }

    class Board
    {
        int[] _board;
        int _width;

        public Board(int width)
        {
            _width = width;
            _board = new int[_width * _width];
        }

        int Index(int x, int y) => x + y * _width;
        void ToXY(int index, out int x, out int y)
        {
            y = index / _width;
            x = index - y * _width;
        }

        public void Draw(in Line line)
        {
            Point p1 = line.P1;
            Draw(p1);

            Point p2 = line.P2;
            while (p1 != p2)
            {
                var diffX = p2.x - p1.x;
                var dx = Math.Sign(diffX);
                p1.x += dx;

                var diffY = p2.y - p1.y;
                var dy = Math.Sign(diffY);
                p1.y += dy;
                Draw(p1);
            }
        }

        public void Draw(in Point p) => ++_board[Index(p.x, p.y)];

        public int CountOverlaps()
        {
            int count = 0;
            for (int i = 0; i < _board.Length; i++)
                if (_board[i] > 1)
                    ++count;
            return count;
        }
    }
    struct Point
    {
        public int x;
        public int y;
        public Point(string parse)
        {
            var data = parse.Split(',');
            x = int.Parse(data[0]);
            y = int.Parse(data[1]);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point p)
                return p.x == x && p.y == y;
            return false;
        }
        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();

        public static bool operator ==(Point a, Point b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Point a, Point b) => !(a == b);

        public override string ToString() => $"{GetType().Name} ( {x }; {y})";
    }
    struct Line
    {
        public Point[] Points;
        public Point P1 { get => Points[0]; set => Points[0] = value; }
        public Point P2 { get => Points[1]; set => Points[1] = value; }

        static char[] _separator;
        static char[] Separator => _separator ??= " -> ".ToCharArray();
        public Line(string parse)
        {
            Points = new Point[2];
            var data = parse.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            P1 = new Point(data[0]);
            P2 = new Point(data[1]);
        }

        public bool IsVertical => P1.x == P2.x;
        public bool IsHorizontal => P1.y == P2.y;

        public int MaxX => P1.x > P2.x ? P1.x : P2.x;
        public int MaxY => P1.y > P2.y ? P1.y : P2.y;
        public int MinX => P1.x < P2.x ? P1.x : P2.x;
        public int MinY => P1.y < P2.y ? P1.y : P2.y;
    }
}