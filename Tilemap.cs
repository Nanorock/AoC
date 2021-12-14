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

        public TilemapPrinter<int> GetPrinter() => new TilemapPrinter<int>(Get, GetState, Width, Height);
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

        public int BFS_4(int start, Func<T, bool> expansion, List<int> result = null) => BFS(start, expansion, Get4Neighbors, result);
        public int BFS_8(int start, Func<T, bool> expansion, List<int> result = null) => BFS(start, expansion, Get8Neighbors, result);
        int BFS(int start, Func<T, bool> expansion, Func<int, Neighbors> getNeighbors, List<int> result = null)
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
                    var value = _board[neighId];
                    if (expansion(value))
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

        
        public Neighbors Get4Neighbors(int id) => GetNeighbors(id, Neighbors.X4Offset, Neighbors.Y4Offset, Neighbors.Get4);
        public Neighbors Get8Neighbors(int id) => GetNeighbors(id, Neighbors.X8Offset, Neighbors.Y8Offset, Neighbors.Get8);
        Neighbors GetNeighbors(int id, int[] xOffsets, int[] yOffsets, Func<Neighbors> getNeighbors)
        {
            GetXY(id, out var x, out var y);
            var neighborSet = getNeighbors();
            int valid = -1;
            for (int i = 0; i < neighborSet.Length; i++)
            {
                int neighborId = GetId(x + xOffsets[i], y + yOffsets[i]);
                if (neighborId >= 0) neighborSet[++valid] = neighborId;
            }
            neighborSet.SetLength(valid+1);
            return neighborSet;
        }


        public TilemapPrinter<T> GetPrinter(Func<T, string> print) => new TilemapPrinter<T>(Get, print, _width, _height);

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
        public static readonly int[] X4Offset = { 0, 1, 0, -1 };
        public static readonly int[] Y4Offset = { 1, 0, -1, 0 };

        public static readonly int[] X8Offset = { 0, 1, 1, 1, 0, -1, -1, -1 };
        public static readonly int[] Y8Offset = { 1, 1, 0, -1, -1, -1, 0, 1 };

        static readonly Stack<int[]> Pool4 = new Stack<int[]>();
        static readonly Stack<int[]> Pool8 = new Stack<int[]>();
        public static Neighbors Get4() => new Neighbors(Pool4,4);
        public static Neighbors Get8() => new Neighbors(Pool8,8);

        int[] _value;
        Stack<int[]> _pool;
        Neighbors(Stack<int[]> pool, int size)
        {
            _pool = pool;
            _value = pool.Count == 0 ? new int[size] : pool.Pop();
            _length = size;
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


    public struct TilemapPrinter
    {
        static readonly string[] _colors = { "Red", "Yellow", "Green", "Cyan", "Blue", "Magenta" };
        public static string GetColor(int id) => ColorConsole.GetBlock(_colors[id % _colors.Length]);
    }
    public struct TilemapPrinter<T>
    {
        StringBuilder _sb;
        int _width, _height;
        Func<int, T> _board;
        Func<T, string> _print;

        public TilemapPrinter(Func<int, T> getter, Func<T, string> print, int width, int height)
        {
            _sb = new StringBuilder();
            _width = width;
            _height = height;
            _board = getter;
            _print = print;
        }

        public string PrintState(Func<T, string> getStr)
        {
            return PrintState(_width, _height, getStr);
        }

        public string PrintState(int width, int height, bool inverseY = false) => PrintState(width, height, _print, inverseY);
        public string PrintState(int width, int height, Func<T, string> getStr, bool inverseY = false)
        {
            _sb.Clear();
            for (int y = 0; y < height; y++)
            {
                int py = y;
                if (inverseY) py = height - 1 - y;
                int start = py * _width;
                for (int x = 0; x < width; x++)
                    _sb.Append(getStr(_board(start + x)));
                _sb.AppendLine();
            }
            return _sb.ToString();
        }



        public IEnumerable<string> PrintStateLines(int width, int height, int xColorWidth) => PrintStateLines(width, height, _print, xColorWidth);
        public IEnumerable<string> PrintStateLines(int width, int height, Func<T, string> getStr, int xColorWidth)
        {
            for (int y = 0; y < height; y++)
            {
                _sb.Clear();
                int py = y;
                int start = py * _width;
                for (int x = 0; x < width; x++)
                {
                    if (x % xColorWidth == 0)
                        _sb.Append(TilemapPrinter.GetColor(x / xColorWidth));
                    _sb.Append(getStr(_board(start + x)));
                }
                yield return _sb.ToString();
            }
        }

    }
}