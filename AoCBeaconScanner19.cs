using System.Numerics;

namespace AdventOfCode_2021;

class AoCBeaconScanner19 : AdventOfCode
{
    List<Scanner> ParseInput()
    {
        List<Scanner> scanners = new List<Scanner>();
        List<Vector3i> beacons = new List<Vector3i>();
        for (int i = 1; i < inputFile.Length; i++)
        {
            var line = inputFile[i];
            if (string.IsNullOrEmpty(line)) continue;
            if (line[1] == '-')
            {
                scanners.Add(new Scanner(default,0,beacons));
                beacons = new List<Vector3i>();
                continue;
            }
            
            var elts = line.Split(',').Select(int.Parse).ToArray();
            beacons.Add(new Vector3i(elts[0],elts[1],elts[2]));
        }

        scanners.Add(new Scanner(default,0,beacons));
        return scanners;
    }

    List<Scanner> _locatedScans = new List<Scanner>();
    public override void Run1()
    {
        _locatedScans = LocateScans();
        HashSet<Vector3i> uniqueBeacons = new HashSet<Vector3i>();
        for (int i = 0; i < _locatedScans.Count; i++)
        {
            var beacons = _locatedScans[i].GetBeaconsInWorld();
            for (int j = 0; j < beacons.Count; j++)
                uniqueBeacons.Add(beacons[j]);
        }
        Console.WriteLine(uniqueBeacons.Count);
    }

    public override void Run2()
    {
        int maxManhattanDistance = 0;
        for (int i = 0; i < _locatedScans.Count; i++)
        {
            for (int j = i+1; j < _locatedScans.Count; j++)
            {
                var distance = _locatedScans[i].Center - _locatedScans[j].Center;
                int manhattanDist = Math.Abs(distance.x) + Math.Abs(distance.y) + Math.Abs(distance.z);
                if (manhattanDist > maxManhattanDistance)
                    maxManhattanDistance = manhattanDist;
            }
        }
        Console.WriteLine(maxManhattanDistance);
    }
    
    List<Scanner> LocateScans()
    {
        var scanners = ParseInput();
        var locatedScans = new List<Scanner>();
        var q = new Queue<Scanner>();

        // when a Scan is located, it get's into the queue so that we can
        // explore its neighbours.

        locatedScans.Add(scanners[0]);
        q.Enqueue(scanners[0]);
        scanners.RemoveAt(0);

        while (q.Count > 0)
        {
            var scanA = q.Dequeue();
            for (var index = scanners.Count - 1; index >= 0; index--)
            {
                var scanB = scanners[index];
                var maybeLocatedScan = TryToLocate(scanA, scanB);
                if (!maybeLocatedScan.IsNull)
                {
                    locatedScans.Add(maybeLocatedScan);
                    q.Enqueue(maybeLocatedScan);
                    scanners.RemoveAt(index);
                }
            }
        }

        return locatedScans;
    }
    Scanner TryToLocate(Scanner scanA, Scanner scanB) {
        var beaconsInA = scanA.GetBeaconsInWorld();

        foreach (var (beaconInA, beaconInB) in PotentialMatchingBeacons(scanA, scanB)) {
            // now try to find the orientation for B:
            var rotatedB = scanB;
            for (var rotation = 0; rotation < 24; rotation++, rotatedB = rotatedB.Rotate()) {
                // Moving the rotated Scan so that beaconA and beaconB overlaps. Are there 12 matches? 
                var beaconInRotatedB = rotatedB.Transform(beaconInB);

                var locatedB = rotatedB.Translate(new Vector3i(
                    beaconInA.x - beaconInRotatedB.x,
                    beaconInA.y - beaconInRotatedB.y,
                    beaconInA.z - beaconInRotatedB.z
                ));

                if (locatedB.GetBeaconsInWorld().Intersect(beaconsInA).Count() >= 12) {
                    return locatedB;
                }
            }
        }

        // no luck
        return default;
    }
    

    IEnumerable<(Vector3i beaconInA, Vector3i beaconInB)> PotentialMatchingBeacons(Scanner scanA, Scanner scanB) {
        
        // https://github.com/encse/adventofcode/blob/master/2021/Day19/Solution.cs
        var a_Beacons = scanA.GetBeaconsInWorld();
        for (int i = 0; i < a_Beacons.Count - 11; i++)
        {
            var beaconInA = a_Beacons[i];
            var scanATranslated = scanA.Translate(new Vector3i(-beaconInA.x, -beaconInA.y, -beaconInA.z));
            var diffsA = scanATranslated.AbsCoordinates().ToHashSet();

            var b_Beacons = scanB.GetBeaconsInWorld();
            for (int j = 0; j < b_Beacons.Count - 11; j++)
            {
                var beaconInB = b_Beacons[j];
                var scanBTranslated = scanB.Translate(new Vector3i(-beaconInB.x, -beaconInB.y, -beaconInB.z));
                var diffsB = scanBTranslated.AbsCoordinates();
                int count = 0;
                for (int k = 0; k < diffsB.Count; k++)
                    if(diffsA.Contains(diffsB[k]) && ++count >= 3 * 12)
                        yield return (beaconInA, beaconInB);

                /*if (diffsB.Count(d => diffsA.Contains(d)) >= 3 * 12) {
                    yield return (beaconInA, beaconInB);
                }*/
            }
        }
    }
}

public readonly struct Scanner
{
    public bool IsNull => _beaconsInLocal == null;

    public readonly Vector3i Center;
    readonly int _rotation;
    readonly List<Vector3i> _beaconsInLocal;
    public Scanner(Vector3i center, int rotation, List<Vector3i> beaconsInLocal)
    {
        Center = center;
        _rotation = rotation;
        _beaconsInLocal = beaconsInLocal;
    }

    public static bool operator ==(in Scanner a, in Scanner b) => a.Center == b.Center && a._rotation == b._rotation && a._beaconsInLocal == b._beaconsInLocal;
    public static bool operator !=(in Scanner a, in Scanner b) => !( a == b );
    public override int GetHashCode() { return HashCode.Combine(Center, _rotation, _beaconsInLocal); }
    bool Equals(Scanner other) { return Center.Equals(other.Center) && _rotation == other._rotation && _beaconsInLocal.Equals(other._beaconsInLocal); }
    public override bool Equals(object? obj) { return obj is Scanner other && Equals(other); }

    public Scanner Rotate() => new Scanner(Center, _rotation + 1, _beaconsInLocal);
    public Scanner Translate(in Vector3i t) => new Scanner(new Vector3i(Center.x + t.x, Center.y + t.y, Center.z + t.z), _rotation, _beaconsInLocal);
    public Vector3i Transform(in Vector3i coords) 
    {
        //if(_rotation == 0) return Center + coords;

        var (x, y, z) = coords;
        // rotate coordinate system so that x-axis points in the possible 6 directions
        switch (_rotation % 6) {
            case 0: (x, y, z) = (x, y, z); break;
            case 1: (x, y, z) = (-x, y, -z); break;
            case 2: (x, y, z) = (y, -x, z); break;
            case 3: (x, y, z) = (-y, x, z); break;
            case 4: (x, y, z) = (z, y, -x); break;
            case 5: (x, y, z) = (-z, y, x); break;
        }
        // rotate around x-axis:
        switch ((_rotation / 6) % 4) {
            case 0: (x, y, z) = (x, y, z); break;
            case 1: (x, y, z) = (x, -z, y); break;
            case 2: (x, y, z) = (x, -y, -z); break;
            case 3: (x, y, z) = (x, z, -y); break;
        }
        return new Vector3i(Center.x + x, Center.y + y, Center.z + z);
    }

    public List<int> AbsCoordinates()
    {
        List<int> result = new List<int>();
        var beacons = GetBeaconsInWorld();
        for (int i = 0; i < beacons.Count; i++)
        {
            var coords = beacons[i];
            result.Add(Math.Abs(coords.x));
            result.Add(Math.Abs(coords.y));
            result.Add(Math.Abs(coords.z));
        }
        return result;
    }

    public List<Vector3i> GetBeaconsInWorld()
    {
        List<Vector3i> result = new List<Vector3i>(_beaconsInLocal.Count);
        for (int i = 0; i < _beaconsInLocal.Count; i++)
            result.Add(Transform(_beaconsInLocal[i]));
        return result;
    }
}