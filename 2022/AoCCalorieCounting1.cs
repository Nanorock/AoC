using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC._2022;

class AoCCalorieCounting1 : AdventOfCodes.AdventOfCode
{
    List<int> _elfCalories = new List<int>();
    public override void Run1()
    {
        base.Run1();
        
        int highestCalories = 0;
        int runningCalories = 0;
        for (int i = 0; i < inputFile.Length; i++)
        {
            if (string.IsNullOrEmpty(inputFile[i]))
            {
                _elfCalories.Add(runningCalories);
                runningCalories = 0;
                continue;
            }
            
            runningCalories += int.Parse(inputFile[i]);
            if(runningCalories > highestCalories)
                highestCalories = runningCalories;
        }
        Console.WriteLine($"Answer is {highestCalories}");
    }
    public override void Run2()
    {
        _elfCalories.Sort();
        int topThreeTotalCalories = 0;
        for(int i = _elfCalories.Count - 1; i >=  _elfCalories.Count - 3; --i)
        {
            topThreeTotalCalories += _elfCalories[i];
        }
        Console.WriteLine($"Answer is {topThreeTotalCalories}");
    }
}
