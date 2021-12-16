using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode_2021
{
    public class Tilemap:BaseTilemap<int>
    {
        public Tilemap(string[] inputFile):base(inputFile, c => c-'0') { }
        public Tilemap(int width, int height):base(width,height) { }
        public Tilemap(Tilemap copy) : base(copy) { }
        
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
        string GetState(int id, int value) => value > 0 ? value.ToString() : " ";

        protected override int GetGScore(int value) => value;

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

        public BaseTilemap(BaseTilemap<T> copy) : this(copy.Width, copy.Height)
        {
            Array.Copy(copy._board, _board, _board.Length);
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

        public int BFS_4(int start, Func<int, T, bool> expansion, List<int> result = null) => BFS(start, expansion, Get4Neighbors, result);
        public int BFS_8(int start, Func<int, T, bool> expansion, List<int> result = null) => BFS(start, expansion, Get8Neighbors, result);
        int BFS(int start, Func<int, T, bool> expansion, Func<int, Neighbors> getNeighbors, List<int> result = null)
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
                    if (expansion(neighId, value))
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Neighbors Get4Neighbors(int id)
        {
            var neighborSet = Neighbors.Get4();
            GetXY(id, out var x, out var y);
            int valid = -1;
            for (int i = 0; i < neighborSet.Length; i++)
            {
                int neighborId = GetId(x + Neighbors.X4Offset[i], y + Neighbors.Y4Offset[i]);
                if (neighborId >= 0) neighborSet[++valid] = neighborId;
            }
            neighborSet.SetLength(valid + 1);
            return neighborSet;
        }
            //=> GetNeighbors(id, Neighbors.X4Offset, Neighbors.Y4Offset, Neighbors.Get4());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Neighbors Get8Neighbors(int id) => GetNeighbors(id, Neighbors.X8Offset, Neighbors.Y8Offset, Neighbors.Get8());
        Neighbors GetNeighbors(int id, int[] xOffsets, int[] yOffsets, Neighbors neighborSet)
        {
            GetXY(id, out var x, out var y);
            int valid = -1;
            for (int i = 0; i < neighborSet.Length; i++)
            {
                int neighborId = GetId(x + xOffsets[i], y + yOffsets[i]);
                if (neighborId >= 0) neighborSet[++valid] = neighborId;
            }
            neighborSet.SetLength(valid+1);
            return neighborSet;
        }


        public TilemapPrinter<T> GetPrinter(Func<int, T, string> print) => new TilemapPrinter<T>(Get, print, _width, _height);

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


        protected virtual int GetGScore(T value)
        {
            return value.GetHashCode();
        }


        int[] _costFromStart;
        PriorityQueue _priorityQueue;
        public bool AStar(int start, int goal, List<int> result)
        {
            _priorityQueue = _priorityQueue ??= new PriorityQueue();
            _priorityQueue.Reset();
            
            _costFromStart ??= new int[_board.Length];
            for (int i = 0; i < _costFromStart.Length; i++)
                _costFromStart[i] = int.MaxValue;
            _costFromStart[start] = 0;
            GetXY(goal, out var x2, out var y2);

            _priorityQueue.Enqueue(start, 0);
            while (_priorityQueue.TryDequeue(out int lowestScoreId))
            {
                if (lowestScoreId == goal)
                {
                    GetPathFromGScore(start,lowestScoreId, result);
                    return true;
                }
                
                using var neighbors = Get4Neighbors(lowestScoreId);
                for (int i = 0; i < neighbors.Length; i++)
                {
                    int n = neighbors[i];
                    var tentativeCost = _costFromStart[lowestScoreId] + GetGScore(_board[n]);
                    if (tentativeCost >= _costFromStart[n]) continue;
                    _costFromStart[n] = tentativeCost;
                    GetXY(lowestScoreId, out var x1, out var y1);
                    int manhattan= Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
                    var priority = tentativeCost + manhattan;
                    _priorityQueue.Enqueue(n, priority);
                }
            }
            return false;
        }
        void GetPathFromGScore(int start, int current, List<int> res)
        {
            int prev = current;
            while (current != start)
            {
                res.Add(current);
                using var neighbors = Get4Neighbors(current);
                int lowestG = int.MaxValue;
                int lowestId = -1;
                for (int i = 0; i < neighbors.Length; i++)
                {
                    var nId = neighbors[i];
                    if (nId == prev) continue;
                    var g = _costFromStart[nId];
                    if (g < lowestG)
                    {
                        lowestG = g;
                        lowestId = nId;
                    }
                }
                prev = current;
                current = lowestId;
            }
            res.Reverse();
        }
    }
    class PriorityQueue
    {
        readonly List<int> _priorities = new List<int>();
        readonly Dictionary<int, List<int>> _prioToIds = new Dictionary<int, List<int>>();
        public void Enqueue(int id, int priority)
        {
            if (!_prioToIds.TryGetValue(priority, out var ids))
            {
                _prioToIds[priority] = ids = new List<int>();
                _priorities.Add(priority);
                _priorities.Sort((a, b) => b.CompareTo(a));
            }
            ids.Add(id);
        }
        
        public bool TryDequeue(out int elt)
        {
            if (_priorities.Count == 0)
            {
                elt = 0;
                return false;
            }

            var prio = _priorities[_priorities.Count-1];
            var ids = _prioToIds[prio];
            elt = ids[ids.Count - 1];
            ids.RemoveAt(ids.Count - 1);
            if (ids.Count == 0)
            {
                _priorities.RemoveAt(_priorities.Count-1);
                _prioToIds.Remove(prio);
            }
            return true;
        }

        public void Reset()
        {
            _priorities.Clear();
            _prioToIds.Clear();
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
        Func<int, T, string> _print;

        public TilemapPrinter(Func<int, T> getter, Func<int, T, string> print, int width, int height)
        {
            _sb = new StringBuilder();
            _width = width;
            _height = height;
            _board = getter;
            _print = print;
        }

        public string PrintState(Func<int, T, string> getStr)
        {
            return PrintState(_width, _height, getStr);
        }

        public string PrintState(int width, int height, bool inverseY = false) => PrintState(width, height, _print, inverseY);
        public string PrintState(int width, int height, Func<int, T, string> getStr, bool inverseY = false)
        {
            _sb.Clear();
            for (int y = 0; y < height; y++)
            {
                int py = y;
                if (inverseY) py = height - 1 - y;
                int start = py * _width;
                for (int x = 0; x < width; x++)
                    _sb.Append(getStr(start + x, _board(start + x)));
                _sb.AppendLine();
            }
            return _sb.ToString();
        }


        public IEnumerable<string> PrintStateLines(Func<int, T, string> getStr)
        {
            return PrintStateLines(_width, _height, getStr);
        }
        public IEnumerable<string> PrintStateLines(int width, int height) => PrintStateLines(width, height, _print);
        public IEnumerable<string> PrintStateLines(int width, int height, Func<int, T, string> getStr)
        {
            for (int y = 0; y < height; y++)
            {
                _sb.Clear();
                int py = y;
                int start = py * _width;
                for (int x = 0; x < width; x++)
                {
                    _sb.Append(getStr(start + x, _board(start + x)));
                }
                yield return _sb.ToString();
            }
        }

    }
}