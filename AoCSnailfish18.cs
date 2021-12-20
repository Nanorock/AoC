using System.Runtime.CompilerServices;

namespace AdventOfCode_2021;

class AoCSnailfish18 : AdventOfCode
{
    public override void Run1()
    {
        ParseNumbersFromFile();
        base.Run1();
        var final = _numbers[0];
        for (int i = 1; i < _numbers.Length; i++)
            final += _numbers[i];
        Console.WriteLine(final);
        Console.WriteLine($"With magnitude {final.GetMagnitude()}");
    }

    public override void Run2()
    {
        ParseNumbersFromFile();
        ulong maxMagnitude = ulong.MinValue;
        for (int i = 0; i < _numbers.Length; i++)
        {
            for (int j = 0; j < _numbers.Length; j++)
            {
                if (i == j) continue;
                var n1 = _numbers[i];
                var n2 = _numbers[j];
                var addition = n1 + n2;
                var magnitude = addition.GetMagnitude();
                if (magnitude > maxMagnitude)
                    maxMagnitude = magnitude;
                ParseNumberFromFile(i);
                ParseNumberFromFile(j);
            }
        }
        Console.WriteLine($"Highest magnitude {maxMagnitude}");
    }


    Number[] _numbers;
    void ParseNumbersFromFile()
    {
        _numbers = new Number[inputFile.Length];
        for (int i = 0; i < inputFile.Length; i++)
            _numbers[i] = new Number(inputFile[i]);
    }
    void ParseNumberFromFile(int i)
    {
        _numbers[i] = new Number(inputFile[i]);
    }


    class Number
    {
        public int Value;
        public bool IsValue => X == null && Y == null;

        public Number? X;
        public Number? Y;
        public bool IsAtomic => !IsValue && X.IsValue && Y.IsValue;

        public Number? Parent;
        public int Depth;
        
        #region Constructor
        Number(){}
        public Number(int value, Number parent)
        {
            Value = value;
            Parent = parent;
        }
        public Number(string parse, Number parent = null)
        {
            bool isValue = parse[0] != '[';
            if (!isValue) ParsePair(parse, parent);
            else ParseNumber(parse);
            Parent = parent;
        }

        void ParsePair(string parse, Number parent = null)
        {
            string content = parse.Substring(1, parse.Length - 2);//Remove []
            int openElements=0;
            int comma=-1;
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '[') ++openElements;
                else if (content[i] == ']') --openElements;
                else if (content[i] == ',' && openElements == 0)
                {
                    comma = i;
                    break;
                }
            }
            X = new Number(content[..comma],this);
            Y = new Number(content[(comma+1)..],this);
            Parent = parent;
        }
        void ParseNumber(string parse)
        {
            Value = int.Parse(parse);
        }

        public Number(Number a, Number b)
        {
            X = a;
            Y = b;
            X.Parent = Y.Parent = this;
        }
        #endregion

        public static Number operator +(Number a, Number b)
        {
            Number result = new Number(a, b);
            result.Reduce();
            return result;
        }
        
        public void Reduce()
        {
            ScanDepth();
            bool changed = true;
            while (changed)
            {
                changed = CheckExplosion();
                while (changed)
                    changed = CheckExplosion();

                changed = CheckSplit();
            }
        }

        bool CheckExplosion()
        {
            var exploder = Find(n => n.IsAtomic && n.Depth >= 4);
            return exploder?.Explode()??false;
        }
        bool Explode()
        {
            if (!IsAtomic || Depth < 4) return false;
            var values = GetValues();
            int myX = values.FindIndex((n) => n == X);
            if (myX > 0) values[myX - 1].Value += X.Value;
            int myY = values.FindIndex((n) => n == Y);
            if (myY < values.Count - 1) values[myY + 1].Value += Y.Value;
            
            X = Y = null;
            Value = 0;
            return true;
        }

        bool CheckSplit()
        {
            var splitter = Find(n => n.IsValue && n.Value > 9);
            return splitter?.Split() ?? false;
        }
        bool Split()
        {
            if (!IsValue || Value <= 9) return false;
            X = new Number((int)Math.Floor(Value * 0.5f), this);
            X.Depth = Depth + 1;
            Y = new Number((int)Math.Ceiling(Value * 0.5f), this);
            Y.Depth = Depth + 1;
            return true;
        }

        static readonly List<Number> _values = new List<Number>();
        List<Number> GetValues() { _values.Clear(); GetTopNumber().GetValues(_values); return _values; }
        void GetValues(List<Number> result)
        {
            if(IsValue) result.Add(this);
            else if(!IsValue)            
            {
                X.GetValues(result);
                Y.GetValues(result);
            }
        }
        Number? Find(Func<Number,bool> filter)
        {
            if(filter(this)) return this;
            return !IsValue ? X.Find(filter) ?? Y.Find(filter) : null;
        }
        
        public void ScanDepth(int depth = 0)
        {
            Depth = depth;
            X?.ScanDepth(depth+1);
            Y?.ScanDepth(depth+1);
        }

        Number GetTopNumber()
        {
            if (Parent == null) return this;
            var parent = Parent;
            while (parent.Parent != null)
                parent = parent.Parent;
            return parent;
        }

        public ulong GetMagnitude()
        {
            if (IsValue) return (ulong)Value;
            return 3ul * X.GetMagnitude() + 2ul * Y.GetMagnitude();
        }

        public override string ToString() => IsValue ? Value.ToString(): $"[{X},{Y}]";
    }
}