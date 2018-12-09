using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Marble
    {
        public Marble Clockwise;
        public Marble CounterClockwise;
        public int Value;

        public Marble(int value)
        {
            Value = value;
        }
    }

    public class Day09
    {
        static void Main(string[] args)
        {
            string input = "";
            int playerCount = 0;
            int lastMarbleValue = 0;

            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    input = line;
                    Regex pattern = new Regex(@"([0-9]+) players; last marble is worth ([0-9]+) points");
                    MatchCollection matches = pattern.Matches(line);
                    foreach (Match thisMatch in matches)
                    {
                        playerCount = int.Parse(thisMatch.Groups[1].Value);
                        lastMarbleValue  = int.Parse(thisMatch.Groups[2].Value);
                    }
                }
            }
            
            long part1 = Part1(playerCount, lastMarbleValue);
            long part2 = Part2(playerCount, lastMarbleValue * 100);

            Console.WriteLine($"Part One: {part1}");
            Console.WriteLine($"Part Two: {part2}");
        }

        static void PrintMarbles(Marble current, Marble start)
        {
            Marble m = start;
            while (true)
            {
                if (m == current)
                {
                    Console.Write($"({m.Value}) ");
                }
                else
                {
                    Console.Write(m.Value + " ");
                }
                
                m = m.Clockwise;
                if (m == start)
                {
                    Console.WriteLine("");
                    return;
                }
            }
        }

        private static long ComputeHighestScore(int playerCount, int lastMarbleValue)
        {
            Marble currentMarble = new Marble(0);
            currentMarble.Clockwise = currentMarble;
            currentMarble.CounterClockwise = currentMarble;

            long[] playerScores = new long[playerCount];

            int playerNum = 0;
            Marble start = currentMarble;
            
            // PrintMarbles(currentMarble, start);
            
            for (int marbleNum = 1; marbleNum <= lastMarbleValue; marbleNum++)
            {
                if (marbleNum % 23 == 0)
                {
                    // Something different happens
                    playerScores[playerNum] += marbleNum;
                    Marble toRemove = currentMarble.CounterClockwise.CounterClockwise.CounterClockwise.CounterClockwise.CounterClockwise.CounterClockwise.CounterClockwise;
                    currentMarble = toRemove.Clockwise;
                    toRemove.CounterClockwise.Clockwise = currentMarble;
                    currentMarble.CounterClockwise = toRemove.CounterClockwise;
                    playerScores[playerNum] += toRemove.Value;
                }
                else
                {
                    Marble insertionPoint = currentMarble.Clockwise;
                    Marble newMarble = new Marble(marbleNum);
                    Marble insertionPointClockwiseMarble = insertionPoint.Clockwise;
                    insertionPoint.Clockwise = newMarble;
                    insertionPointClockwiseMarble.CounterClockwise = newMarble;
                    newMarble.Clockwise = insertionPointClockwiseMarble;
                    newMarble.CounterClockwise = insertionPoint;
                    currentMarble = newMarble;
                }

                //PrintMarbles(currentMarble, start);

                playerNum++;
                if (playerNum >= playerCount)
                    playerNum = 0;

                
            }
            
            return playerScores.Max();
        }

        public static long Part1(int playerCount, int lastMarbleValue)
        {
            return ComputeHighestScore(playerCount, lastMarbleValue);
        }

        public static long Part2(int playerCount, int lastMarbleValue)
        {
            return ComputeHighestScore(playerCount, lastMarbleValue);
        }
    }
}
