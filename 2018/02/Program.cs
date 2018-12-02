using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Day02
    {
        static void Main(string[] args)
        {
            var boxList = new List<string>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                string line = "";
                
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    boxList.Add(line);
                }
            }

            int part1 = Part1(boxList);
            string part2 = Part2(boxList);

            Console.WriteLine($"Part One: Checksum is {part1}");
            Console.WriteLine($"Part Two: Common characters in matching box ids are {part2}");
        }

        public static int Part1(IList<string> boxes)
        {
            int twosCount = 0;
            int threesCount = 0;

            foreach (var box in boxes)
            {
                Dictionary<char, int> occurences = new Dictionary<char, int>();
                
                foreach (char c in box)
                {
                    if (!occurences.ContainsKey(c))
                    {
                        occurences.Add(c, 0);
                    }

                    occurences[c] = occurences[c] + 1;
                }

                int thisTwosCount = 0;
                int thisThreesCount = 0;
                foreach (var k in occurences.Keys)
                {
                    if (occurences[k] == 2)
                    {
                        thisTwosCount++;
                    }
                    else if (occurences[k] == 3)
                    {
                        thisThreesCount++;
                    }
                }

                if (thisTwosCount > 0)
                {
                    twosCount++;
                }

                if (thisThreesCount > 0)
                {
                    threesCount++;
                }

            }

            return twosCount * threesCount;
        }

        public static string Part2(IList<string> boxes)
        {
            foreach (var box1 in boxes)
            {
                foreach (var box2 in boxes)
                {
                    System.Diagnostics.Debug.Assert(box1.Length == box2.Length);

                    int sameCharCounter = 0;
                    int differingCharIndex = 0;
                    for (int i = 0; i < box1.Length; i++)
                    {
                        if (box1[i] == box2[i])
                        {
                            sameCharCounter++;
                        }
                        else
                        {
                            differingCharIndex = i;
                        }
                    }

                    if (sameCharCounter == box1.Length - 1)
                    {
                        string matching = box1.Remove(differingCharIndex, 1);
                        System.Diagnostics.Debug.Assert(matching == box2.Remove(differingCharIndex, 1));
                        return matching;
                    }
                }
            }

            throw new InvalidDataException("The input IDs have none that differ by one character");
        }
    }
}
