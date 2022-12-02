using AdventOfCodes;

namespace AdventOfCode_2021;

//I totally stole this code online.
//I'm so disappointed that one had to read the input to figure out the solution
//I was excited to have to code an ALU and use it to solve the challenge :(

class AoCArithmeticLogicUnit24 : AdventOfCode
{
    readonly List<long> addX = new();
    readonly List<long> divZ = new();
    readonly List<long> addY = new();
    readonly List<long> MaxZAtStep = new();
    readonly Dictionary<(int groupNum, long prevZ), List<string>> cacheDic = new();
    List<string> ValidModelNumbers;

    public override void Init()
    {
        for (int i = 0; i < 14; i++)
        {
            divZ.Add(int.Parse(inputFile[(18 * i) + 4].Split()[2]));
            addX.Add(int.Parse(inputFile[(18 * i) + 5].Split()[2]));
            addY.Add(int.Parse(inputFile[(18 * i) + 15].Split()[2]));
        }

        //We can only divide by 26 so many times at each step, at some point we can bail early. 
        for (int i = 0; i < divZ.Count; i++)
        {
            MaxZAtStep.Add(divZ.Skip(i).Aggregate(1L, (a, b) => a * b));
        }
        ValidModelNumbers = RecursiveSearch(0, 0).ToList();
        ValidModelNumbers.Sort();
        base.Init();
    }

    public override void Run1()
    {
        WriteLine($"Total Valid IDs: {ValidModelNumbers.Count}");
        WriteLine($"Max: {ValidModelNumbers.Last()}");
    }

    public override void Run2()
    {
        WriteLine($"Min: {ValidModelNumbers.First()}");
    }

    long RunGroup(int groupNum, long prevZ, long input)
    {
        long z = prevZ;
        long x = addX[groupNum] + z % 26;
        z /= divZ[groupNum];
        if (x != input)
        {
            z *= 26;
            z += input + addY[groupNum];
        }

        return z;
    }

    List<string> RecursiveSearch(int groupNum, long prevZ)
    {
        //We've Been here before...
        if (cacheDic.ContainsKey((groupNum, prevZ))) return cacheDic[(groupNum, prevZ)];
        //We've gon past the end
        if (groupNum >= 14)
        {
            if (prevZ == 0) return new() { "" };
            return null;
        }

        if (prevZ > MaxZAtStep[groupNum])
        {
            return null;
        }

        List<string> res = new();

        long nextX = addX[groupNum] + prevZ % 26;

        List<string> nextStrings;
        long nextZ;
        if (0 < nextX && nextX < 10)
        {
            nextZ = RunGroup(groupNum, prevZ, nextX);
            nextStrings = RecursiveSearch(groupNum + 1, nextZ);
            if (null != nextStrings)
            {
                foreach (var s in nextStrings)
                {
                    res.Add($"{nextX}{s}");
                }
            }
        }
        else
        {
            foreach (int i in Enumerable.Range(1, 9))
            {
                nextZ = RunGroup(groupNum, prevZ, i);
                nextStrings = RecursiveSearch(groupNum + 1, nextZ);

                if (null != nextStrings)
                {
                    foreach (var s in nextStrings)
                    {
                        res.Add($"{i}{s}");
                    }
                }
            }
        }
        cacheDic[(groupNum, prevZ)] = res;
        return res;
    }
}

/*class AoCArithmeticLogicUnit24 : AdventOfCode
{

    int[] _input = new int[14];
    int[] _maxValue = new int[14];
    
    public override void Run1()
    {
        var monad = new MONAD(inputFile);
        //monad.Valid(new[] { 7 });
        Console.WriteLine("Welcome to MONAD");
        
        for (int i = 0; i < _input.Length; i++)
            _input[i] = 9;

        _maxValue = _input.ToArray();
        bool islastNumber = false;
        while (true)
        {
            bool correct = monad.Valid(_input);
            if (correct)
            {
                _maxValue = _input.ToArray();
                break;
            }
            for (int i = _input.Length - 1; i >= 0; i--)
            {
                int digit = _input[i];
                if (digit > 1)
                {
                    --_input[i];
                    break;
                }
                _input[i] = 9;
            }
        }
        Console.WriteLine("Highest serial is:" + new string(_maxValue.Select(v => (char)('0'+v)).ToArray()));
    }
    
    class MONAD
    {
        readonly int[][] _compiled;
        readonly long[] _variables = new long[5];
        
        public MONAD(string[] program)
        {
            _compiled = new int[program.Length][];
            for (int i = 0;i < program.Length;i++)
            {
                var line = program[i];
                var instructions = line.Split(' ');
                _compiled[i] = new int[4];
                _compiled[i][0] = GetInstructionId(instructions[0]);
                _compiled[i][1] = GetVarIndex(instructions[1]);
                if (instructions.Length <= 2) continue;
                _compiled[i][2] = GetVarIndex(instructions[2], out var constant);
                if (constant) _compiled[i][3] = int.Parse(instructions[2]);
            }
        }
        
        int[] _numbers;
        int _inputIndex=-1;
        public bool Valid(int[] numbers)
        {
            _numbers = numbers;
            Run();
            return _variables[4] == 0;
        }
        
        void Run()
        {
            _inputIndex=-1;
            for (int i = 0; i < _variables.Length; i++)
                _variables[i] = 0;

            for (int i = 0; i < _compiled.Length; i++)
            {
                var op = _compiled[i];
                _variables[_constant] = op[3];
                switch (op[0])
                {
                    case 0: _variables[op[1]] = _numbers[++_inputIndex]; break;
                    case 1: _variables[op[1]] += _variables[op[2]]; break;
                    case 2: _variables[op[1]] *= _variables[op[2]]; break;
                    case 3: _variables[op[1]] /= _variables[op[2]]; break;
                    case 4: _variables[op[1]] %= _variables[op[2]]; break;
                    case 5: _variables[op[1]] = _variables[op[1]] == _variables[op[2]] ? 1 : 0; break;
                }
            }
        }
    }
    static byte GetInstructionId(string instruction)
    {
        return instruction switch
        {
            "inp" => 0,
            "add" => 1,
            "mul" => 2,
            "div" => 3,
            "mod" => 4,
            "eql" => 5,
            _ => throw new Exception("unknown instruction " + instruction)
        };
    }

    const int _constant = 0;
    static byte GetVarIndex(string c) => GetVarIndex(c, out _);
    static byte GetVarIndex(string c, out bool constant)
    {
        constant = false;
        if (c.Length <= 1 && !char.IsDigit(c[0]))
            return (byte)(1 + c[0] - 'w');
        constant = true;
        return _constant;
    }
}*/
