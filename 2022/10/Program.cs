public class AdventOfCode
{
    public static void Main(string[] args)
    {
        using TextReader tr = File.OpenText("input_01.txt");
        
        while (true)
        {
            string? line = tr.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            
            // Todo: Parse input.
        }

        int part1 = Part1();
        int part2 = Part2();

        Console.WriteLine($"Part One: {part1}.");
        Console.WriteLine($"Part Two: {part2}.");
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