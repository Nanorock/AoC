using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AdventOfCode_2021;

class AoCAmphipod23 : AdventOfCode
{
    public override void Run2()
    {
        var start = ParseBoard();
        var sw = Stopwatch.StartNew();
        var bestMove = GetLowestEnergyMove(start);
        sw.Stop();

        Console.WriteLine($"Min cost {bestMove} in {sw.Elapsed.TotalMilliseconds}ms");
    }

    readonly List<Board> _possibleMoves = new List<Board>(64);
    int GetLowestEnergyMove(Board initialState)
    {
        var priorityQueue = new PriorityQueue<Board, int>();// Dijkstra
        priorityQueue.Enqueue(initialState, 0);
        var visited = new HashSet<int>();
        while (priorityQueue.TryDequeue(out var node, out _))
        {
            if (!visited.Add(node.GetHashCode()))
                continue;
            if (node.Wins())
            {
                return node.Cost;
            }

            _possibleMoves.Clear();
            node.GetPossiblesMoves(_possibleMoves);
            
            for (int i = 0; i < _possibleMoves.Count; i++)
            {
                var mode = _possibleMoves[i];
                //_froms[mode] = chain;
                priorityQueue.Enqueue(mode,mode.Cost);
            }
        }
        return int.MaxValue;
    }

    Dictionary<Board, Chain> _froms = new Dictionary<Board, Chain>();
    class Chain
    {
        public Chain Previous;
        public Chain Next;

        public Board Board;
        public override string ToString() => Board.ToString();
    }

    Board ParseBoard()
    {
        string[] roomStates = new string[4];
        for (int i = 0; i < 4; i++)
            roomStates[i] = "";

        for (int i = 5; i >= 2; i--)
        {
            var line = inputFile[i].Replace("#", "").Trim();
            for (int charId = 0; charId < line.Length; charId++)
                roomStates[charId] += line[charId];
        }

        ValueArray4<string> rooms = default;
        for (int i = 0; i < 4; i++)
            rooms.Set(i,roomStates[i]);
        ValueArray11<char> defaultHall = default;
        for (int i = 0; i < defaultHall.Count; i++)
            defaultHall.Set(i,Empty);
        return new Board(defaultHall, rooms, 0);
    }
    

    const char Empty = '.';
    static readonly char[] Amphipods = { 'A', 'B', 'C', 'D' };
    static readonly int[] RoomHallId = { 2, 4, 6, 8 };
    static readonly int[] AmphipodMoveCost = { 0, 1, 10, 100, 1000 };//A starts at 1
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static int GetAmphiId(char amphipod) => amphipod - 'A' + 1;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool IsRoomEntrance(int hallId) => hallId is 2 or 4 or 6 or 8;

    readonly struct Board
    {
        readonly ValueArray11<char> _hall;
        readonly ValueArray4<string> _rooms;
        public readonly int Cost;
        
        public override int GetHashCode() => HashCode.Combine(_hall,_rooms);
        public Board(ValueArray11<char> hall, ValueArray4<string> rooms, int cost)
        {
            _hall = hall;
            _rooms = rooms;
            Cost = cost;
        }
        

        public void GetPossiblesMoves(List<Board> moves)
        {
            GetPossiblesHallToRoomMoves(moves);
            if(moves.Count == 0) 
                GetPossiblesRoomMoves(moves);
        }
        
        void GetPossiblesHallToRoomMoves(List<Board> moves)
        {
            for (int i = _hall.Count - 1; i >= 0; --i)
            {
                var amphipod = _hall[i];
                if (amphipod == Empty) continue;
                GetPossiblesHallToRoomMoves(amphipod, i, moves);
            }
        }
        void GetPossiblesRoomMoves(List<Board> moves)
        {
            GetPossiblesRoomMoves(moves, (c)=>c=='D');
            if (moves.Count > 0) return;
            GetPossiblesRoomMoves(moves, (_)=>true);
        }

        void GetPossiblesRoomMoves(List<Board> moves, Func<char,bool> filter)
        {
            for (int i = 0; i < _rooms.Count; ++i)
            {
                if (RoomContainsOnlyValid(i)) continue;
                int roomHallId = RoomHallId[i];
                var amphipod = PeekRoom(i);
                if(filter(amphipod))
                    GetPossiblesRoomHallMoves(amphipod, roomHallId, moves);
            }
        }

        public bool RoomContainsOnlyValid(int id)
        {
            var r = _rooms[id];
            var validChar = Amphipods[id];
            for (int i = 0; i < r.Length; i++)
                if (r[i] != validChar)
                    return false;
            return true;
        }
        public char PeekRoom(int id) => _rooms[id][^1];
        
        void GetPossiblesHallToRoomMoves(char amphipod, int from, List<Board> moves)
        {
            int amphiId = GetAmphiId(amphipod);
            for (int fwd = from+1; fwd < _hall.Count; fwd++)
            {
                if (_hall[fwd] != Empty) break;
                if (!IsRoomEntrance(fwd)) continue;
                int roomId = fwd / 2 - 1;
                if (roomId+1 == amphiId && RoomContainsOnlyValid(roomId)) 
                    moves.Add(GetRoomMove(amphipod, @from, roomId));
            }
            for (int rev = from-1; rev >= 0; --rev)
            {
                if (_hall[rev] != Empty) break;
                if (!IsRoomEntrance(rev)) continue;
                int roomId = rev / 2 - 1;

                if (roomId+1 == amphiId && RoomContainsOnlyValid(roomId)) 
                    moves.Add(GetRoomMove(amphipod, @from, roomId));
            }
        }
        void GetPossiblesRoomHallMoves(char amphipod, int roomHallId, List<Board> moves)
        {
            int amphiId = GetAmphiId(amphipod);

            for (int fwd = roomHallId+1; fwd < _hall.Count; ++fwd)
            {
                if (_hall[fwd] != Empty) break;
                if (!IsRoomEntrance(fwd))
                    moves.Add(GetRoomToHallMove(amphipod, roomHallId, fwd));
                else
                {
                    int roomId = fwd / 2 - 1;
                    if (roomId+1 == amphiId && RoomContainsOnlyValid(roomId))
                        moves.Add(GetRoomToRoomMove(amphipod, roomHallId, fwd));
                }
            }
            for (int rev = roomHallId-1; rev >= 0; --rev)
            {
                if (_hall[rev] != Empty) break;
                if (!IsRoomEntrance(rev))
                    moves.Add(GetRoomToHallMove(amphipod, roomHallId, rev));
                else
                {
                    int roomId = rev / 2 - 1;
                    if (roomId+1 == amphiId && RoomContainsOnlyValid(roomId))
                        moves.Add(GetRoomToRoomMove(amphipod, roomHallId, rev));
                }
            }
        }
        
        Board GetRoomMove(char amphipod, int from, int roomId)
        {
            int amphiId = GetAmphiId(amphipod);
            int moveCost = AmphipodMoveCost[amphiId];
            var hall = _hall;
            hall.Set(from, Empty);
            
            var rooms = _rooms;    
            var roomState = _rooms[roomId];
            roomState += amphipod;
            rooms.Set(roomId, roomState);

            int cost = 5 - roomState.Length + Math.Abs(from - RoomHallId[roomId]);
            cost *= moveCost;
            
            return new Board(hall,rooms, Cost + cost);
        }
        Board GetRoomToHallMove(char amphipod, int roomHallId, int toHallId)
        {
            int amphiId = GetAmphiId(amphipod);
            int moveCost = AmphipodMoveCost[amphiId];

            var hall = _hall;
            hall.Set(roomHallId, Empty);
            hall.Set(toHallId, amphipod);
            
            int roomId = roomHallId / 2 - 1;
            var rooms = _rooms;
            var copyRoomState = rooms[roomId][..^1];
            rooms.Set(roomId, copyRoomState); 
            
            int cost = 4 - copyRoomState.Length + Math.Abs(roomHallId - toHallId);
            cost *= moveCost;
            
            return new Board(hall, rooms, Cost + cost);
        }
        Board GetRoomToRoomMove(char amphipod, int roomHallId, int toHallId)
        {
            int amphiId = GetAmphiId(amphipod);
            int moveCost = AmphipodMoveCost[amphiId];
            
            int roomId = roomHallId / 2 - 1;
            var rooms = _rooms;
            var copyRoomState = rooms[roomId][..^1];
            rooms.Set(roomId, copyRoomState); 

            roomId = toHallId / 2 - 1;
            var copyDestRoomState = rooms[roomId]+amphipod;
            rooms.Set(roomId, copyDestRoomState); 
            
            int cost = 9 - copyRoomState.Length- copyDestRoomState.Length + Math.Abs(roomHallId - toHallId);
            cost *= moveCost;
            
            return new Board(_hall, rooms, Cost + cost);
        }

        public bool Wins()
        {
            for (int i = 0; i < _hall.Count; ++i)
                if (_hall[i] != Empty)
                    return false;
            for (int i = 0; i < _rooms.Count; ++i)
                if(!RoomContainsOnlyValid(i))
                    return false;
            return true;
        }

        public override string ToString() => $"{_hall[0]}{_hall[1]}{_hall[2]}{_hall[3]}{_hall[4]}{_hall[5]}{_hall[6]}{_hall[7]}{_hall[8]}{_hall[9]}{_hall[10]}/{_rooms[0]}/{_rooms[1]}/{_rooms[2]}/{_rooms[3]}/{Cost}";
    }

}




