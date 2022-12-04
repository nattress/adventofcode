public class AdventOfCode
{
    enum Move
    {
        Rock,
        Paper,
        Scissors
    }

    public static void Main(string[] args)
    {
        List<(Move, Move)> moveList = new List<(Move, Move)>();

        using (TextReader tr = File.OpenText(args[0]))
        {
            while (true)
            {
                string? line = tr.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                string[] moves = line.Split(' ');
                Move them = moves[0] == "A" ? Move.Rock : moves[0] == "B" ? Move.Paper : Move.Scissors;
                Move us = moves[1] == "X" ? Move.Rock : moves[1] == "Y" ? Move.Paper : Move.Scissors;
                moveList.Add((them, us));
            }

            int part1 = Part1(moveList);
            int part2 = Part2(moveList);

            Console.WriteLine($"Part One: {part1}.");
            Console.WriteLine($"Part Two: {part2}.");
        }
    }

    static int ScoreGame(Move them, Move us)
    {
        int score = us == Move.Rock ? 1 : us == Move.Paper ? 2 : 3;

        if (them == us)
        {
            // Draw.
            return score + 3;
        }

        if (us == Move.Rock && them == Move.Scissors ||
            us == Move.Paper && them == Move.Rock ||
            us == Move.Scissors && them == Move.Paper)
        {
            return score + 6;
        }

        return score;
    }

    static int Part1(List<(Move, Move)> moves)
    {
        int score = 0;
        foreach ((Move them, Move us) in moves)
        {
            score += ScoreGame(them, us);
        }

        return score;
    }

    static int Part2(List<(Move, Move)> moves)
    {
        // us move is now X == Lose, Y == Draw, Z == Win.
        int score = 0;

        foreach ((Move them, Move us) in moves)
        {
            // Initialize as draw.
            Move newUs = them;

            // Need to beat "them"
            if (us == Move.Scissors)
            {
                newUs = them == Move.Rock ? Move.Paper : them == Move.Paper ? Move.Scissors : Move.Rock;
            }
            else if (us == Move.Rock)
            {
                newUs = them == Move.Rock ? Move.Scissors : them == Move.Paper ? Move.Rock : Move.Paper;
            }

            score += ScoreGame(them, newUs);
        }

        return score;

    }
}