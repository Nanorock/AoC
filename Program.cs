
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode_2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(13, false, ERunPart.Both);
            Console.ReadKey();
        }

        static void Run(int day, bool example, ERunPart part)
        {
            var aocType = Type.GetType($"AdventOfCode_2021.AoC{day}");
            if (aocType == null)
            {
                Console.WriteLine($"No aoc class for day {day}");
                return;
            }
            var aoc = Activator.CreateInstance(aocType) as AdventOfCode;
            if(example) aoc.SetInput(true);
            aoc.Start();
            if(part != ERunPart.Part2)
                aoc.Run1();
            if (part != ERunPart.Part1)
                aoc.Run2();
        }
    }

    enum ERunPart
    {
        Both,
        Part1,
        Part2
    }
    abstract class AdventOfCode
    {
        protected string[] inputFile;

        public AdventOfCode() { SetInput(); }
        public void SetInput(bool example = false)
        {
            string number = "";
            string name = GetType().Name;
            for (int i = 0; i < name.Length; i++)
                if (char.IsDigit(name[i]))
                    number += name[i];

            string file = $"C:/Users/Nanorock/source/repos/AoC2021/INPUT/aoc_{number}";
            if (example) file += "_ex";
            inputFile = File.ReadAllLines($"{file}.txt");
        }


        public virtual void Start() { }
        public virtual void Run1() { }
        public virtual void Run2() { }

        protected void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        protected void Wait()
        {
            Console.ReadKey();
        }
    }

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
                if(!hash.Remove(list[i]))
                    list.RemoveAt(i);

            hash.Clear();
        }
        public static void Intersect<T>(this List<T> list, IEnumerable<T> values)
        {
            var hash = HashCache<T>.Value;
            foreach(var v in values)
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
}
