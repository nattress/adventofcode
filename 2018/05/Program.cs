using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day05
    {
        static void Main(string[] args)
        {
            string input = "";
            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    input = tr.ReadLine();
                    break;
                }
            }
            
            int part1 = Part1(input);
            int part2 = Part2(input);

            Console.WriteLine($"Part One: Units remaining {part1}");
            Console.WriteLine($"Part Two: {part2}");
        }

        public static string ReducePolymer(string input)
        {
            bool reductionHappened = true;
            while(reductionHappened)
            {
                reductionHappened = false;
                string lower = "abcdefghijklmnopqrstuvwxyz";
                string upper = lower.ToUpper();

                for (int i = 0; i < input.Length - 1; i++)
                {
                    if ((lower.Contains(input[i]) && upper.Contains(input[i + 1]) || upper.Contains(input[i]) && lower.Contains(input[i + 1])) && input.Substring(i, 1).ToUpper() == input.Substring(i + 1, 1).ToUpper())
                    {
                        reductionHappened = true;
                        input = input.Remove(i, 2);
                    }
                }
            }

            return input;
        }

        public static int Part1(string input)
        {
            string result = ReducePolymer(input);

            return result.Length;
        }

        public static int Part2(string input)
        {
            int minPolymerLength = int.MaxValue;
            
            // Collapse any starting polymers to reduce the search space
            input = ReducePolymer(input);
            
            Parallel.ForEach("abcdefghijklmnopqrstuvwxyz", (c) => 
            {
                string candidate = input.Replace(c.ToString(), "", true, CultureInfo.InvariantCulture);
                candidate = ReducePolymer(candidate);
                minPolymerLength = Math.Min(candidate.Length, minPolymerLength);
            });
            
            return minPolymerLength;
        }
    }
}
