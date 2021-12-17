namespace AdventOfCode_2021;

class AoCBallistic17 : AdventOfCode
{
    int minX, minY, maxX, maxY;
    public override void Init()
    {
        var input = inputFile[0];
        int start = input.IndexOf("=")+1;
        int end = input.IndexOf(",");
        var xStr = input.Substring(start, end - start);
        var xs = xStr.Split('.', StringSplitOptions.RemoveEmptyEntries);
        minX = int.Parse(xs[0]);
        maxX = int.Parse(xs[1]);
                var yStr = input.Substring(start+12);
        var ys = yStr.Split('.', StringSplitOptions.RemoveEmptyEntries);
        minY = int.Parse(ys[0]);    
        maxY = int.Parse(ys[1]);
    }

    class Probe 
    { 
        public Vector2i pos;
        public Vector2i vel;

        public Probe(Vector2i velocity)
        {
            vel = velocity;
        }

        public void Step()
        {
            pos.x += vel.x;
            pos.y += vel.y;
            vel.x -= Math.Sign(vel.x);
            //vel.y -= Math.Sign(vel.y);
            --vel.y;
        }
    }

    
    int RevFactorial(int x)
    {
        int revFactorial = 0;
        while (x >= revFactorial)
            x -= ++revFactorial;
        return revFactorial;
    }

    public override void Run1()
    {
        //Run1_v0();//Previous version, until I heard about triangular number : https://en.wikipedia.org/wiki/Triangular_number
        WriteLine($"Correct shot with max y {(minY*(minY+1)/2)}");
    }
    public void Run1_v0()
    {
        //To allow greater Y, we need x that stop within the area. X decrease by 1 every tick.
        int shortestX = RevFactorial(minX);
        int furthestX = RevFactorial(maxX);
        
        Vector2i highest = default;
        int maxY = -1;
        for (int vx = shortestX; vx <= furthestX; vx++)
        {
            for (int vy = minY; vy <= 1-minY; vy++)
            {
                if (LaunchProbe(new Vector2i(vx, vy), out int probeMaxY) && probeMaxY > maxY)
                {
                    highest = new Vector2i(vx, vy);
                    maxY = probeMaxY;
                }
            }
        }
        WriteLine($"Correct shot with max y {maxY} with vel {highest}");
    }
    
    //Just for fun
    void Plot(Vector2i vel)
    {
        int Y(int y) => y + Math.Abs(minY);

        var maxAboveGround = (minY * (minY + 1) / 2);
        var yHeight = Y(maxAboveGround);
        
        BaseTilemap<char> tm = new BaseTilemap<char>(maxX, yHeight);
        for (int x = minX; x <= maxX; x++)
        for (int y = Y(minY); y <= Y(maxY); y++)
            tm.Set(x,y,'O');
        for (int x = 0; x <= maxX; x++)
            tm.Set(x,Y(0),'-');
        tm.Set(0,Y(0),'S');
        
        var probe = new Probe(vel);
        Vector2i lastPlot = new Vector2i(0, Y(0));;
        List<int> plotLine = new List<int>();
        while (!OverReach(probe))
        {
            probe.Step();
            tm.BresenhamLine(lastPlot.x,lastPlot.y,probe.pos.x,Y(probe.pos.y),plotLine);
            for (int i = 0; i < plotLine.Count; i++)
                tm.Set(plotLine[i], '#');
            lastPlot = new Vector2i(probe.pos.x, Y(probe.pos.y));

            if (IsWithinArea(probe))
                break;
        }
        var printer = tm.GetPrinter((i, c) => c == default ? " " : c == '#' ? "<Red>#<>" : ""+c);
        foreach(var line in printer.PrintStateLines(true))
            ColorConsole.PrintLine(line);
    }

    public override void Run2()
    {
        int count = 0;
        for (int vx = 1; vx <= maxX; vx++)
        for (int vy = minY; vy <= -minY; vy++)
            if (LaunchProbe(new Vector2i(vx, vy), out _))
                ++count;
        WriteLine($"Successful shots {count}");
    }
    //152 too low ??

    bool LaunchProbe(Vector2i vel, out int maxShotY)
    {
        maxShotY = 0;
        var probe = new Probe(vel);
        while (!OverReach(probe))
        {
            probe.Step();
            if (probe.pos.y > maxShotY)
                maxShotY = probe.pos.y;
            if (IsWithinArea(probe))
                return true;
        }
        return false;
    }
    
    bool IsWithinArea(Probe p) => p.pos.x >= minX && p.pos.x <= maxX && p.pos.y >= minY && p.pos.y <= maxY;
    bool OverReach(Probe p) => p.pos.x > maxX || p.pos.y < minY;

}
