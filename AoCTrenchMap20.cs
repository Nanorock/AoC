using System.Collections;
using System.Numerics;

namespace AdventOfCode_2021;

class AoCTrenchMap20 : AdventOfCode
{
    int[] _imageEnhancementAlgorithm = Array.Empty<int>();
    Tilemap _sourceImage;
    
    public override void Init()
    {
        var imageEnhancementAlgorithmStr = inputFile[0];
        Array.Resize(ref _imageEnhancementAlgorithm, imageEnhancementAlgorithmStr.Length);
        for (int i = 0; i < imageEnhancementAlgorithmStr.Length; i++)
            _imageEnhancementAlgorithm[i] = imageEnhancementAlgorithmStr[i] == '#' ? 1 : 0;

        string[] map = new string[inputFile.Length - 2];
        Array.ConstrainedCopy(inputFile, 2, map, 0, map.Length);
        _sourceImage = new Tilemap(map, c => c=='#'?1:0);
    }

    public override void Run1() { RunEnhancementAndCountPixels(_sourceImage, 2); }
    public override void Run2() { RunEnhancementAndCountPixels(_sourceImage, 50); }
    
    void RunEnhancementAndCountPixels(Tilemap source, int count)
    {
        var image = source;
        var emptyBit = 0;
        for (int i = 0; i < count; i++)
        {
            image = EnhanceImage(image, emptyBit);
            emptyBit = GetInfiniteVoidAlgoBit(emptyBit);
        }
        //Console.WriteLine(image.GetPrinter((id,c)=>""+c).PrintState());
        Console.WriteLine($"Pixel Lit:{image.Count(c => c == 1)}");
    }
    
    int GetInfiniteVoidAlgoBit(int emptyBit)
    {
        int algoId = emptyBit == 0 ? 0 : (emptyBit << 9) - 1;
        return _imageEnhancementAlgorithm[algoId];
    }
    
    const int InfinitePadding = 1; 
    Tilemap EnhanceImage(Tilemap sourceImage, int infiniteVoidChar = 0)
    {
        var enhancedImage = new Tilemap(sourceImage.Width + 2 * InfinitePadding, 
                                       sourceImage.Height + 2 * InfinitePadding);
        for (int y = 0; y < enhancedImage.Height; y++)
        for (int x = 0; x < enhancedImage.Width; x++)
        {
            var sourceImageX = x - InfinitePadding;
            var sourceImageY = y - InfinitePadding;

            using var neighbors = sourceImage.GetFullNeighbors(sourceImageX,sourceImageY);
            int algoId = 0;
            for (int j = 0; j < neighbors.Length; j++)
            {
                var neighId = neighbors[j];
                int bit = neighId >= 0 ? sourceImage[neighId] : infiniteVoidChar;
                algoId |= bit << neighbors.Length - j - 1;
            }
            var outputBit = _imageEnhancementAlgorithm[algoId];
            enhancedImage.Set(x,y, outputBit);
        }
        
        return enhancedImage;
    }
}