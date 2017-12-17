using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            int ropeLength = 256;
            string[] input = File.ReadAllText(args[0]).Split(",");
            //string[] input = "3,4,1,5".Split(",");
            int[] lengths = new int[input.Length];
            for (int i = 0; i < lengths.Length; i++)
                lengths[i] = int.Parse(input[i]);
            
            int[] rope = new int[ropeLength];
            for (int i = 0; i < ropeLength; i++)
                rope[i] = i;
            
            int currentPosition = 0;
            int skipSize = 0;
            HashRope(ref rope, lengths, ref currentPosition, ref skipSize);

            // Puzzle 1: Multiply the first two elements of rope after hashing
            Console.WriteLine($"Puzzle 1: {rope[0] * rope[1]}");

            byte[] byteLengths = File.ReadAllBytes(args[0]);
            //byte[] byteLengths = Encoding.ASCII.GetBytes("1,2,3");
            lengths = new int[byteLengths.Length + 5];
            for (int i = 0; i < byteLengths.Length; i++)
                lengths[i] = byteLengths[i];

            lengths[lengths.Length - 5] = 17;
            lengths[lengths.Length - 4] = 31;
            lengths[lengths.Length - 3] = 73;
            lengths[lengths.Length - 2] = 47;
            lengths[lengths.Length - 1] = 23;
            
            rope = new int[ropeLength];
            for (int i = 0; i < ropeLength; i++)
                rope[i] = i;

            currentPosition = 0;
            skipSize = 0;
            for (int i = 0; i < 64; i++)
            {
                HashRope(ref rope, lengths, ref currentPosition, ref skipSize);
            }

            string hash = GetHash(ref rope);
            Console.WriteLine($"Puzzle 2: Knot Hash {hash}");
        }

        static string GetHash(ref int[] rope)
        {
            int counter = 0;
            int runningXor = 0;
            string runningDenseHash = "";
            while (counter < rope.Length)
            {
                runningXor = runningXor ^ rope[counter];
                counter++;

                if (counter % 16 == 0)
                {
                    runningDenseHash += runningXor.ToString("x2");
                    runningXor = 0;
                }
            }

            return runningDenseHash;
        }

        static void HashRope(ref int[] rope, int[] lengths, ref int currentPosition, ref int skipSize)
        {
            int ropeLength = rope.Length;
            for (int i = 0; i < lengths.Length; i++)
            {
                // Reverse numbers from [currentPosition, currentPosiiton + lengths[i]
                for (int j = currentPosition; j < currentPosition + (lengths[i]) / 2; j++)
                {
                    int temp = rope[j % ropeLength];
                    rope[j % ropeLength] = rope[(currentPosition + lengths[i] - (j - currentPosition) - 1) % ropeLength];
                    rope[(currentPosition + lengths[i] - (j - currentPosition) - 1) % ropeLength] = temp;
                }

                currentPosition += lengths[i];
                currentPosition += skipSize++;
            }
        }
    }
}
