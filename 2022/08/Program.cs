public class AdventOfCode
{
    public static void Main(string[] args)
    {
        using TextReader tr = File.OpenText("input_01.txt");
        
        List<int[]> grid = new List<int[]>();

        while (true)
        {
            string? line = tr.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            
            int[] newRow = new int[line.Length];
            int column = 0;
            foreach (char c in line)
            {
                int treeHeight = int.Parse(c.ToString());
                newRow[column++] = treeHeight;
            }

            grid.Add(newRow);
        }

        int part1 = CountVisible(grid);
        int part2 = Part2();

        Console.WriteLine($"Part One: {part1}.");
        Console.WriteLine($"Part Two: {part2}.");
    }

    static int CountVisible(List<int[]> grid)
    {
        Dictionary<(int, int), bool> visibleMap = new Dictionary<(int, int), bool>();
        int numberVisible = 0;
        for (int y = 0; y < grid.Count; y++)
        {
            
        }

        for (int x = 0; x < grid[0].Length; x++)
        {
            
        }

        return numberVisible;
    }

    static int Part2()
    {
        return 0;
    }
}