namespace AdventOfCode_2021;

class AoCReactorReboot22 : AdventOfCode
{
    public override void Run1()
    {
        HashSet<string> ons = new HashSet<string>();
        for (int i = 0; i < inputFile.Length; i++)
        {
            var input = inputFile[i];
            var command = ParseCommand(input);
            if (command.Cube.MinX < -50 || command.Cube.MaxX > 50 || 
                command.Cube.MinY < -50 || command.Cube.MaxY > 50 ||
                command.Cube.MinZ < -50 || command.Cube.MaxZ > 50)
                break;
            CommandProcess(command);
        }
        Console.WriteLine(CountCommands());
    }
    
    public override void Run2()
    {
        ulong cubeOn = 0;
        for (int i = 0; i < inputFile.Length; i++)
        {
            var input = inputFile[i];
            var command = ParseCommand(input);
            if (command.Cube.MinX < -50 || command.Cube.MaxX > 50 || 
                command.Cube.MinY < -50 || command.Cube.MaxY > 50 ||
                command.Cube.MinZ < -50 || command.Cube.MaxZ > 50)
            {
                CommandProcess(command);
            }
        }

        Console.WriteLine(CountCommands());
    }

    CommandCube ParseCommand(string input)
    {
        bool on = input[1] == 'n';//on
        int firstEqId = input.IndexOf('=')+1;
        int secondEqId = input.IndexOf('=',firstEqId)+1;
        int thirdEqId = input.IndexOf('=',secondEqId)+1;
        var xrangeStr = input.Substring(firstEqId, secondEqId - 3 - firstEqId);
        var yrangeStr = input.Substring(secondEqId, thirdEqId - 3 - secondEqId);
        var zrangeStr = input.Substring(thirdEqId);

        var xs = xrangeStr.Split("..").Select(int.Parse).ToArray();
        var ys = yrangeStr.Split("..").Select(int.Parse).ToArray();
        var zs = zrangeStr.Split("..").Select(int.Parse).ToArray();
        var cube = new Cube(xs[0], ys[0], zs[0], xs[1], ys[1], zs[1]);
        return new CommandCube(cube, on);
    }

    readonly List<CommandCube> _commands = new List<CommandCube>();
    readonly List<CommandCube> _overlaps = new List<CommandCube>();
    void CommandProcess(CommandCube command)
    {
        _overlaps.Clear();
        for (int j = 0; j < _commands.Count; j++)
        {
            var prevCommand = _commands[j];
            if (command.DoOverlaps(prevCommand))
            {
                var overlapCube = command.Overlaps(prevCommand);
                _overlaps.Add(new CommandCube(overlapCube, !prevCommand.On));
            }
        }
        _commands.AddRange(_overlaps);

        if(command.On)
            _commands.Add(command);
    }

    ulong CountCommands()
    {
        ulong cubeOn = 0;
        for (int i = 0; i < _commands.Count; i++)
        {
            var command = _commands[i];
            if (command.On)
                cubeOn += command.Count;
            else 
                cubeOn -= command.Count;
        }

        return cubeOn;
    }
    
    class CommandCube
    {
        public readonly Cube Cube;
        public readonly bool On;

        public ulong Count => Cube.Count;
        public CommandCube(in Cube cube, bool on)
        {
            Cube = cube;
            On = on;
        }
        
        public bool DoOverlaps(in CommandCube cube) => Cube.DoOverlaps(cube.Cube);
        public Cube Overlaps(in CommandCube cube) => Cube.Overlaps(cube.Cube);
    }
    
    readonly struct Cube
    {
        public readonly int MinX,MinY,MinZ;
        public readonly int MaxX,MaxY,MaxZ;
        public readonly ulong XRange, YRange, ZRange;
        public Cube(int minx, int miny, int minz, int maxx, int maxy, int maxz)
        {
            MinX = minx;
            MinY = miny;
            MinZ = minz;
            MaxX = maxx;
            MaxY = maxy;
            MaxZ = maxz;
            XRange = (ulong)(1 + MaxX - MinX);
            YRange = (ulong)(1 + MaxY - MinY);
            ZRange = (ulong)(1 + MaxZ - MinZ);
        }
        
        public ulong Count => XRange * YRange * ZRange;

        public bool DoOverlaps(in Cube cube) =>
            !(MaxX < cube.MinX || MinX > cube.MaxX 
           || MaxY < cube.MinY || MinY > cube.MaxY 
           || MaxZ < cube.MinZ || MinZ > cube.MaxZ);
        
        public Cube Overlaps(in Cube cube)
        {
            var minx = Math.Max(MinX, cube.MinX);
            var miny = Math.Max(MinY, cube.MinY);
            var minz = Math.Max(MinZ, cube.MinZ);
            var maxx = Math.Min(MaxX, cube.MaxX);
            var maxy = Math.Min(MaxY, cube.MaxY);
            var maxz = Math.Min(MaxZ, cube.MaxZ);
            return new Cube(minx, miny, minz,maxx, maxy, maxz);
        }
    }
}

