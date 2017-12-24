using System;
using System.Collections.Generic;
using System.IO;

namespace _15
{
    class Generator
    {
        public long PreviousValue {get;set;}
        public long MultiplicationFactor {get;set;}
        int _moduloRequirement;
        
        public Generator(int seed, int multiplicationFactor, int moduloRequirement)
        {
            PreviousValue = seed;
            MultiplicationFactor = multiplicationFactor;
            _moduloRequirement = moduloRequirement;
        }

        public long GetNextValue()
        {
            while(true)
            {
                PreviousValue = PreviousValue * MultiplicationFactor % 2147483647;
                if (PreviousValue % _moduloRequirement == 0)
                    break;
            }
            
            return PreviousValue;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int matchCount = CountGeneratorMatches(618, 814, 40000000, 1, 1);
            Console.WriteLine($"Part 1: Found {matchCount} matches.");

            matchCount = CountGeneratorMatches(618, 814, 5000000, 4, 8);
            Console.WriteLine($"Part 2: Found {matchCount} matches.");
        }

        static int CountGeneratorMatches(int seedA, int seedB, int iterationCount, int moduloRequirementA, int moduluRequirementB)
        {
            Generator a = new Generator(seedA, 16807, moduloRequirementA);
            Generator b = new Generator(seedB, 48271, moduluRequirementB);

            int matchCount = 0;
            for (int i = 0; i < iterationCount; i++)
            {
                long first = a.GetNextValue();
                long second = b.GetNextValue();
                if ((first & 0xffff) == (second & 0xffff))
                {
                    matchCount++;
                }
            }

            return matchCount;
        }
    }
}
