using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode_2021
{
    public class Tilemap
    {
        int _width, _height;
        int[] _board;

        public int Width => _width;
        public int Height => _height;
        public int Size => _board.Length;

        public Tilemap(string[] inputFile) { Set(inputFile); }
        public Tilemap(int width, int height)
        {
            _width = width;
            _height = height;
            _board = new int[_width * _height];
        }

        public void Set(string[] inputFile)
        {
            _width = inputFile[0].Length;
            _height = inputFile.Length;
            _board = new int[_width * _height];
            for (int i = 0; i < inputFile.Length; i++)
            {
                var line = inputFile[i];
                int start = i * _width;
                for (int j = 0; j < line.Length; j++)
                    _board[start + j] = line[j] - '0';
            }
        }

        readonly HashSet<int> _bfsVisited = new HashSet<int>();
        readonly Queue<int> _bfsSearch = new Queue<int>();

        public int BFS(int start, Func<int, bool> expansion, List<int> result = null)
        {
            _bfsVisited.Clear();
            _bfsSearch.Clear();
            _bfsSearch.Enqueue(start);
            _bfsVisited.Add(start);
            int results = 0;
            while (_bfsSearch.Count > 0)
            {
                int id = _bfsSearch.Dequeue();
                ++results;
                result?.Add(id);

                int count = Get4Neighbors(id, out var fourNeighbors);
                for (int i = 0; i < count; i++)
                {
                    int neighId = fourNeighbors[i];
                    if (!_bfsVisited.Add(neighId)) continue;
                    int neighValue = _board[neighId];
                    if (expansion(neighValue))
                        _bfsSearch.Enqueue(neighId);
                }
            }

            return results;
        }

        public int GetId(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
                return -1;
            return x + y * _width;
        }

        public void GetXY(int id, out int x, out int y)
        {
            y = id / _width;
            x = id - y * _width;
        }

        public void FoldHorizontal(int yLine, int maxWidth, int maxHeight)
        {
            for (int y = yLine + 1; y < maxHeight; y++)
            {
                int yId = y * Width;
                int reflectY = 2 * yLine - y;
                if (reflectY < 0) break;
                int reflectYId = reflectY * Width;
                for (int x = 0; x < maxWidth; x++)
                {
                    int id = yId + x;
                    if (this[id] > 0)
                    {
                        this[reflectYId + x] = 1;
                        this[id] = 0;
                    }
                }
            }
        }
        public void FoldVertical(int xLine, int maxWidth, int maxHeight)
        {
            for (int x = xLine + 1; x < maxWidth; x++)
            {
                int reflectX = 2 * xLine - x;
                if (reflectX < 0) break;
                for (int y = 0; y < maxHeight; y++)
                {
                    int id = y * Width + x;
                    if (this[id] > 0)
                    {
                        this[y * Width + reflectX] = 1;
                        this[id] = 0;
                    }
                }
            }
        }

        static Stack<int[]> _4pool = new Stack<int[]>();
        static int[] _x4Offset = { 0, 1, 0, -1};
        static int[] _y4Offset = { 1, 0, -1, 0};
        public Temp Get4Neighbors(int id)
        {
            GetXY(id, out var x, out var y);
            var neighborSet = new Temp(_4pool);

            int valid = 0;
            for (int i = 0; i < neighborSet.Length; i++)
            {
                int neighborId = GetId(x + _x4Offset[i], y + _y4Offset[i]);
                if (neighborId >= 0) neighborSet[valid++] = neighborId;
            }
            neighborSet.SetLength(valid);
            return neighborSet;
        }

        //readonly int[] _eightNeighbors = new int[8];
        static int[] _x8Offset = { 0, 1, 1, 1, 0, -1, -1, -1 };
        static int[] _y8Offset = { 1, 1, 0, -1, -1, -1, 0, 1 };
        
        static Stack<int[]> _8pool = new Stack<int[]>();
        public struct Temp : IDisposable
        {
            public int[] Value;
            Stack<int[]> _pool;
            public Temp(Stack<int[]> pool)
            {
                _pool = pool;
                Value = pool.Count == 0 ? new int[8]: pool.Pop();
                _length = 8;
            }
            public void Dispose()
            {
                _pool.Push(Value);
                Value = null;
                _pool = null;
            }

            int _length;
            public void SetLength(int count) => _length = count;

            public int Length => _length;
            public int this[int key]
            {
                get => Value[key];
                set => Value[key] = value;
            }
        }
        public Temp Get8Neighbors(int id)
        {
            GetXY(id, out var x, out var y);
            var neighborSet = new Temp(_8pool);
            int valid = 0;
            for (int i = 0; i < neighborSet.Length; i++)
            {
                int neighborId = GetId(x + _x8Offset[i], y + _y8Offset[i]);
                if (neighborId >= 0) neighborSet[valid++] = neighborId;
            }
            neighborSet.SetLength(valid);
            return neighborSet;
        }

        StringBuilder _sb = new StringBuilder();
        public string PrintState()
        {
            return PrintState(Width, Height);
        }
        public string PrintState(int width, int height, bool inverseY = false)
        {
            _sb.Clear();
            for (int y = 0; y < height; y++)
            {
                int py = y;
                if (inverseY) py = height - 1 - y;
                int start = py * Width;
                for (int x = 0; x < width; x++)
                {
                    int value = _board[start + x];
                    var space = value > 0 ? value.ToString() : " ";
                    _sb.Append(space);
                }
                _sb.AppendLine();
            }
            return _sb.ToString();
        }

        public int Count(Func<int, bool> filter)
        {
            int count = 0;
            for (int i = 0; i < Size; i++)
            {
                if (filter != null ? filter(this[i]) : this[i] > 0)
                    ++count;
            }
            return count;
        }


        public int this[int key]
        {
            get
            {
                if (key < 0 || key >= Size) 
                    return -1;
                return _board[key];
            }
            set
            {
                if (key >= 0 && key < Size)
                    _board[key] = value;
            }
        }
    }
}