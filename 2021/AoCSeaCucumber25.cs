using AdventOfCodes;

namespace AdventOfCode_2021;

class AoCSeaCucumber25 : AdventOfCode
{
    const char EMPTY = '.', EAST = '>', SOUTH = 'v';
    public override void Run1()
    {
        BaseTilemap<char> tm = new BaseTilemap<char>(inputFile, c => c);
        /*var printer = tm.GetPrinter((id,c) => "" + c);
        WriteLine(printer.PrintState());*/
        int step = 1;
        while (ProgressBoard(tm))
        {
            ++step;
            /*WriteLine("Step " + step);
            WriteLine(printer.PrintState());*/
        }
        WriteLine($"Stopped at step {step}");
    }
    //471

    HashSet<int> _moved = new HashSet<int>();
    bool ProgressBoard(BaseTilemap<char> tilemap) => EastStep(tilemap) | SouthStep(tilemap);
    bool EastStep(BaseTilemap<char> tileMap)
    {
        _moved.Clear();
        for (int y = 0; y < tileMap.Height; y++)
        for (int x = 0; x < tileMap.Width; x++)
        {
            int seaCell = tileMap.Get(x, y);
            if (seaCell == EAST)
            {
                int id = tileMap.GetId(x, y);
                if (_moved.Contains(id))
                    continue;
                int nx = (x + 1) % tileMap.Width;
                var eastSeaCell = tileMap.Get(nx, y);
                if (eastSeaCell == EMPTY && _moved.Add(tileMap.GetId(nx, y)))
                {
                    tileMap.Set(x, y, EMPTY);
                    tileMap.Set(nx, y, EAST);
                    _moved.Add(id);
                }
            }
        }
        return _moved.Count > 0;
    }
    bool SouthStep(BaseTilemap<char> tileMap)
    {
        _moved.Clear();
        for (int y = 0; y < tileMap.Height; y++)
        for (int x = 0; x < tileMap.Width; x++)
        {
            int seaCell = tileMap.Get(x, y);
            if (seaCell == SOUTH)
            {
                int id = tileMap.GetId(x, y);
                if (_moved.Contains(id))
                    continue;

                int ny = (y + 1) % tileMap.Height;
                var southSeaCell = tileMap.Get(x, ny);
                if (southSeaCell == EMPTY && _moved.Add(tileMap.GetId(x, ny)))
                {
                    tileMap.Set(x, y, EMPTY);
                    tileMap.Set(x, ny, SOUTH);
                    _moved.Add(id);
                }
            }
        }
        return _moved.Count > 0;
    }

}