using System.Numerics;

namespace AdventOfCode_2021;

class AoCBeaconScanner19 : AdventOfCode
{
    List<Scanner> _scanners = new List<Scanner>();
    public override void Init()
    {
        Scanner? lastScanner = null;
        for (int i = 0; i < inputFile.Length; i++)
        {
            var line = inputFile[i];
            if (string.IsNullOrEmpty(line)) continue;
            if (line[1] == '-')
            {
                if (lastScanner != null)
                    _scanners.Add(lastScanner);
                lastScanner = new Scanner(_scanners.Count);
                continue;
            }

            var vi = new Vector3i();
            var elts = line.Split(',').Select(int.Parse);
            int id = -1;
            foreach (var e in elts)
                vi[++id] = e;
            lastScanner.AddBeacon(vi);
        }

        if (lastScanner != null)
            _scanners.Add(lastScanner);
    }
    
    public override void Run1()
    {
        bool haveOverlaps = true;
        while (haveOverlaps)
        {
            haveOverlaps = false;
            for (int j = 1; j < _scanners.Count; j++)
                if (_scanners[0].MergeOverlaps(_scanners[j]))
                    haveOverlaps = true;
        }
        
        Console.WriteLine(_scanners[0].Beacons.Length);
    }

    public override void Run2()
    {
        int largestDistance = 0;
        for (int i = 0; i < _scanners.Count; i++)
        for (int j = i + 1; j < _scanners.Count; j++)
        {
            int d = ManhattanDistance(_scanners[i].Offset, _scanners[j].Offset);
            if(d > largestDistance)
                largestDistance = d;
        }
        Console.WriteLine(largestDistance);
    }

    int ManhattanDistance(Vector3i a, Vector3i b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
}

class Scanner
{
    string _name;
    public override string ToString() => _name;
    static int _ids;
    public Scanner(int id)
    {
        _name = "Scanner " + id;
        if (id > _ids) _ids = id;
    }
    public Vector3i[] Beacons = Array.Empty<Vector3i>();

    public void AddBeacon(in Vector3i beacon)
    {
        Array.Resize(ref Beacons, Beacons.Length+1);
        Beacons[^1] = beacon;
    }

    
    static int RotationCount = 24;
    static Vector3i Rotate(in Vector3i v, int permutation)
    {
        var rotated = permutation switch
        {
             0 => ( v.x,  v.y,  v.z),  1 => ( v.y,  v.z,  v.x),   2 => (-v.y,  v.x,  v.z),  3 => (-v.x, -v.y,  v.z),
             4 => ( v.y, -v.x,  v.z),  5 => ( v.z,  v.y, -v.x),   6 => ( v.z,  v.x,  v.y),  7 => ( v.z, -v.y,  v.x),
             8 => ( v.z, -v.x, -v.y),  9 => (-v.x,  v.y, -v.z),  10 => ( v.y,  v.x, -v.z), 11 => ( v.x, -v.y, -v.z),
            12 => (-v.y, -v.x, -v.z), 13 => (-v.z,  v.y,  v.x),  14 => (-v.z,  v.x, -v.y), 15 => (-v.z, -v.y, -v.x),
            16 => (-v.z, -v.x,  v.y), 17 => ( v.x, -v.z,  v.y),  18 => (-v.y, -v.z,  v.x), 19 => (-v.x, -v.z, -v.y),
            20 => ( v.y, -v.z, -v.x), 21 => ( v.x,  v.z, -v.y),  22 => (-v.y,  v.z, -v.x), 23 => (-v.x,  v.z,  v.y),
        };
        return new Vector3i(rotated.Item1, rotated.Item2, rotated.Item3);
    }
    
    Vector3i[] _mineTransformed = Array.Empty<Vector3i>();
    Vector3i[] _otherRotated    = Array.Empty<Vector3i>();
    Vector3i[] _otherTranslated = Array.Empty<Vector3i>();
    
    Vector3i _offset;
    public Vector3i Offset => _offset;

    public bool Merged;

    static HashSet<Vector3i> _intersects = new HashSet<Vector3i>();

    public bool MergeOverlaps(Scanner otherScanner)
    {
        if (otherScanner.Merged) 
            return false;

        var otherBeacons = otherScanner.Beacons;
        
        if(_mineTransformed.Length != Beacons.Length)
            Array.Resize(ref _mineTransformed, Beacons.Length);
        if (_otherTranslated.Length != otherBeacons.Length)
        {
            Array.Resize(ref _otherTranslated, otherBeacons.Length);
            Array.Resize(ref _otherRotated, otherBeacons.Length);
        }
        
        for (int rotationId = 0; rotationId < RotationCount; rotationId++)
        {
            for (int r = 0; r < otherBeacons.Length; r++)
                _otherRotated[r] = Rotate(otherBeacons[r], rotationId);

            for (int o = 0; o < otherBeacons.Length; o++)
            {
                for (int b = 0; b < Beacons.Length; b++)
                {
                    var myBeacon = Beacons[b];

                    for (int s = 0; s < otherBeacons.Length; s++)
                    {
                        var offset = myBeacon - _otherRotated[s];

                        _intersects.Clear();
                        for (int t = 0; t < otherBeacons.Length; t++)
                        {
                            var translated = _otherRotated[t] + offset;
                            _otherTranslated[t] = translated;
                            _intersects.Add(translated);
                        }

                        //Intersection
                        int startCount = _intersects.Count;
                        for (int b2 = 0; b2 < Beacons.Length; b2++)
                            _intersects.Remove(Beacons[b2]);
                        int intersectingCount = startCount - _intersects.Count;
                        
                        if (intersectingCount >= 12)
                        {
                            foreach(var translated in _intersects)
                                AddBeacon(translated);
                            otherScanner.Merged = true;
                            otherScanner._offset = offset;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}