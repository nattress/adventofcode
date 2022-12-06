public class AdventOfCode
{
    public static void Main(string[] args)
    {
        string part1 = GetStackTops(true);
        string part2 = GetStackTops(false);
        
        Console.WriteLine($"Part One: {part1}.");
        Console.WriteLine($"Part Two: {part2}.");
    }

    static string GetStackTops(bool part1)
    {
        using TextReader tr = File.OpenText("input_01.txt");
        
        bool parsingStacks = true;
        List<Stack<string>> stacks = new List<Stack<string>>();
        while (true)
        {
            string? line = tr.ReadLine();
            if (line == null)
                break;
            if (string.IsNullOrEmpty(line))
                continue;
            
            // Detect end of stack data with the numbers
            if (parsingStacks)
            {
                if (line.Contains('1'))
                {
                    parsingStacks = false;
                    
                    // Reverse all stacks now we've parsed them all (since we parsed top-down).
                    Queue<string> swap = new Queue<string>();

                    for (int i = 0; i < stacks.Count; i++)
                    {
                        while (stacks[i].Count > 0)
                        {
                            swap.Enqueue(stacks[i].Pop());
                        }
                        while (swap.Count > 0)
                        {
                            stacks[i].Push(swap.Dequeue());
                        }
                    }
                    continue;
                }
                else
                {
                    // Each stack is 4 characters wide
                    for (int i = 0; i < line.Length; i += 4)
                    {
                        string packageBytes = line.Substring(i, Math.Min(4, line.Length - i));
                        if (packageBytes.Trim() == "")
                            continue;

                        string package = packageBytes.Trim()[1].ToString();
                        while (stacks.Count <= i / 4)
                        {
                            stacks.Add(new Stack<string>());
                        }
                        stacks[i / 4].Push(package);
                    }
                }
            }
            else
            {
                int[] moves = line.Split(new string[] {"move", "from", "to"}, StringSplitOptions.TrimEntries)
                    .Skip(1)
                    .Select((item, index) => int.Parse(item))
                    .ToArray();

                if (part1)
                {
                    for (int i = 0; i < moves[0]; i++)
                    {
                        stacks[moves[2] - 1].Push(stacks[moves[1] - 1].Pop());
                    }
                }
                else
                {
                    Stack<string> swap = new Stack<string>();
                    for (int i = 0; i < moves[0]; i++)
                    {
                        swap.Push(stacks[moves[1] - 1].Pop());
                    }
                    for (int i = 0; i < moves[0]; i++)
                    {
                        stacks[moves[2] - 1].Push(swap.Pop());
                    }
                }

                Console.WriteLine(string.Join("", stacks.Select((stack, index) => stack.TryPeek(out string r) ? r : "").ToArray()));
            }
        }

        return string.Join("", stacks.Select((stack, index) => stack.Peek()).ToArray());
    }
}