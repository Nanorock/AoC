using System;
using System.Collections.Generic;
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

    public static class ColorConsole
    {
        public static void PrintLine(string formatted)
        {
            if (string.IsNullOrEmpty(formatted)) return;
            int colorCodeStart = formatted.IndexOf('{');
            if (colorCodeStart < 0)
            {
                PrintDirectLine(formatted);
                return;
            }
            int colorCodeEnd = formatted.IndexOf('}');
            if (colorCodeEnd < 0)
            {
                PrintDirectLine(formatted);
                return;
            }

            while (colorCodeStart >= 0 && colorCodeEnd >= 0)
            {
                var colorStr = formatted.Substring(colorCodeStart+1, colorCodeEnd - colorCodeStart - 1);
                Console.ForegroundColor = StrColors[colorStr];
                formatted = formatted.Substring(colorCodeEnd + 1);
                
                colorCodeStart = formatted.IndexOf('{');
                colorCodeEnd = formatted.IndexOf('}');
                PrintDirect(colorCodeStart < 0 ? formatted : formatted.Substring(0, colorCodeStart));
            }
            Console.WriteLine();
            Console.ResetColor();
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