public class AdventOfCode
{
    public static void Main(string[] args)
    {
        // 1. Add state tracking for file parsing 

        using (TextReader tr = File.OpenText(args[0]))
        {
            while (true)
            {
                string? line = tr.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // 2. Add logic for per-line handling
            }

            int part1 = Part1();
            int part2 = Part2();

            Console.WriteLine($"Part One: {part1}.");
            Console.WriteLine($"Part Two: {part2}.");
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