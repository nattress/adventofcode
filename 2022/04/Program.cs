public class AdventOfCode
{
    static bool RangeIsSubset(int oneStart, int oneEnd, int twoStart, int twoEnd)
    {
        return oneStart >= twoStart && oneEnd <= twoEnd ||
            twoStart >= oneStart && twoEnd <= oneEnd;
    }

    static bool RangeOverlaps(int oneStart, int oneEnd, int twoStart, int twoEnd)
    {
        return oneStart >= twoStart && oneStart <= twoEnd ||
            twoStart >= oneStart && twoStart <= oneEnd;
    }

    public static void Main(string[] args)
    {
        // 1. Add state tracking for file parsing 
        int partOneOverlapCount = 0;
        int partTwoOverlapCount = 0;
        using (TextReader tr = File.OpenText(args[0]))
        {
            while (true)
            {
                string? line = tr.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                int[] elfPair = line.Split(new char[] {',', '-'}).Select((item, index) => int.Parse(item)).ToArray();
                if (RangeIsSubset(elfPair[0], elfPair[1], elfPair[2], elfPair[3]))
                    partOneOverlapCount++;
                
                if (RangeOverlaps(elfPair[0], elfPair[1], elfPair[2], elfPair[3]))
                    partTwoOverlapCount++;
            }

            Console.WriteLine($"Part One: {partOneOverlapCount}.");
            Console.WriteLine($"Part Two: {partTwoOverlapCount}.");
        }
    }

    static int Part1()
    {
        return 0;
    }

    static int Part2()
    {
        return 0;
    }
}