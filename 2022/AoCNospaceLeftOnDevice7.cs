using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodes;

namespace AoC._2022
{
    class AoCNospaceLeftOnDevice7 : AdventOfCodes.AdventOfCode
    {
        Dir root = new Dir("root", null);
        public override void Run1()
        {
            Dir currentDir = root;
            for (int i = 2; i < inputFile.Length; i++)
                currentDir = currentDir.Process(inputFile[i]);
            while(currentDir != root)
                currentDir = currentDir.Process("$ cd ..");

            ulong totalSmallDirSize = 0;
            root.ForEachDir(dir =>
            {
                if (dir.FolderSize <= 100000)
                    totalSmallDirSize += dir.FolderSize;
            });
            Console.WriteLine($"Answer is {totalSmallDirSize}");
        }

        public override void Run2()
        {
            ulong unusedSpace = 70000000ul - root.FolderSize;
            ulong needToFree = 30000000ul - unusedSpace;
            
            ulong closestDirSize = ulong.MaxValue;
            root.ForEachDir(dir =>
            {
                if (dir.FolderSize < closestDirSize && dir.FolderSize >= needToFree)
                    closestDirSize = dir.FolderSize;
            });
            Console.WriteLine($"Answer is {closestDirSize}");
        }
        class Dir
        {
            public readonly string Name;
            public ulong FolderSize;
            public readonly List<File> Files = new List<File>();
            public readonly List<Dir> Directories = new List<Dir>();
            public readonly Dir Parent;
            public Dir(string name, Dir parent)
            {
                Name = name;
                Parent = parent;
            }

            public Dir Process(string input)
            {
                if (input.StartsWith("dir"))
                    Directories.Add(new Dir(input[4..], this));
                else if (char.IsDigit(input[0]))
                {
                    int separator = input.IndexOf(" ");
                    var file = new File(input[(separator + 1)..], uint.Parse(input[..separator]));
                    FolderSize += file.Size;
                    Files.Add(file);
                }
                else if (input == "$ cd ..")
                {
                    Parent.FolderSize += FolderSize;
                    return Parent;
                }
                else if (input.StartsWith("$ cd "))
                {
                    string dirName = input[5..];
                    for (int i = 0; i < Directories.Count; i++)
                        if(Directories[i].Name == dirName)
                            return Directories[i];
                    throw new Exception($"Unknown folder {dirName} in {Name}");
                }
                return this;
            }

            public override string ToString() => $"Dir {Name}:{FolderSize}";

            public void ForEachDir(Action<Dir> forEach)
            {
                for (int i = 0; i < Directories.Count; i++)
                    Directories[i].ForEachDir(forEach);
                forEach(this);
            }
        }
        class File
        {
            public string Name;
            public uint Size;
            public File(string name, uint size)
            {
                Name = name;
                Size = size;
            }
            public override string ToString() => $"F {Name}:{Size}";
        }
    }
}
