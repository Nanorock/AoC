using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCodes;

namespace AdventOfCode_2021
{
    internal class AoCBingo4 : AdventOfCode
    {
        const int BoardStartLine = 2;

        int[] _inputNumbers;
        readonly List<Board> _boards = new List<Board>();
        public override void Init()
        {
            _inputNumbers = inputFile[0].Split(',').Select(int.Parse).ToArray();
            int boardReadLine = 2;
            while (_boards.Count * 6 + boardReadLine + Board.BoardSize <= inputFile.Length)
                _boards.Add(new Board(inputFile, _boards.Count));
        }
        public override void Run1()
        {
            for (int i = 0; i < _inputNumbers.Length; i++)
            {
                var input = _inputNumbers[i];
                for (int boardIndex = 0; boardIndex < _boards.Count; boardIndex++)
                {
                    var board = _boards[boardIndex];
                    if (board.SetNumberAndCheckWin(input))
                    {
                        PresentWinningBoard(board, input);
                        return;
                    }
                }
            }
        }
        //Result 49860 win

        public override void Run2()
        {
            HashSet<Board> winningBoards = new HashSet<Board>();
            Board lastWinningBoard = null;
            int lastWinInput = 0;
            for (int i = 0; i < _inputNumbers.Length; i++)
            {
                var input = _inputNumbers[i];
                for (int boardIndex = 0; boardIndex < _boards.Count; boardIndex++)
                {
                    var board = _boards[boardIndex];
                    if (winningBoards.Contains(board)) 
                        continue;

                    if (board.SetNumberAndCheckWin(input))
                    {
                        winningBoards.Add(board);
                        lastWinningBoard = board;
                        lastWinInput = input;
                    }
                }
            }
            PresentWinningBoard(lastWinningBoard, lastWinInput);
        }

        void PresentWinningBoard(Board board, int input)
        {
            int sum = board.SumCheck();
            WriteLine($"Input Value {input}");
            WriteLine($"Unchecked Sum {sum}");
            WriteLine($"Result {sum * input}");
        }
        
        struct BoardValue
        {
            public bool Checked;
            public int Value;
            public BoardValue(int value, bool @checked = false) { Value = value; Checked = @checked; }
        }
        class Board
        {
            public const int BoardSize = 5;
            readonly BaseTilemap<BoardValue> _board;
            readonly HashSet<int> _values = new HashSet<int>();

            public Board(string[] input, int index)
            {
                _board = new BaseTilemap<BoardValue>(BoardSize, BoardSize);

                int startRead = BoardStartLine + index * 6;
                for (int row = 0; row < BoardSize; row++)
                {
                    int fileLine = startRead + row;
                    var chars = input[fileLine].Trim().Replace("  "," ").Split(' ');
                    for (int column = 0; column < BoardSize; column++)
                    {
                        int val = int.Parse(chars[column]);
                        _board.Set(column, row, new BoardValue(val));
                        _values.Add(val);
                    }
                }
            }

            public bool SetNumberAndCheckWin(int val)
            {
                if (!_values.Contains(val)) return false;
                for (int i = 0; i < _board.Size; i++)
                {
                    if (_board[i].Value == val)
                    {
                        _board[i] = new BoardValue(_board[i].Value, true);
                        _board.GetXY(i, out var column, out var row);
                        if (CheckWinColumn(column) || CheckWinRow(row))
                            return true;
                    }
                }
                return false;
            }

            bool CheckWinColumn(int column)
            {
                for (int row = 0; row < BoardSize; row++)
                    if (!_board.Get(column, row).Checked)
                        return false;
                return true;
            }
            bool CheckWinRow(int row)
            {
                for (int column = 0; column < BoardSize; column++)
                    if (!_board.Get(column, row).Checked)
                        return false;
                return true;
            }
            
            public int SumCheck(bool @checked = false)
            {
                int sum = 0;
                for (int i = 0; i < _board.Size; i++)
                    if (_board[i].Checked == @checked)
                        sum += _board[i].Value;
                return sum;
            }
        }
    }
}