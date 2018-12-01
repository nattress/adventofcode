using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Day01
    {
        static void Main(string[] args)
        {
            var numberList = new List<int>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                string line = "";
                
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    numberList.Add(int.Parse(line));
                }
            }

            int part1 = Part1(numberList);
            int part2 = Part2(numberList);

            Console.WriteLine($"Part One: Final frequency {part1}");
            Console.WriteLine($"Part Two: Saw frequency {part2} twice.");
        }

        public static int Part1(IList<int> numberList)
        {
            int frequency = 0;

            foreach (var i in numberList)
            {
                frequency += i;
            }

            return frequency;
        }

        public static int Part2(IList<int> numberList)
        {
            var seen = new HashSet<int>();
            int frequency = 0;

            for (;;)
            {
                foreach (var i in numberList)
                {
                    frequency += i;
                    if (seen.Contains(frequency))
                    {
                        return frequency;
                    }
                    seen.Add(frequency);
                }
            }
        }
    }
}
