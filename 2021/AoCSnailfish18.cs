using System.Runtime.CompilerServices;
using AdventOfCodes;

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
                var addition = _numbers[i] + _numbers[j];
                var magnitude = addition.GetMagnitude();
                if (magnitude > maxMagnitude)
                    maxMagnitude = magnitude;
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


    class Number
    {
        int _value;
        
        Number? _x;
        Number? _y;
        
        bool _isAtomic;
        void SetAtomic()
        {
            _isAtomic = true;
            _isValue = false;
        }

        bool _isValue;
        void SetValue(int val)
        {
            _x = _y = null;
            _value = val;
            _isValue = true;
            _isAtomic = false;
            if (_parent._x._isValue && _parent._y._isValue)
                _parent._isAtomic = true;
        }

        Number? _parent;
        int _depth;
        
        #region Constructor
        Number(){}
        Number(int value, Number parent, int depth)
        {
            _value = value;
            _parent = parent;
            _isValue = true;
            _depth = depth;
        }
        public Number(string parse, Number parent = null)
        {
            bool isValue = parse[0] != '[';
            if (!isValue) ParsePair(parse, parent);
            else ParseNumber(parse);
            _parent = parent;
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
            _x = new Number(content[..comma],this);
            _y = new Number(content[(comma+1)..],this);
            _isAtomic = _x._isValue && _y._isValue;
            _parent = parent;
        }
        void ParseNumber(string parse)
        {
            _value = int.Parse(parse);
            _isValue = true;
        }

        Number(Number a, Number b)
        {
            _x = a;
            _y = b;
            _x._parent = _y._parent = this;
        }
        #endregion

        public static Number operator +(Number a, Number b)
        {
            Number result = new Number(a.Clone(), b.Clone());
            result.Reduce();
            return result;
        }

        static Number _reducing;
        void Reduce()
        {
            _reducing = this;
            ScanDepth();
            while (CheckExplosion() || CheckSplit())
                ;
            _reducing = null;
        }

        bool CheckExplosion()
        {
            UpdateAllExplode();
            for (int i = 0; i < _allExpsCount; i++)
                _allExps[i].Explode();
            return _allExpsCount > 0;
        }
        void Explode()
        {
            UpdateAllValues();
            int myX = -1;
            for (int i = 0; i < _allValuesCount; i++)
            {
                if (_allValues[i] == _x)
                {
                    myX = i;
                    break;
                }
            }
            int myY = myX + 1;
            if (myX > 0) _allValues[myX - 1]._value += _x._value;
            if (myY < _allValuesCount - 1) _allValues[myY + 1]._value += _y._value;
            SetValue(0);
        }

        bool CheckSplit()
        {
            var splitter = Find(n => n._isValue && n._value > 9);
            splitter?.Split();
            return splitter != null;
        }
        void Split()
        {
            _x = new Number((int)Math.Floor(_value * 0.5f)  , this,_depth + 1);
            _y = new Number((int)Math.Ceiling(_value * 0.5f), this,_depth + 1);
            SetAtomic();
        }

        static readonly Number[] _allValues = new Number[256];
        static int _allValuesCount;
        void UpdateAllValues()
        {
            _allValuesCount = -1;
            _reducing.UpdateValues();
            ++_allValuesCount;
        }
        void UpdateValues()
        {
            if (_isValue)
                _allValues[++_allValuesCount] = this;
            else
            {
                _x.UpdateValues();
                _y.UpdateValues();
            }
        }

        static readonly Number[] _allExps = new Number[256];
        static int _allExpsCount;
        void UpdateAllExplode()
        {
            _allExpsCount = -1;
            _reducing.UpdateExplode();
            ++_allExpsCount;
        }
        void UpdateExplode()
        {
            if(_isAtomic && _depth >= 4) 
                _allExps[++_allExpsCount] = this;
            else if(!_isValue)            
            {
                _x.UpdateExplode();
                _y.UpdateExplode();
            }
        }


        Number? Find(Func<Number,bool> filter)
        {
            if(filter(this)) return this;
            return !_isValue ? _x.Find(filter) ?? _y.Find(filter) : null;
        }
        
        public void ScanDepth(int depth = 0)
        {
            _depth = depth;
            _x?.ScanDepth(depth+1);
            _y?.ScanDepth(depth+1);
        }
        public ulong GetMagnitude()
        {
            if (_isValue) return (ulong)_value;
            return 3ul * _x.GetMagnitude() + 2ul * _y.GetMagnitude();
        }

        public override string ToString() => _isValue ? _value.ToString(): $"[{_x},{_y}]";


        Number Clone()
        {
            var number = SubClone();
            number.SetParent();
            return number;
        }
        Number SubClone()
        {
            var number = new Number {
                _depth = _depth,
                _value = _value,
                _isAtomic = _isAtomic,
                _isValue = _isValue
            };
            if (!number._isValue)
            {
                number._x = _x.Clone();
                number._y = _y.Clone();
            }
            return number;
        }
        void SetParent()
        {
            if (_isValue) return;
            _x._parent = this;
            _x.SetParent();
            _y._parent = this;
            _y.SetParent();
        }
    }
}