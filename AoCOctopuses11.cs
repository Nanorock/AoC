using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode_2021
{
    class AoCOctopuses11 : AdventOfCode
    {
        Tilemap _octopuses;
        public override void Run1()
        {
            _octopuses = new Tilemap(inputFile);
            const int simulationStep = 100;
            var flashes = 0;
            for (int i = 0; i < simulationStep; i++)
                flashes += Step();

            WriteLine($"Cute flashes count:{flashes}");
        }
        public override void Run2()
        {
            _octopuses = new Tilemap(inputFile);//Reset !
            var step = 1;
            while (Step() < _octopuses.Size)
                ++step;

            WriteLine($"All synchro flash at step:{step}");
        }

        readonly HashSet<int> _stepFlashes = new HashSet<int>();
        int Step()
        {
            _stepFlashes.Clear();
            for (int i = 0; i < _octopuses.Size; i++)
                Step(i);
            return _stepFlashes.Count;
        }
        void Step(int id)
        {
            if (_stepFlashes.Contains(id)) return;
            if (++_octopuses[id] >= 10) //Flash !
                Flash(id);
        }
        void Flash(int id)
        {
            if (!_stepFlashes.Add(id)) return;
            _octopuses[id] = 0;
            using var neighbors= _octopuses.Get8Neighbors(id);
            for (int i = 0; i < neighbors.Length; i++)
                Step(neighbors[i]);
        }
    }
}