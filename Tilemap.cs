using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode_2021
{
    public class Tilemap:BaseTilemap<int>
    {
        public Tilemap(string[] inputFile):base(inputFile, c => c-'0') { }
        public Tilemap(int width, int height):base(width,height) { }
        
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
                    if (_board[id] > 0)
                    {
                        _board[reflectYId + x] = 1;
                        _board[id] = 0;
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
                    if (_board[id] > 0)
                    {
                        _board[y * Width + reflectX] = 1;
                        _board[id] = 0;
                    }
                }
            }
        }

        public string PrintState() => PrintState(Width, Height);
        public string PrintState(int width, int height, bool inverseY = false) => PrintState(width, height, GetState, inverseY);
        public IEnumerable<string> PrintStateLines(int width, int height, int xColorWidth) => PrintStateLines(width, height, GetState, xColorWidth);
        string GetState(int id) => id > 0 ? id.ToString() : " ";
    }
    public class BaseTilemap<T>
    {
        int _width, _height;
        protected T[] _board;

        public int Width => _width;
        public int Height => _height;
        public int Size => _board.Length;

        public BaseTilemap(string[] inputFile, Func<char, T> get) { Set(inputFile, get); }
        public BaseTilemap(int width, int height)
        {
            _width = width;
            _height = height;
            _board = new T[_width * _height];
        }
        void Set(string[] inputFile, Func<char,T> get)
        {
            _width = inputFile[0].Length;
            _height = inputFile.Length;
            _board = new T[_width * _height];
            for (int i = 0; i < inputFile.Length; i++)
            {
                var line = inputFile[i];
                int start = i * _width;
                for (int j = 0; j < line.Length; j++)
                    _board[start + j] = get(line[j]);
            }
        }

        readonly HashSet<int> _bfsVisited = new HashSet<int>();
        readonly Queue<int> _bfsSearch = new Queue<int>();

        public int BFS_4(int start, Func<int, bool> expansion, List<int> result = null) => BFS(start, expansion, Get4Neighbors, result);
        public int BFS_8(int start, Func<int, bool> expansion, List<int> result = null) => BFS(start, expansion, Get8Neighbors, result);
        int BFS(int start, Func<int, bool> expansion, Func<int, Neighbors> getNeighbors, List<int> result = null)
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

                using var neighbors = getNeighbors(id);
                for (int i = 0; i < neighbors.Length; i++)
                {
                    int neighId = neighbors[i];
                    if (!_bfsVisited.Add(neighId)) continue;
                    if (expansion(neighId))
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

        static readonly int[] X4Offset = { 0, 1, 0, -1 };
        static readonly int[] Y4Offset = { 1, 0, -1, 0 };
        public Neighbors Get4Neighbors(int id)
        {
            GetXY(id, out var x, out var y);
            var neighborSet = Neighbors.Get4();

            int valid = 0;
            for (int i = 0; i < neighborSet.Length; i++)
            {
                int neighborId = GetId(x + X4Offset[i], y + Y4Offset[i]);
                if (neighborId >= 0) neighborSet[valid++] = neighborId;
            }
            neighborSet.SetLength(valid);
            return neighborSet;
        }

        static readonly int[] X8Offset = { 0, 1, 1, 1, 0, -1, -1, -1 };
        static readonly int[] Y8Offset = { 1, 1, 0, -1, -1, -1, 0, 1 };
        public Neighbors Get8Neighbors(int id)
        {
            GetXY(id, out var x, out var y);
            var neighborSet = Neighbors.Get8();
            int valid = 0;
            for (int i = 0; i < neighborSet.Length; i++)
            {
                int neighborId = GetId(x + X8Offset[i], y + Y8Offset[i]);
                if (neighborId >= 0) neighborSet[valid++] = neighborId;
            }
            neighborSet.SetLength(valid);
            return neighborSet;
        }

        StringBuilder _sb = new StringBuilder();
        public string PrintState(Func<T, string> getStr)
        {
            return PrintState(Width, Height, getStr);
        }

        public string PrintState(int width, int height, Func<T,string> getStr,  bool inverseY = false)
        {
            _sb.Clear();
            for (int y = 0; y < height; y++)
            {
                int py = y;
                if (inverseY) py = height - 1 - y;
                int start = py * Width;
                for (int x = 0; x < width; x++)
                    _sb.Append(getStr(_board[start + x]));
                _sb.AppendLine();
            }
            return _sb.ToString();
        }

        static string[] _colors = { "Red", "Yellow", "Green", "Cyan", "Blue", "Magenta" };
        static string GetColor(int id) => "{" + _colors[id % _colors.Length] + "}";
        public IEnumerable<string> PrintStateLines(int width, int height, Func<T, string> getStr, int xColorWidth)
        {
            for (int y = 0; y < height; y++)
            {
                _sb.Clear();
                int py = y;
                int start = py * Width;
                for (int x = 0; x < width; x++)
                {
                    if (x % xColorWidth == 0)
                        _sb.Append(GetColor(x / xColorWidth));
                    _sb.Append(getStr(_board[start + x]));
                }
                yield return _sb.ToString();
            }
        }

        public int Count(Func<T, bool> filter)
        {
            int count = 0;
            for (int i = 0; i < Size; i++)
                if (filter(_board[i]))
                    ++count;
            return count;
        }


        public T this[int key]
        {
            get => Get(key);
            set => Set(key, value);
        }
        public T Get(int x, int y)
        {
            return Get(GetId(x, y));
        }
        public T Get(int key)
        {
            if (key < 0 || key >= Size)
                return default;
            return _board[key];
        }
        public void Set(int x, int y, T value)
        {
            Set(GetId(x, y), value);
        }
        public void Set(int key, T value)
        {
            if (key >= 0 && key < Size)
                _board[key] = value;
        }
    }

    public struct Neighbors : IDisposable
    {
        static readonly Stack<int[]> Pool4 = new Stack<int[]>();
        static readonly Stack<int[]> Pool8 = new Stack<int[]>();
        public static Neighbors Get4() => new Neighbors(Pool4);
        public static Neighbors Get8() => new Neighbors(Pool8);

        int[] _value;
        Stack<int[]> _pool;
        Neighbors(Stack<int[]> pool)
        {
            _pool = pool;
            _value = pool.Count == 0 ? new int[8] : pool.Pop();
            _length = 8;
        }
        public void Dispose()
        {
            _pool.Push(_value);
            _value = null;
            _pool = null;
        }

        int _length;
        public void SetLength(int count) => _length = count;

        public int Length => _length;
        public int this[int key]
        {
            get => _value[key];
            set => _value[key] = value;
        }
    }

}