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
        int part2 = GetHighestScenicScore(grid);

        Console.WriteLine($"Part One: {part1}.");
        Console.WriteLine($"Part Two: {part2}.");
    }

    static int CountVisible(List<int[]> grid)
    {
        Dictionary<(int, int), bool> visibleMap = new Dictionary<(int, int), bool>();

        for (int y = 0; y < grid.Count; y++)
        {
            int height = -1;
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] > height)
                {
                    visibleMap[(x, y)] = true;
                    height = Math.Max(height, grid[y][x]);
                }
                else
                {
                    continue;
                }
            }

            height = -1;
            for (int x = grid[y].Length - 1; x >= 0; x--)
            {
                if (grid[y][x] > height)
                {
                    visibleMap[(x, y)] = true;
                    height = Math.Max(height, grid[y][x]);
                }
                else
                {
                    continue;
                }
            }
        }

        for (int x = 0; x < grid[0].Length; x++)
        {
            int height = -1;
            for (int y = 0; y < grid.Count; y++)
            {
                if (grid[y][x] > height)
                {
                    visibleMap[(x, y)] = true;
                    height = Math.Max(height, grid[y][x]);
                }
                else
                {
                    continue;
                }
            }

            height = -1;
            for (int y = grid.Count - 1; y >= 0; y--)
            {
                if (grid[y][x] > height)
                {
                    visibleMap[(x, y)] = true;
                    height = Math.Max(height, grid[y][x]);
                }
                else
                {
                    continue;
                }
            }
        }

        return visibleMap.Keys.Count;
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    static int CountTreesOfMaxHeight(List<int[]> grid, int x, int y, Direction direction)
    {
        int xStep = direction == Direction.Left ? -1 : direction == Direction.Right ? 1 : 0;
        int yStep = direction == Direction.Up ? -1 : direction == Direction.Down ? 1 : 0;

        int viewable = 0;
        int initialHeight = grid[y][x];
        for (int xCurrent = x + xStep, yCurrent = y + yStep; ;xCurrent += xStep, yCurrent += yStep)
        {
            // Check boundaries
            if (xCurrent < 0) return x;
            if (yCurrent < 0) return y;
            if (xCurrent >= grid[0].Length) return xCurrent - x - 1;
            if (yCurrent >= grid.Count) return yCurrent - y - 1;
            
            // Look until first tree >= initial height.
            if (grid[yCurrent][xCurrent] >= initialHeight)
            {
                viewable = (Math.Max(xCurrent, x) - Math.Min(xCurrent, x)) + (Math.Max(yCurrent, y) - Math.Min(yCurrent, y));
                return viewable;
            }
        }
    }

    /// <summary>
    /// Computes Scenic Score for the tree at coordinates (x, y).
    /// </summary>
    static int GetScenicScore(List<int[]> grid, int x, int y)
    {
        int up = CountTreesOfMaxHeight(grid, x, y, Direction.Up);
        int down = CountTreesOfMaxHeight(grid, x, y, Direction.Down);
        int left = CountTreesOfMaxHeight(grid, x, y, Direction.Left);
        int right = CountTreesOfMaxHeight(grid, x, y, Direction.Right);

        int score = up * down * left * right;
        return score;
    }

    static int GetHighestScenicScore(List<int[]> grid)
    {
        int highest = 0;
        
        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Length; x++)
            {
                highest = Math.Max(GetScenicScore(grid, x, y), highest);
            }
        }

        return highest;
    }
}