using System;
using System.IO;
using System.Collections.Generic;

namespace _09
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTests();
            int position = 0;
            int garbageCount = 0;
            using (TextReader tr = File.OpenText(args[0]))
            {
                int totalGroupScore = ComputeGroupScore(tr.ReadToEnd(), ref position, 0, ref garbageCount);

                Console.WriteLine($"Total Score: {totalGroupScore}");
                Console.WriteLine($"Total non-canceled garbage count: {garbageCount}");
                // while(true)
                // {
                //     string line = tr.ReadLine();
                //     if (string.IsNullOrEmpty(line))
                //         break;
                // }
            }
        }

        static void RunTests()
        {
            Test("{}", 1);
            Test("{{{}}}", 6);
            Test("{{},{}}", 5);
            Test("{{{},{},{{}}}}", 16);
            Test("{<a>,<a>,<a>,<a>}", 1);
            Test("{{<ab>},{<ab>},{<ab>},{<ab>}}", 9);
            Test("{{<!!>},{<!!>},{<!!>},{<!!>}}", 9);
            Test("{{<a!>},{<a!>},{<a!>},{<ab>}}", 3);

            TestGarbageCount("<>", 0);
            TestGarbageCount("<random characters>", 17);
            TestGarbageCount("<<<<>", 3);
            TestGarbageCount("<{!>}>", 2);
            TestGarbageCount("<!!>", 0);
            TestGarbageCount("<!!!>>", 0);
            TestGarbageCount("<{o\"i!a,<{i<a>", 10);
        }

        static void Test(string input, int expectedScore)
        {
            int position = 0;
            int garbageCount = 0;
            int actualScore = ComputeGroupScore(input, ref position, 0, ref garbageCount);

            Console.Write($"Testing {input}: ");
            if (actualScore != expectedScore)
            {
                Console.WriteLine($"Failed. Expected: {expectedScore}  Actual: {actualScore}");
            }
            else
            {
                Console.WriteLine("Pass");
            }
        }

        static void TestGarbageCount(string input, int expectedGarbageCount)
        {
            int position = 0;
            int actualGarbageCount = 0;
            ComputeGroupScore(input, ref position, 0, ref actualGarbageCount);

            Console.Write($"Testing {input}: ");
            if (actualGarbageCount != expectedGarbageCount)
            {
                Console.WriteLine($"Failed. Expected: {expectedGarbageCount}  Actual: {actualGarbageCount}");
            }
            else
            {
                Console.WriteLine("Pass");
            }
        }

        static int ComputeGroupScore(string input, ref int pos, int nestingLevel, ref int garbageCount)
        {
            int scoresInThisGroup = 0;
            while (pos < input.Length)
            {
                //
                // The possible inputs:
                //
                //  '{' Start a new group
                //  '}' End the current group
                //  ',' Next group / garbage in the current group
                //  '<' Begin garbage sequence
                switch (input[pos++])
                {
                    case '{':
                        scoresInThisGroup += ComputeGroupScore(input, ref pos, nestingLevel + 1, ref garbageCount);
                        break;
                    case '}':
                        return scoresInThisGroup + nestingLevel;
                    case ',':
                        // Do nothing - iterate another time
                        break;
                    case '<':
                        garbageCount += SkipGarbage(input, ref pos);
                        break;
                }
            }

            return scoresInThisGroup;
        }

        static int SkipGarbage(string input, ref int pos)
        {
            int countOfNonCancelledGarbage = 0;
            while (pos < input.Length)
            {
                //
                // The possible inputs:
                //
                //  '!' Skip the next character. Ie, !> would not close the garbage sequence
                //  '>' End garbage
                //
                switch (input[pos++])
                {
                    case '!':
                        // Skip escaped character
                        pos++;
                        break;
                    case '>':
                        return countOfNonCancelledGarbage;
                    default:
                        // Any other character is garbage - skip
                        countOfNonCancelledGarbage++;
                        break;
                }
            }

            throw new InvalidDataException("Invalid format of garbage");
        }
    }
}
