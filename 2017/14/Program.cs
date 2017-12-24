using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _14
{
    class Program
    {
        static void Main(string[] args)
        {
            int usedSpaceCount = 0;
            string inputHash = "vbqugkhl";
            int[,] grid = new int[128, 128];

            for (int i = 0; i < 128; i++)
            {
                var lineHash = inputHash + "-" + i;
                string computedHash = ComputeHash(Encoding.ASCII.GetBytes(lineHash));
                
                for (int j = 0; j < 32; j++)
                {
                    var hashChar = computedHash.Substring(j, 1);
                    int intHashChar = int.Parse(hashChar, System.Globalization.NumberStyles.HexNumber);

                    if ((intHashChar & 0x1) != 0)
                    {
                        usedSpaceCount++;
                        grid[i, j * 4 + 3] = -1;
                    }
                    if ((intHashChar & 0x2) != 0)
                    {
                        usedSpaceCount++;
                        grid[i, j * 4 + 2] = -1;
                    }
                    if ((intHashChar & 0x4) != 0)
                    {
                        usedSpaceCount++;
                        grid[i, j * 4 + 1] = -1;
                    }
                    if ((intHashChar & 0x8) != 0)
                    {
                        usedSpaceCount++;
                        grid[i, j * 4] = -1;
                    }
                }
            }

            Console.WriteLine($"{usedSpaceCount} spaces used");

            int groupCount = 0;
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    if (grid[i, j] == -1)
                    {
                        // Unvisited block. Start a new group.
                        FloodFillGrid(ref grid, i, j, ++groupCount);
                    }
                }
            }

            Console.WriteLine($"Group count: {groupCount}");
        }

        static void FloodFillGrid(ref int[,] grid, int startI, int startJ, int numberToFill)
        {
            if (startI < 0 || startI > 127 || startJ < 0 || startJ > 127)
                return;
            
            if (grid[startI, startJ] == -1)
            {
                grid[startI, startJ] = numberToFill;
                FloodFillGrid(ref grid, startI + 1, startJ, numberToFill);
                FloodFillGrid(ref grid, startI - 1, startJ, numberToFill);
                FloodFillGrid(ref grid, startI, startJ + 1, numberToFill);
                FloodFillGrid(ref grid, startI, startJ - 1, numberToFill);
            }
        }

        static string ComputeHash(byte[] input)
        {
            int ropeLength = 256;
            int currentPosition = 0;
            int skipSize = 0;
            
            byte[] byteLengths = input;
            int[] lengths = new int[byteLengths.Length + 5];
            for (int i = 0; i < byteLengths.Length; i++)
                lengths[i] = byteLengths[i];

            lengths[lengths.Length - 5] = 17;
            lengths[lengths.Length - 4] = 31;
            lengths[lengths.Length - 3] = 73;
            lengths[lengths.Length - 2] = 47;
            lengths[lengths.Length - 1] = 23;
            
            int[] rope = new int[ropeLength];
            for (int i = 0; i < ropeLength; i++)
                rope[i] = i;

            for (int i = 0; i < 64; i++)
            {
                HashRope(ref rope, lengths, ref currentPosition, ref skipSize);
            }

            string hash = GetHash(ref rope);
            return hash;
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
