using System.Text;
namespace AoC._2022
{
    class AoCCathodeRayTube10 : AdventOfCodes.AdventOfCode
    {
        public override void Run1()
        {
            Renderer renderer = new Renderer();
            long totalSignalStrength = 0;
            for (int i = 0; i < inputFile.Length; i++)
            {
                var input = inputFile[i];
                var command = input[..4];
                totalSignalStrength += renderer.AddCycleAndRecordSignalStrength(20);
                if (command == "addx")
                    totalSignalStrength += renderer.AddCycleAndRecordSignalStrength(20, int.Parse(input[5..])); 
            }
            Console.WriteLine($"Answer is {totalSignalStrength}");
        }

        public override void Run2()
        {
            Renderer renderer = new Renderer();
            StringBuilder line = new StringBuilder();
            int lineDrawn = 0;
            for (int i = 0; i < inputFile.Length; i++)
            {
                var input = inputFile[i];
                var command = input[..4];
                renderer.RenderCycle(line, ref lineDrawn);
                if (command == "addx")
                    renderer.RenderCycle(line, ref lineDrawn, int.Parse(input[5..]));
            }
            Console.WriteLine(line);
        }
        
        class Renderer
        {
            int _x = 1;
            int _cycle;
            readonly int _cycleCapture;
            public Renderer(int screenCycleWidth = 40) { _cycleCapture = screenCycleWidth; }
            
            public long AddCycleAndRecordSignalStrength(int cycleStart, int thenChangeX = 0)
            {
                ++_cycle;
                var xStartCycle = _x;
                _x += thenChangeX;
                return (_cycle + cycleStart) % _cycleCapture == 0 ? _cycle * xStartCycle : 0;
            }
            public void RenderCycle(StringBuilder line, ref int currentLine, int changeX = 0)
            {
                AddRowPixel(GetPixel(), line, ref currentLine);
                ++_cycle;
                _x += changeX;
            }
            char GetPixel()
            {
                var row = _cycle / _cycleCapture;
                var column = _cycle - row * _cycleCapture;
                var pixel = Math.Abs(_x - column) <= 1 ? '#' : '.';
                return pixel;
            }
            void AddRowPixel(char pixel, StringBuilder line, ref int currentLine)
            {
                var y = _cycle/_cycleCapture;
                if (currentLine != y)
                {
                    currentLine = y;
                    Console.WriteLine(line);
                    line.Clear();
                }
                line.Append(pixel);
            }
        }
    }
}
