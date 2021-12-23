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
    //15322 !

    public override void Run1()
    {
        base.Run1();
    }


    public override void Run2()
    {
        
        var start = ParseBoard();
        var sw = Stopwatch.StartNew();
        var bestMove = GetBestMove(start);
        sw.Stop();

        Console.WriteLine($"Min cost {bestMove} in {sw.Elapsed.TotalMilliseconds}ms");
    }

    List<Board> _possibleMoves = new List<Board>();
    int GetBestMove(Board initialState)
    {
        var priorityQueue = new PriorityQueue<Board, int>();// Dijkstra
        priorityQueue.Enqueue(initialState, 0);
        var visited = new HashSet<int>();
        while (priorityQueue.TryDequeue(out var node, out _))
        {
            if (!visited.Add(node.GetHashCode()))
                continue;
            if (node.Wins())
                return node.Cost;
            
            _possibleMoves.Clear();
            node.GetPossiblesMoves(_possibleMoves);
            for (int i = 0; i < _possibleMoves.Count; i++)
            {
                var mode = _possibleMoves[i];
                priorityQueue.Enqueue(mode,mode.Cost);
            }
        }
        return int.MaxValue;
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
        return new Board(default, rooms, 0);
    }
    

    const char Empty = default;
    static char[] Amphipods = { 'A', 'B', 'C', 'D' };
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static int GetAmphiId(char amphipod) => amphipod - 'A' + 1;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool IsRoomEntrance(int hallId) => hallId is 2 or 4 or 6 or 8;

    readonly struct Board
    {
        public readonly ValueArray11<char> Hall;
        public readonly ValueArray4<string> Rooms;
        public readonly int Cost;
        
        public override int GetHashCode() => HashCode.Combine(Hall,Rooms);
        public Board(ValueArray11<char> hall, ValueArray4<string> rooms, int cost)
        {
            Hall = hall;
            Rooms = rooms;
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
            for (int i = Hall.Count - 1; i >= 0; --i)
            {
                var amphipod = Hall[i];
                if (amphipod == Empty) continue;
                GetPossiblesHallToRoomMoves(amphipod, i, moves);
            }
        }
        void GetPossiblesRoomMoves(List<Board> moves)
        {
            for (int i = Rooms.Count - 1; i >= 0; --i)
            {
                if (RoomContainsOnlyValid(i)) continue;
                int roomHallId = _roomHallId[i];
                var amphipod = PeekRoom(i);
                GetPossiblesRoomHallMoves(amphipod, roomHallId, moves);
            }
        }
        
        public bool RoomContainsOnlyValid(int id)
        {
            var r = Rooms[id];
            var validChar = Amphipods[id];
            for (int i = 0; i < r.Length; i++)
                if (r[i] != validChar)
                    return false;
            return true;
        }
        public char PeekRoom(int id) => Rooms[id][^1];
        
        void GetPossiblesHallToRoomMoves(char amphipod, int from, List<Board> moves)
        {
            int amphiId = GetAmphiId(amphipod);
            for (int rev = from-1; rev >= 0; --rev)
            {
                if (Hall[rev] != Empty) break;
                if (!IsRoomEntrance(rev)) continue;
                int roomId = rev / 2 - 1;

                if (roomId+1 == amphiId && RoomContainsOnlyValid(roomId)) 
                    moves.Add(GetRoomMove(amphipod, @from, roomId));
            }
            for (int fwd = from+1; fwd < Hall.Count; fwd++)
            {
                if (Hall[fwd] != Empty) break;
                if (!IsRoomEntrance(fwd)) continue;
                int roomId = fwd / 2 - 1;
                if (roomId+1 == amphiId && RoomContainsOnlyValid(roomId)) 
                    moves.Add(GetRoomMove(amphipod, @from, roomId));
            }
        }
        void GetPossiblesRoomHallMoves(char amphipod, int roomHallId, List<Board> moves)
        {
            for (int rev = roomHallId-1; rev >= 0; --rev)
            {
                if (Hall[rev] != Empty) break;
                if (!IsRoomEntrance(rev))
                    moves.Add(GetRoomToHallMove(amphipod, roomHallId, rev));
                else
                {
                    int roomId = rev / 2 - 1;
                    if (Amphipods[roomId] == amphipod && RoomContainsOnlyValid(roomId))
                        moves.Add(GetRoomToRoomMove(amphipod, roomHallId, rev));
                }
            }
            for (int fwd = roomHallId+1; fwd < Hall.Count; ++fwd)
            {
                if (Hall[fwd] != Empty) break;
                if (!IsRoomEntrance(fwd))
                    moves.Add(GetRoomToHallMove(amphipod, roomHallId, fwd));
                else
                {
                    int roomId = fwd / 2 - 1;
                    if (Amphipods[roomId] == amphipod && RoomContainsOnlyValid(roomId))
                        moves.Add(GetRoomToRoomMove(amphipod, roomHallId, fwd));
                }
            }
        }
        
        Board GetRoomMove(char amphipod, int from, int roomId)
        {
            int amphiId = GetAmphiId(amphipod);
            int moveCost = _amphipodMoveCost[amphiId];
            var hall = Hall;
            hall.Set(from, Empty);
            
            var rooms = Rooms;    
            var roomState = Rooms[roomId];
            roomState += amphipod;
            rooms.Set(roomId, roomState);

            int cost = Math.Abs(from - _roomHallId[roomId]);
            cost += 5 - roomState.Length;
            cost *= moveCost;
            
            return new Board(hall,rooms, Cost + cost);
        }
        Board GetRoomToHallMove(char amphipod, int roomHallId, int toHallId)
        {
            int amphiId = GetAmphiId(amphipod);
            int moveCost = _amphipodMoveCost[amphiId];

            var hall = Hall;
            hall.Set(roomHallId, Empty);
            hall.Set(toHallId, amphipod);
            
            int roomId = roomHallId / 2 - 1;
            var rooms = Rooms;
            var copyRoomState = rooms[roomId][..^1];
            rooms.Set(roomId, copyRoomState); 
            
            int cost = 4 - copyRoomState.Length;//Getting to the hallway
            cost += Math.Abs(roomHallId - toHallId);
            cost *= moveCost;
            
            return new Board(hall, rooms, Cost + cost);
        }
        Board GetRoomToRoomMove(char amphipod, int roomHallId, int toHallId)
        {
            int amphiId = GetAmphiId(amphipod);
            int moveCost = _amphipodMoveCost[amphiId];
            
            int roomId = roomHallId / 2 - 1;
            var rooms = Rooms;
            var copyRoomState = rooms[roomId][..^1];
            rooms.Set(roomId, copyRoomState); 

            roomId = toHallId / 2 - 1;
            var copyDestRoomState = rooms[roomId]+amphipod;
            rooms.Set(roomId, copyDestRoomState); 
            
            int cost = 4 - copyRoomState.Length;//Getting to the hallway
            cost += 5 - copyDestRoomState.Length;
            cost += Math.Abs(roomHallId - toHallId);
            cost *= moveCost;
            
            return new Board(Hall, rooms, Cost + cost);
        }

        public bool Wins()
        {
            for (int i = 0; i < Hall.Count; i++)
                if (Hall[i] != Empty)
                    return false;
            for (int i = 0; i < Rooms.Count; i++)
                if(!RoomContainsOnlyValid(i))
                    return false;
            return true;
        }

        public override string ToString()
        {
            return $"{Hall}/{Rooms[0]}/{Rooms[1]}/{Rooms[2]}/{Rooms[3]}/{Cost}";
        }
    }

    static int[] _roomHallId = { 2, 4, 6, 8 };
    static int[] _pauseHallId = { 0, 1, 3, 5, 7, 9, 10 };
    static int[] _amphipodMoveCost = { 0, 1, 10, 100, 1000 };
    static int[] _amphipodRoom = { 0, 0, 1, 2, 3 };
    
}




