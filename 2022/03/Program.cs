public class AdventOfCode
{
    private static string GetCommonCharacter(string ruckSack)
    {
        HashSet<string> left = new HashSet<string>();
        HashSet<string> right = new HashSet<string>();

        for (int i = 0; i < ruckSack.Length / 2; i++)
        {
            left.Add(ruckSack[i].ToString());
            right.Add(ruckSack[ruckSack.Length / 2 + i].ToString());
        }

        return left.Intersect(right).First();
    }

    private static char GetCommonCharacter(string sack1, string sack2, string sack3)
    {
        return sack1.Intersect(sack2).Intersect(sack3).First();
    }

    public static void Main(string[] args)
    {
        List<string> commonCharacters = new List<string>();
        List<string> sackGroups = new List<string>();
        List<char> sackGroupCommonCharacters = new List<char>();

        using (TextReader tr = File.OpenText(args[0]))
        {
            while (true)
            {
                string? line = tr.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                commonCharacters.Add(GetCommonCharacter(line));

                sackGroups.Add(line);
                
                if (sackGroups.Count == 3)
                {
                    sackGroupCommonCharacters.Add(GetCommonCharacter(sackGroups[0], sackGroups[1], sackGroups[2]));
                    sackGroups.Clear();
                }
            }

            int part1 = Part1(commonCharacters);
            int part2 = Part2(sackGroupCommonCharacters);

            Console.WriteLine($"Part One: {part1}.");
            Console.WriteLine($"Part Two: {part2}.");
        }
    }

    static int Part1(List<string> commonCharacters)
    {
        int result = 0;

        foreach (string s in commonCharacters)
        {
            if (char.IsUpper(s[0]))
            {
                result += s[0] - 'A' + 27;
            }
            else
            {
                result += s[0] - 'a' + 1;
            }
        }

        return result;
    }

    static int Part2(List<char> commonCharacters)
    {
        int result = 0;

        foreach (char c in commonCharacters)
        {
            if (char.IsUpper(c))
            {
                result += c - 'A' + 27;
            }
            else
            {
                result += c - 'a' + 1;
            }
        }

        return result;
    }
}