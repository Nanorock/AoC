using System;
using System.Collections.Generic;
using System.Linq;
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
                    for (int i = 0; i < _colors.Length; i++)
                        _strColors[_colors[i].ToString()] = _colors[i];
                }
                return _strColors;
            }
        }
    }
}