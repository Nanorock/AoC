using AdventOfCodes;

namespace AdventOfCode_2021;

class AoCDiracDice21 : AdventOfCode
{
    public override void Run1()
    {
        int winningScore = 1000;

        var p1 = new Player(inputFile[0][^1] - '0', 0);
        var p2 = new Player(inputFile[1][^1] - '0', 0);
        Dice dice = new Dice();
        var game = new Game(p1, p2);

        while (!game.Win(winningScore))
            game = game.Progress(dice.Roll3());

        int lowestScore = Math.Min(game.P1.Score , game.P2.Score);
        Console.WriteLine(lowestScore * dice.DiceRolls);
    }
    class Dice
    {
        int _dice;
        int _diceRoll;
        public int DiceRolls => _diceRoll;
        public int Roll3()
        {
            int move = 0;
            for (int i = 0; i < 3; ++i)
                move += Roll();
            return move;
        }
        int Roll()
        {
            ++_diceRoll;
            var result = ++_dice;
            if (_dice >= 100) _dice=0;
            return result;
        }
    }
    readonly struct Game
    {
        public override string ToString() { return $"{P1.Score}/{P2.Score}/{_p2Turn}"; }

        public readonly Player P1;
        public readonly Player P2;
        readonly bool _p2Turn;
        
        public bool Win(int score) => P1.Win(score) || P2.Win(score);
        public Game(Player p1, Player p2, bool p2Turn = false)
        {
            P1 = p1;
            P2 = p2;
            _p2Turn = p2Turn;
        }
        public Game Progress(int roll) =>
            _p2Turn ? new Game(P1, P2.Step(roll), !_p2Turn) 
                    : new Game(P1.Step(roll), P2, !_p2Turn);

        public override int GetHashCode() => HashCode.Combine(P1,P2,_p2Turn);
    }
    readonly struct Player
    {
        readonly int _board;
        readonly int _score;
        public int Score => _score;
        public Player(int start, int score = 0)
        {
            _board = start;
            _score = score;
        }
        public bool Win(int score) => _score >= score;
        public Player Step(int move)
        {
            var nextMove = (_board+move-1)%10 + 1;
            return new Player(nextMove, _score + nextMove);
        }
        public override int GetHashCode() => HashCode.Combine(_board,_score);
    }

   
    public override void Run2()
    {
        var game = new Game(new Player(inputFile[0][^1] - '0'), new Player(inputFile[1][^1] - '0'));
        var result = Play(game);
        Console.WriteLine($"{result.player1Wins},{result.player2Wins}");
    }
    
    //If you roll 3 times a D3, here are all the possible results frequency
    readonly ulong[] _d3Roll3Freq = { 1, 3, 6, 7, 6, 3, 1 };
    Dictionary<int, (ulong player1Wins, ulong player2Wins)> _determinedOutcome = new Dictionary<int, (ulong player1Wins, ulong player2Wins)>();
    
    (ulong player1Wins, ulong player2Wins) Play(in Game game)
    {
        int hash = game.GetHashCode();
        if (_determinedOutcome.TryGetValue(hash, out var resultCache))
            return resultCache;

        (ulong player1Wins, ulong player2Wins) results = default;
        for (int diceResult = 3; diceResult <= 9; ++diceResult)
        {
            var frequency = _d3Roll3Freq[diceResult - 3];

            var newGame = game.Progress(diceResult);

            if (newGame.P1.Win(21)) results.player1Wins += frequency;
            else if (newGame.P2.Win(21)) results.player2Wins += frequency;
            else
            {
                var play = Play(newGame);
                results.player1Wins += play.player1Wins * frequency;
                results.player2Wins += play.player2Wins * frequency;
            }
        }

        _determinedOutcome.Add(hash, results);

        return results;
    }
}