using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AdventOfCodes;

namespace AdventOfCode_2021
{
    class AoCBoardDrawing5 : AdventOfCode
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
                _board.Draw(_lines[i]);

            WriteLine($"Overlaps:{_board.CountOverlaps()}");
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

        public void Draw(in Line line)
        {
            Point p1 = line.Points[0];
            ++_board[p1.x + p1.y * _width];
            ref readonly Point p2 = ref line.Points[1];
            while (p1 != p2)
            {
                var diffX = p2.x - p1.x;
                var dx = Math.Sign(diffX);
                p1.x += dx;

                var diffY = p2.y - p1.y;
                var dy = Math.Sign(diffY);
                p1.y += dy;
                ++_board[p1.x + p1.y * _width];
            }
        }

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
        static char[] _separator;
        static char[] Separator => _separator ??= " -> ".ToCharArray();
        public Line(string parse)
        {
            Points = new Point[2];
            var data = parse.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            Points[0] = new Point(data[0]);
            Points[1] = new Point(data[1]);
        }
        public int MaxX => Points[0].x > Points[1].x ? Points[0].x : Points[1].x;
        public int MaxY => Points[0].y > Points[1].y ? Points[0].y : Points[1].y;
    }
}