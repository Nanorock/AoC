using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void Intersect<T>(this List<T> list, List<T> values)
        {
            var hash = HashCache<T>.Value;
            for (int i = 0; i < values.Count; i++)
                hash.Add(values[i]);

            for (int i = list.Count - 1; i >= 0; i--)
                if (!hash.Remove(list[i]))
                    list.RemoveAt(i);

            hash.Clear();
        }
        public static void Intersect<T>(this List<T> list, IEnumerable<T> values)
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
}