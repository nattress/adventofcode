public class AdventOfCode
{
    public static void Main(string[] args)
    {
        using TextReader tr = File.OpenText("input_01.txt");
        
        string? line = tr.ReadLine();
        int part1 = FindUniqueCharacterRun(line!, 4);
        int part2 = FindUniqueCharacterRun(line!, 14);

        Console.WriteLine($"Part One: {part1}.");
        Console.WriteLine($"Part Two: {part2}.");
    }

    static int FindUniqueCharacterRun(string input, int runLength)
    {
        HashSet<char> letters = new HashSet<char>();
        for (int i = 0; i < input.Length - runLength; i++)
        {
            for (int j = 0; j < runLength; j++)
            {
                letters.Add(input[i + j]);
            }
            
            if (letters.Count == runLength)
            {
                // done
                return i + runLength;
            }

            letters.Clear();
        }

        throw new InvalidDataException();
    }
}