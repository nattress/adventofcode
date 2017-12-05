using System;
using System.Collections.Generic;
using System.IO;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Day2 <data.txt>");
                return;
            }

            List<string> rawData = new List<string>();
            using (TextReader tr = File.OpenText(args[0]))
            {
                string line = tr.ReadLine();
                while (line != null)
                {
                    rawData.Add(line);
                    line = tr.ReadLine();
                }
            }

            RunTests();
            RunTests2();

            int result = ComputeCheckSum1(rawData);
            Console.WriteLine($"Checksum: {result}");

            result = ComputeCheckSum2(rawData);
            Console.WriteLine($"Checksum: {result}");
        }

        static void RunTests()
        {
            List<string> testData = new List<string>() { "5 1 9 5", "7 5 3", "2 4 6 8" };

            int actualValue = ComputeCheckSum1(testData);
            int expectedValue = 18;

            if (actualValue != expectedValue)
            {
                Console.WriteLine("Fail.");
                Console.WriteLine($"Expected: {expectedValue}. Actual: {actualValue}");
            }
            else
            {
                Console.WriteLine("Pass");
            }
        }

        static void RunTests2()
        {
            List<string> testData = new List<string>() { "5 9 2 8", "9 4 7 3", "3 8 6 5" };

            int actualValue = ComputeCheckSum2(testData);
            int expectedValue = 9;

            if (actualValue != expectedValue)
            {
                Console.WriteLine("Fail.");
                Console.WriteLine($"Expected: {expectedValue}. Actual: {actualValue}");
            }
            else
            {
                Console.WriteLine("Pass");
            }
        }

        static int ComputeCheckSum1(List<string> rawData)
        {
            int sum = 0;

            foreach (var line in rawData)
            {
                List<int> intTerms = new List<int>();
                string[] terms = line.Split(' ', '\t');
                foreach (string t in terms)
                {
                    intTerms.Add(int.Parse(t));
                }

                int minValue = MinValueInList(intTerms);
                int maxValue = MaxValueInList(intTerms);

                sum += maxValue - minValue;
            }

            return sum;
        }

        static int ComputeCheckSum2(List<string> rawData)
        {
            int sum = 0;

            foreach (var line in rawData)
            {
                List<int> intTerms = new List<int>();
                string[] terms = line.Split(' ', '\t');
                foreach (string t in terms)
                {
                    intTerms.Add(int.Parse(t));
                }

                for (int i = 0; i < intTerms.Count; i++)
                {
                    bool found = false;
                    for (int j = i + 1; j < intTerms.Count; j++)
                    {
                        int a = intTerms[i];
                        int b = intTerms[j];
                        
                        if (a % b == 0)
                        {
                            sum += a / b;
                            found = true;
                            break;
                        }
                        else if (b % a == 0)
                        {
                            sum += b / a;
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;

                    if (i == intTerms.Count - 1)
                        throw new InvalidDataException();
                }
            }

            return sum;
        }

        static int MinValueInList(List<int> list)
        {
            int min = int.MaxValue;
            foreach (var x in list)
            {
                min = Math.Min(min, x);
            }

            return min;
        }

        static int MaxValueInList(List<int> list)
        {
            int max = int.MinValue;
            foreach (var x in list)
            {
                max = Math.Max(max, x);
            }

            return max;
        }
    }
}
