using System;
using System.Collections.Generic;
using System.IO;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            var numberList = new List<int>();

            using (TextReader tr = File.OpenText("input.txt"))
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

            Part1(numberList);
            Part2(numberList);
        }

        static void Part1(IList<int> numberList)
        {
            int frequency = 0;

            foreach (var i in numberList)
            {
                frequency += i;
            }

            Console.WriteLine($"Part One: Final frequency {frequency}");
        }

        static void Part2(IList<int> numberList)
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
                        Console.WriteLine($"Part Two: Saw frequency {frequency} twice.");
                        return;
                    }
                    seen.Add(frequency);
                }
            }
        }
    }
}
