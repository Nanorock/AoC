using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode_2021
{
    public static class HashCache<T>
    {
        public static HashSet<T> Value = new HashSet<T>();
    }
    public static class ListEx
    {
        public static void RemoveIntersect<T>(this List<T> list, List<T> values)
        {
            var hash = HashCache<T>.Value;
            for (int i = 0; i < values.Count; i++)
                hash.Add(values[i]);

            for (int i = list.Count - 1; i >= 0; i--)
                if (!hash.Remove(list[i]))
                    list.RemoveAt(i);

            hash.Clear();
        }
        public static void RemoveIntersect<T>(this List<T> list, IEnumerable<T> values)
        {
            var hash = HashCache<T>.Value;
            foreach (var v in values)
                hash.Add(v);

            for (int i = list.Count - 1; i >= 0; i--)
                if (!hash.Remove(list[i]))
                    list.RemoveAt(i);

            hash.Clear();
        }

        public static void Reset<T>(this List<T> list, IEnumerable<T> values)
        {
        }
    }
    public static class EnumerableExt
    {
        static StringBuilder _sb = new StringBuilder();
        public static string ToString<T>(this IEnumerable<T> list, char separator)
        {
            _sb.Clear();
            foreach (var t in list)
            {
                _sb.Append(t.ToString());
                _sb.Append(separator);
            }
            var ret = _sb.ToString();
            return ret.Substring(0, ret.Length - 1);
        }
    }

    static class StringExt
    {
        public static string RemoveChars(this string input, string remove)
        {
            string ret = input;
            for (int i = 0; i < remove.Length; i++)
                ret = ret.RemoveChar(remove[i]);
            return ret;
        }
        public static string RemoveChar(this string input, char remove)
        {
            for (int i = input.Length - 1; i >= 0; i--)
                if (input[i] == remove)
                    return input.Remove(i, 1);
            return input;
        }
        public static string OrderAlpha(this string input) => string.Concat(input.OrderBy(c => c));

        public static bool Has(this string input, HashSet<char> chars)
        {
            int match = 0;
            for (int i = 0; i < input.Length; i++)
                if (chars.Contains(input[i]))
                    ++match;
            return match == chars.Count;
        }
        public static bool Has(this string input, string other)
        {
            int match = 0;
            for (int i = 0; i < other.Length; i++)
                if (input.Contains(other[i]))
                    ++match;
            return match == other.Length;
        }
    }

    public static class ColorConsole
    {
        static readonly string[] _rainbowColors = { "Red", "Yellow", "Green", "Cyan", "Blue", "Magenta" };
        public static string GetBlock(string color) => $"<{color}>";
        public static void PrintLine(string formatted)
        {
            if (string.IsNullOrEmpty(formatted)) return;
            int colorCodeStart = formatted.IndexOf('<');
            if (colorCodeStart < 0)
            {
                PrintDirectLine(formatted);
                return;
            }
            int colorCodeEnd = formatted.IndexOf('>');
            if (colorCodeEnd < 0)
            {
                PrintDirectLine(formatted);
                return;
            }

            if (colorCodeStart > 0)
                PrintDirect(formatted.Substring(0, colorCodeStart));

            while (colorCodeStart >= 0 && colorCodeEnd >= 0)
            {
                var colorStr = formatted.Substring(colorCodeStart+1, colorCodeEnd - colorCodeStart - 1);
                Console.ForegroundColor = StrColors[colorStr];
                formatted = formatted.Substring(colorCodeEnd + 1);
                
                colorCodeStart = formatted.IndexOf('<');
                colorCodeEnd = formatted.IndexOf('>');
                PrintDirect(colorCodeStart < 0 ? formatted : formatted.Substring(0, colorCodeStart));
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        static int _rainbows;
        public static void RainbowLine(string raw)
        {
            int start = _rainbows += 2;
            string formated = "";
            for (int i = 0; i < raw.Length; i++)
                formated += GetBlock(_rainbowColors[(start + i) % _rainbowColors.Length]) + raw[i];
            PrintLine(formated);
        }

        static void PrintDirect(string direct) { Console.Write(direct); }
        static void PrintDirectLine(string direct) { Console.WriteLine(direct); }
        
        static ConsoleColor[] _colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
        static Dictionary<string, ConsoleColor> _strColors;
        static Dictionary<string, ConsoleColor> StrColors
        {
            get
            {
                if (_strColors == null)
                {
                    _strColors = new Dictionary<string, ConsoleColor>();
                    _strColors[""] = ConsoleColor.White;
                    for (int i = 0; i < _colors.Length; i++)
                        _strColors[_colors[i].ToString()] = _colors[i];
                }
                return _strColors;
            }
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2i : IEquatable<Vector2i>
    {
        static Vector2i _zeros = new Vector2i(0, 0);
        public static Vector2i ZEROS => _zeros;

        static Vector2i _ones = new Vector2i(1, 1);
        public static Vector2i ONES => _ones;

        public int x, y;

        public Vector2i(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2i(Vector2i other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public void Set(Vector2i other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public void Set(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
        }
        public bool IsZero()
        {
            return x == 0 && y == 0;
        }
       
        public void SetToZero()
        {
            this.x = this.y = 0;
        }

        public void SetToAbsoluteValues()
        {
            if (x < 0) x = -x;
            if (y < 0) y = -y;
        }

        public void Add(Vector2i other)
        {
            x += other.x;
            y += other.y;
        }

        public void Add(int x, int y)
        {
            this.x += x;
            this.y += y;
        }
        public void Add(int x, int y, int z)
        {
            this.x += x;
            this.y += y;
        }

        public void Subtract(Vector2i other)
        {
            x -= other.x;
            y -= other.y;
        }

        public void Scale(int scale)
        {
            this.x *= scale;
            this.y *= scale;
        }

        public void ScaleAdd(int scale, Vector2i other)
        {
            this.x += scale * other.x;
            this.y += scale * other.y;
        }

        public void Divide(int divisor)
        {
            this.x /= divisor;
            this.y /= divisor;
        }

        public void Avg(Vector2i other)
        {
            this.x = (this.x + other.x) / 2;
            this.y = (this.y + other.y) / 2;
        }

        /**
         * scalar product
         */
        public int Dot(Vector2i other)
        {
            return this.x * other.x + this.y * other.y;
        }
        
        public int LengthSquared()
        {
            return x * x + y * y;
        }
        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }
        public float Lengthf()
        {
            return (float)Math.Sqrt(LengthSquared());
        }
                
        public int DistanceSquared(Vector2i other)
        {
            int dx = other.x - x;
            int dy = other.y - y;
            return dx * dx + dy * dy;
        }

        public double Distance(Vector2i other)
        {
            return Math.Sqrt(DistanceSquared(other));
        }

        public float Distancef(Vector2i other)
        {
            return (float)Math.Sqrt(DistanceSquared(other));
        }


        public bool IsInCircleRadius(Vector2i other, int radius)
        {
            return DistanceSquared(other) <= radius * radius;
        }

        public Vector2i To90DegreeCcw()
        => new Vector2i(-y, x);
        public Vector2i To90DegreeCw()
        => new Vector2i(y, -x);
        public void Rotate90DegreeCcw()
        {
            int tmp = x;
            x = -y;
            y = tmp;
        }
        public void Rotate90DegreeCw()
        {
            int tmp = x;
            x = y;
            y = -tmp;
        }

        public int MaxComponent()
        {
            return x >= y ? x : y;
        }

        public int MinComponent()
        {
            return x <= y ? x : y;
        }

        public static int Side(Vector2i line, Vector2i p)
        {
            Vector2i perp = new Vector2i(-line.y, line.x);
            int d = p.Dot(perp);
            return Math.Sign(d);
        }
        
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Vector2i? v = obj as Vector2i?;
            if (v is null)
            {
                return false;
            }
            return this == v;
        }
        public bool Equals(Vector2i other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {

            return x.GetHashCode() ^ y.GetHashCode();
        }

        /**
         * indexing operator
         */
        public int this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    default: throw new IndexOutOfRangeException("Index " + i + " is out of " + GetType() + " Range");
                }
            }
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: throw new IndexOutOfRangeException("Index " + i + " is out of " + GetType() + " Range");
                }
            }
        }

        public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);

        public static Vector2i operator +(Vector2i a, int offset) => new Vector2i(a.x + offset, a.y + offset);

        public static Vector2i operator +(int offset, Vector2i a) => new Vector2i(offset + a.x, offset + a.y);

        public static Vector2i operator -(Vector2i a, Vector2i b) => new Vector2i(a.x - b.x, a.y - b.y);

        public static Vector2i operator -(Vector2i a, int offset) => new Vector2i(a.x - offset, a.y - offset);

        public static Vector2i operator -(int offset, Vector2i a) => new Vector2i(offset - a.x, offset - a.y);

        public static Vector2i operator *(Vector2i a, int scale) => new Vector2i(a.x * scale, a.y * scale);

        public static Vector2i operator *(int scale, Vector2i a) => a * scale;

        public static Vector2i operator /(Vector2i a, int scale) => new Vector2i(a.x / scale, a.y / scale);

        public static bool operator ==(Vector2i a, Vector2i b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Vector2i a, Vector2i b) => !( a == b );
        public static bool operator <(Vector2i a, Vector2i b) => a.LengthSquared() < b.LengthSquared();
        public static bool operator <=(Vector2i a, Vector2i b) => a.LengthSquared() <= b.LengthSquared();
        public static bool operator >(Vector2i a, Vector2i b) => a.LengthSquared() > b.LengthSquared();

        public static bool operator >=(Vector2i a, Vector2i b) => a.LengthSquared() >= b.LengthSquared();
        
        public override string ToString()
        => $"{GetType().Name} ( {x.ToString() }; {y.ToString()})";

        public static Vector2i Min(Vector2i v1, Vector2i v2)
        {
            return new Vector2i(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y));
        }
        public static Vector2i Max(Vector2i v1, Vector2i v2)
        {
            return new Vector2i(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y));
        }

        public static bool DoBoxesOverlap(Vector2i box1Min, Vector2i box1Max, Vector2i box2Min, Vector2i box2Max)
        {
            return
                !(box2Max.x <= box1Min.x || box2Min.x >= box1Max.x
                || box2Max.y <= box1Min.y || box2Min.y >= box1Max.y);
        }

        public static bool DoBoundsOverlap(Vector2i location1, Vector2i bounds1, Vector2i location2, Vector2i bounds2)
        {
            return !(Math.Abs(location1.x - location2.x) >= 0.5f * (bounds1.x + bounds2.x)
                  || Math.Abs(location1.y - location2.y) >= 0.5f * (bounds1.y + bounds2.y));
        }

        public readonly void Deconstruct(out int x, out int y) 
        {
            x = this.x;
            y = this.y;
        }
    }

    /**
     * vector of three ints with some vector functions (right handed), which can safely passed by value to a native library 
     * (if passed as pointer it must also be pinned in memory)
     */
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3i : IEquatable<Vector3i>
    {
        static Vector3i _zeros = new Vector3i(0, 0, 0);
        public static Vector3i ZEROS => _zeros;

        static Vector3i _ones = new Vector3i(1, 1, 1);
        public static Vector3i ONES => _ones;

        public int x, y, z;

        public Vector3i(int x = 0, int y = 0, int z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3i(Vector3i other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
        }

        public void Set(Vector3i other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
        }

        public void Set(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public bool IsZero()
        {
            return x == 0 && y == 0 && z == 0;
        }
        
        public void SetToZero()
        {
            this.x = this.y = this.z = 0;
        }

        public void SetToAbsoluteValues()
        {
            if (x < 0) x = -x;
            if (y < 0) y = -y;
            if (z < 0) z = -z;
        }

        public void Add(Vector3i other)
        {
            x += other.x;
            y += other.y;
            z += other.z;
        }

        public void Add(int x, int y)
        {
            this.x += x;
            this.y += y;
        }
        public void Add(int x, int y, int z)
        {
            this.x += x;
            this.y += y;
            this.z += z;
        }

        public void Subtract(Vector3i other)
        {
            x -= other.x;
            y -= other.y;
            z -= other.z;
        }

        public void Scale(int scale)
        {
            this.x *= scale;
            this.y *= scale;
            this.z *= scale;
        }

        public void ScaleAdd(int scale, Vector3i other)
        {
            this.x += scale * other.x;
            this.y += scale * other.y;
            this.z += scale * other.z;
        }

        public void Divide(int divisor)
        {
            this.x /= divisor;
            this.y /= divisor;
            this.z /= divisor;
        }

        public void Avg(Vector3i other)
        {
            this.x = (this.x + other.x) / 2;
            this.y = (this.y + other.y) / 2;
            this.z = (this.z + other.z) / 2;
        }

        /**
         * scalar product
         */
        public int Dot(Vector3i other)
        {
            return this.x * other.x + this.y * other.y + this.z * other.z;
        }

        /**
         * cross product stored in this vector
         */
        public void CrossWith(Vector3i other)
        {
            int nx = this.y * other.z - this.z * other.y;
            int ny = this.z * other.x - this.x * other.z;
            int nz = this.x * other.y - this.y * other.x;
            this.x = nx;
            this.y = ny;
            this.z = nz;
        }

        /**
        * cross product stored in new vector
        */
        public Vector3i Cross(Vector3i other)
        {
            return new Vector3i(
                this.y * other.z - this.z * other.y,
                this.z * other.x - this.x * other.z,
                this.x * other.y - this.y * other.x
            );
        }
        
      
        public readonly int LengthSquared()
        {
            return x * x + y * y + z * z;
        }
        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }
        /*
        public float Lengthf()
        {
            return UnityEngine.Mathf.Sqrt(LengthSquared());
        }
        */
        public int LengthSquared2D()
        {
            return x * x + y * y;
        }
        public double length2D()
        {
            return Math.Sqrt(LengthSquared());
        }
        /*
        public float Lengthf2D()
        {
            return UnityEngine.Mathf.Sqrt(LengthSquared());
        }
        */
        public int DistanceSquared(Vector3i other)
        {
            int dx = other.x - x;
            int dy = other.y - y;
            int dz = other.z - z;
            return dx * dx + dy * dy + dz * dz;
        }

        public int DistanceSquared2D(Vector3i other)
        {
            int dx = other.x - x;
            int dy = other.y - y;

            return dx * dx + dy * dy;
        }

        public double Distance(Vector3i other)
        {
            return Math.Sqrt(DistanceSquared(other));
        }
        /*
        public float Distancef(Vector3i other)
        {
            return UnityEngine.Mathf.Sqrt(DistanceSquared(other));
        }
        */
        public double Distance2D(Vector3i other)
        {
            return Math.Sqrt(DistanceSquared2D(other));
        }
        /*
        public float Distancef2D(Vector3i other)
        {
            return UnityEngine.Mathf.Sqrt(DistanceSquared2D(other));
        }
        */
        public bool IsInSphereRadius(Vector3i other, int radius)
        {
            return DistanceSquared(other) <= radius * radius;
        }

        public bool IsInCircleRadius(Vector3i other, int radius)
        {
            return DistanceSquared2D(other) <= radius * radius;
        }

        public int MaxComponent()
        {
            return x >= y ? (x >= z ? x : z) : (y >= z ? y : z);
        }
        public int MaxComponent2D()
        {
            return x >= y ? x : y;
        }

        public int MinComponent()
        {
            return x <= y ? (x <= z ? x : z) : (y <= z ? y : z);
        }
        public int MinComponent2D()
        {
            return x <= y ? x : y;
        }
/*
        public UnityEngine.Vector3 ToLefthanded(UnityEngine.Vector3 lhVector3)
        {
            lhVector3.x = x;
            lhVector3.y = z;
            lhVector3.z = y;
            return lhVector3;
        }
*/       
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Vector3i? v = obj as Vector3i?;
            if (v is null)
            {
                return false;
            }
            return this == v;
        }
        public bool Equals(Vector3i other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {

            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        /**
         * indexing operator
         */
        public int this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new IndexOutOfRangeException("Index " + i + " is out of " + GetType() + " Range");
                }
            }
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: throw new IndexOutOfRangeException("Index " + i + " is out of " + GetType() + " Range");
                }
            }
        }

        public static Vector3i operator +(in Vector3i a, in Vector3i b) => new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Vector3i operator +(Vector3i a, int offset) => new Vector3i(a.x + offset, a.y + offset, a.z + offset);

        public static Vector3i operator +(int offset, Vector3i a) => new Vector3i(offset + a.x, offset + a.y, offset + a.z);

        public static Vector3i operator -(in Vector3i a, in Vector3i b) => new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);

        public static Vector3i operator -(Vector3i a, int offset) => new Vector3i(a.x - offset, a.y - offset, a.z - offset);

        public static Vector3i operator -(int offset, Vector3i a) => new Vector3i(offset - a.x, offset - a.y, offset - a.z);

        public static Vector3i operator *(Vector3i a, int scale) => new Vector3i(a.x * scale, a.y * scale, a.z * scale);

        public static Vector3i operator *(int scale, Vector3i a) => a * scale;

        public static Vector3i operator /(Vector3i a, int scale) => new Vector3i(a.x / scale, a.y / scale, a.z / scale);

        /**
         * cross product
         */
        public static Vector3i operator *(Vector3i a, Vector3i b) => new Vector3i(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );


        public static bool operator ==(in Vector3i a, in Vector3i b) => a.x == b.x && a.y == b.y && a.z == b.z;
        public static bool operator !=(in Vector3i a, in Vector3i b) => !( a == b );
        public static bool operator <(in Vector3i a, in Vector3i b) => a.LengthSquared() < b.LengthSquared();
        public static bool operator <=(in Vector3i a,in Vector3i b) => a.LengthSquared() <= b.LengthSquared();
        public static bool operator >(in Vector3i a, in Vector3i b) => a.LengthSquared() > b.LengthSquared();

        public static bool operator >=(in Vector3i a, in Vector3i b) => a.LengthSquared() >= b.LengthSquared();

        public override string ToString()
        {
            return GetType().Name + "( " + x + " ; " + y + " ; " + z + " )";

        }

        public static Vector3i Min(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y), Math.Min(v1.z, v2.z));
        }
        public static Vector3i Max(Vector3i v1, Vector3i v2)
        {
            return new Vector3i(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y), Math.Max(v1.z, v2.z));
        }

        public static bool DoBoxesOverlap(Vector3i box1Min, Vector3i box1Max, Vector3i box2Min, Vector3i box2Max)
        {
            return
                !(box2Max.x <= box1Min.x || box2Min.x >= box1Max.x
                || box2Max.y <= box1Min.y || box2Min.y >= box1Max.y
                || box2Max.z <= box1Min.z || box2Min.z >= box1Max.z);
        }
        public static bool DoBoxesOverlap2D(Vector3i box1Min, Vector3i box1Max, Vector3i box2Min, Vector3i box2Max)
        {
            return
                !(box2Max.x <= box1Min.x || box2Min.x >= box1Max.x
                || box2Max.y <= box1Min.y || box2Min.y >= box1Max.y);
        }

        public static bool DoBoundsOverlap(Vector3i location1, Vector3i bounds1, Vector3i location2, Vector3i bounds2)
        {
            return !(Math.Abs(location1.x - location2.x) >= 0.5f * (bounds1.x + bounds2.x)
                  || Math.Abs(location1.y - location2.y) >= 0.5f * (bounds1.y + bounds2.y)
                  || Math.Abs(location1.z - location2.z) >= 0.5f * (bounds1.z + bounds2.z));
        }

        public static bool DoBoundsOverlap2D(Vector3i location1, Vector3i bounds1, Vector3i location2, Vector3i bounds2)
        {
            return !(Math.Abs(location1.x - location2.x) >= 0.5f * (bounds1.x + bounds2.x)
                  || Math.Abs(location1.y - location2.y) >= 0.5f * (bounds1.y + bounds2.y));
        }

        public readonly Vector2i ToVector2i => new Vector2i(x, y);

        public readonly void Deconstruct(out int x, out int y, out int z) 
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }
    }

    [Serializable]
    public class ArrayDeque<T> : IList<T>, IReadOnlyList<T>
    {
        int _begin,_end,_size,_capacity;
        T[] _ring;
        readonly IEqualityComparer<T> _comparer;

        public ArrayDeque() : this(0) { }
        public ArrayDeque(int initialCapacity) : this(initialCapacity, EqualityComparer<T>.Default) { }
        public ArrayDeque(IEqualityComparer<T> comparer) : this(16, comparer) { }
        public ArrayDeque(int initialCapacity, IEqualityComparer<T> comparer) {
		    _capacity = initialCapacity;
            _ring = _capacity == 0 ? Array.Empty<T>() : new T[_capacity];
            _begin = _end = _size = 0;
            _comparer = comparer;
	    }

        public ArrayDeque(ArrayDeque<T> other)
        {
            _capacity = other._capacity;
            _ring = new T[_capacity];
            Array.Copy(other._ring, _ring, other._size);
            _begin = other._begin;
            _end = other._end;
            _size = other._size;
            _comparer = other._comparer;
        }

        public T Get(int i) {
		    return i >= 0 && i < _size ? _ring[Index(i)] : default;
	    }

        public void Set(int i, in T value)
        {
            if (i >= 0 && i < _size) 
                _ring[Index(i)] = value;
        }

        public ref T GetRef(int i)
        {
            return ref _ring[Index(i >= 0 && i < _size ? i : _end)];
        }

        public T GetFirst() {
		    return _size == 0 ? default : _ring[_begin];
	    }
        public ref T GetFirstRef()
        {
            return ref _ring[_begin];
        }
        public T GetLast() {
            return _size == 0 ? default : _ring[Prev(_end)];
	    }
        public ref T GetLastRef()
        {
            return ref _ring[Prev(_end)];
        }

        public void Push(in T elem) { AddFirst(elem); }
        public T Pop() { return RemoveFirst(); }
        public T Peek() { return GetFirst(); }

        public void Enqueue(in T elem) { AddLast(elem); }
        public T Dequeue() { return RemoveFirst(); }

        //public void Add(in T elem){ AddLast(elem); }
        public void Add(T elem) { AddLast(elem); }

        public bool AddIfNotContained(T elem) { if (Contains(elem)) return false; AddLast(elem); return true; }

        public void AddLast(in T elem) {
		    EnsureCapacity();
		    _ring[_end] = elem;
            _end = Next(_end);
		    ++_size;	
	    }

	    public T RemoveLast() {
		    if (_size==0) return default;
            _end = Prev(_end);
		    --_size;		
		    return _ring[_end];
	    }

	    public void AddFirst(in T elem) {
		    EnsureCapacity();				
		    _begin = Prev(_begin);
            _ring[_begin] = elem;		    			
		    ++_size;	
	    }

	    public T RemoveFirst() {
		    if (_size==0) return default;		
		    var elem = _ring[_begin];
            _begin = Next(_begin);
		    --_size;		
		    return elem;
	    }

	    public void Clear() {
            _size = _begin = _end = 0;
	    }
        /*
        private bool isEqual(object elem1, object elem2) {
            return elem1 == elem2 || elem1 != null && elem1.Equals(elem2);
        }
        private bool isEqual(T elem1, T elem2)
        {
            //if (elem1 == elem2) return true;
            return elem1 == null && elem2 == null || elem1 != null && elem1.Equals(elem2);
        } 
        */
        /*
        public bool Contains(T elem)
        {
            return Contains(in elem);
        }
        */
        public bool Contains(T elem)
        {
            for (var i = _begin; i != _end; i = Next(i))
            {
                if (_comparer.Equals(_ring[i], elem))
                    return true;
            }
            return false;
        }

        /*
        public bool Remove(T elem)
        {
            return Remove(in elem);
        }*/
        public bool Remove(T elem)
        {
            for (var i = _begin; i != _end; i = Next(i))
            {
                if (_comparer.Equals(_ring[i], elem))
                {
                    _end = Prev(_end);
                    int j;
                    for (; i != _end; i = j)
                    {
                        j = Next(i);
                        _ring[i] = _ring[j];
                    }
                    --_size;
                    return true;
                }
            }
            return false;
        }
        
        public int FindIndex(Predicate<T> predicate)
        {
            for (var i = 0; i < _size; ++i)
            {
                if (predicate(_ring[Index(i)]))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool RemoveFirst(Predicate<T> predicate)
        {
            if (_size == 0) return false;

            for (var i = _begin; i != _end; i = Next(i))
            {
                if (predicate(_ring[i]))
                {
                    _end = Prev(_end);
                    int j;
                    for (; i != _end; i = j)
                    {
                        j = Next(i);
                        _ring[i] = _ring[j];
                    }
                    --_size;
                    return true;
                }
            }
            return false;
        }

        public int RemoveAll(Predicate<T> match)
        {
            if (_size == 0) return 0;

            var freeIndex = _begin;
            while (freeIndex != _end && !match(_ring[freeIndex])) freeIndex = Next(freeIndex);
            if (freeIndex == _end) return 0;

            var removedCount = 1;
            var current = Next(freeIndex);
            while (true)
            {
                while (current != _end && match(_ring[current]))
                {
                    current = Next(current);
                    removedCount++;
                }
                if (current != _end)
                {
                    _ring[freeIndex] = _ring[current];
                    freeIndex = Next(freeIndex);
                    current = Next(current);
                }
                else
                    break;
            }
            var r = removedCount;
            while (--r >= 0)
                _end = Prev(_end);
            _size -= removedCount;
            return removedCount;
        }

        /*     
        public T[] ToArray(T[] tofill = null)
        { 
            if (tofill == null)
                tofill = new T[size];
            for (int i = 0; i < size; ++i)
            { 
                tofill[i] = ring[index(i)];
            }
            return tofill;
        }
        */
        public int Count => _size;

        public bool IsEmpty() { return _size == 0; }

        public int Capacity => _capacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int Index(int i) => (_begin + i) % _capacity;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int Next(int i) => (i + 1) % _capacity;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int Prev(int i) => (i - 1 + _capacity) % _capacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity() {
            if (_size + 1 >= _capacity)
            { // size +1 to have always an unused element for the end pointer (alternative would need more case distinctions)
                Resize(_capacity == 0 ? 8 :  (3 * _capacity + 1) >> 1);               
		    }
	    }
        [MethodImpl(MethodImplOptions.NoInlining)]
        void Resize(int newCapacity)
        {
            var newRing = new T[newCapacity];
            if (_begin <= _end)
            {
                Array.Copy(_ring, _begin, newRing, 0, _size);
            }
            else
            {
                var rightIntervalCount = _capacity - _begin;
                Array.Copy(_ring, _begin, newRing, 0, rightIntervalCount);
                Array.Copy(_ring, 0, newRing, rightIntervalCount, _end);
            }
            _capacity = newCapacity;
            _ring = newRing;
            _begin = 0;
            _end = _size;
        }

        public void ResizeTo(int newCapacity)
        {
            Resize(Math.Max(newCapacity, _size));
        }

        /// Remaining Implementation of IList<T>
        /// ------------------------------------

        public bool IsReadOnly => false;

        public T this[int i]
        {
            get => Get(i);
            set => Set(i, value);
        }
        /*
        public ref T this[int i]
        {
            get { return ref GetRef(i); }
        }
        */
        public void AddRange(ArrayDeque<T> queue)
        {
            var cnt = queue.Count;
            for (var i = 0; i < cnt; ++i)
            {
                AddLast(queue.Get(i));
            }
        }
        public void AddRange(IEnumerable<T> source)
        {            
            foreach (var item in source)
            {
                AddLast(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array), "array is null");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex is less than 0.");
            if (arrayIndex + array.Length < _size) throw new ArgumentException("The number of elements in the source ICollection<T> is greater than the available space from arrayIndex to the end of the destination array.", nameof(arrayIndex));

            for (var i = 0; i < _size; ++i)
            {
                array[arrayIndex+i] = _ring[Index(i)];
            }
        }
        public int IndexOf(T item)
        {
            for (var i = 0; i < _size; ++i)
            {
                if (_comparer.Equals(_ring[Index(i)], item)) 
                    return i;
            }
            return -1;
        }

        public void Insert(int idx, T item)
        {
            Insert(idx, in item);
        }
        public void Insert(int idx, in T item)
        {
            if (idx < 0 || idx > _size) throw new ArgumentException("index is not a valid index in the IList<T>.", nameof(idx));

            EnsureCapacity(); // resize if needed
            var slot = Index(idx);
            int j;
            for (var i = _end; i != slot; i = j) 
            { // move all elements one slot backward
                j = Prev(i);
                _ring[i] = _ring[j];
            }
            _ring[slot] = item; // store item at idx
            _end = Next(_end);
            _size++;
        }
       
        public void RemoveAt(int idx)
        {
            if (idx < 0 || idx >= _size) throw new ArgumentException("index is not a valid index in the IList<T>.", nameof(idx));
            _end = Prev(_end);
            var j = Index(idx);
            for (var i = j; j != _end; i = j)
            { // move all elements one slot forward
                j = Next(i);
                _ring[i] = _ring[j];
            }
            _size--;        
        }

        public void RemoveAtBySwapEnd(int idx)
        {
            if (idx < 0 || idx >= _size) throw new ArgumentException("index is not a valid index in the IList<T>.", nameof(idx));
            
            _end = Prev(_end);
            _ring[Index(idx)] = _ring[_end];
            _size--;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()  => new Enumerator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);
        public Enumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<T>
        {
            ArrayDeque<T> _deque;
            int _index;
            public Enumerator(ArrayDeque<T> deque)
            {
                _deque = deque;
                _index = -1;
            }
            public T Current => _deque._ring[_deque.Index(_index)];
            object System.Collections.IEnumerator.Current => _deque._ring[_deque.Index(_index)];
            public void Dispose() {}
            public bool MoveNext() => ++_index < _deque.Count;
            public void Reset() => _index = -1;
        }

    }
}