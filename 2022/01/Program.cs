﻿

List<List<int>> elves = new List<List<int>>();
List<int> currentElf = new List<int>();
elves.Add(currentElf);

using (TextReader tr = File.OpenText(args[0]))
{
    while (true)
    {
        string? line = tr.ReadLine();
        if (line == null)
            break;

        if (line == "")
        {
            currentElf = new List<int>();
            elves.Add(currentElf);
            continue;
        }

        currentElf.Add(int.Parse(line));
    }

    int part1 = Part1(elves);
    int part2 = Part2(elves);

    Console.WriteLine($"Part One: Max calories on an elf: {part1}.");
    Console.WriteLine($"Part Two: Calories on top 3 elves: {part2}.");
}

Console.WriteLine($"Found {elves.Count} elves.");

static int Part1(List<List<int>> elves)
{
    // Max calories carried by an elf.
    int maxSeen = 0;

    List<int> sums = new List<int>();
    elves.ForEach(x => sums.Add(x.Sum()));
    return sums.Max();
}

static int Part2(List<List<int>> elves)
{
    List<int> sums = new List<int>();
    elves.ForEach(x => sums.Add(x.Sum()));
    return sums.OrderBy(x => -x).Take(3).Sum();
}