using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public struct Claim
    {
        public int Id;
        public int Left;
        public int Top;
        public int Width;
        public int Height;
        public Claim(int id, int left, int top, int width, int height)
        {
            Id = id;
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
    }
    public class Day03
    {
        static void Main(string[] args)
        {
            var claimList = new List<Claim>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                string line = "";
                
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    // Example input:
                    // #6 @ 1,184: 16x24
                    Regex pattern = new Regex(@"#([0-9]+) @ ([0-9]+),([0-9]+): ([0-9]+)x([0-9]+)");
                    MatchCollection matches = pattern.Matches(line);
                    foreach (Match thisMatch in matches)
                    {
                        int id = int.Parse(thisMatch.Groups[1].Value);
                        int left = int.Parse(thisMatch.Groups[2].Value);
                        int top = int.Parse(thisMatch.Groups[3].Value);
                        int width = int.Parse(thisMatch.Groups[4].Value);
                        int height = int.Parse(thisMatch.Groups[5].Value);

                        claimList.Add(new Claim(id, left, top, width, height));
                    }
                }
            }
            
            int part1 = Part1(claimList);
            int part2 = Part2(claimList);

            Console.WriteLine($"Part One: Square inches of fabric with >= 2 claims {part1}");
            Console.WriteLine($"Part Two: Id of unique claim {part2}");
        }

        private static Dictionary<(int, int),List<Claim>> BuildClaimCounts(IList<Claim> claims)
        {
            var claimCount = new Dictionary<(int, int),List<Claim>>();

            foreach (var claim in claims)
            {
                for (int x = 0; x < claim.Width; x++)
                {
                    for (int y = 0; y < claim.Height; y++)
                    {
                        int currentX = claim.Left + x;
                        int currentY = claim.Top + y;

                        if (!claimCount.ContainsKey((currentX, currentY)))
                        {
                            claimCount[(currentX, currentY)] = new List<Claim>();
                        }
                        
                        claimCount[(currentX, currentY)].Add(claim);
                    }
                }
            }

            return claimCount;
        }
        public static int Part1(IList<Claim> claims)
        {
            var claimCount = BuildClaimCounts(claims);
            int numberInContention = 0;
            foreach (var claim in claimCount.Values)
            {
                if (claim.Count > 1)
                    numberInContention++;
            }

            return numberInContention;
        }

        public static int Part2(IList<Claim> claims)
        {
            var claimCount = BuildClaimCounts(claims);
            var singleClaims = new HashSet<Claim>(claims);
            
            foreach (var claimList in claimCount.Values)
            {
                if(claimList.Count == 1)
                    continue;
                
                foreach (var c in claimList)
                {
                    if (singleClaims.Contains(c))
                        singleClaims.Remove(c);
                }
            }

            if (singleClaims.Count != 1)
            {
                Console.WriteLine($"Error, expected a unique claim with no overlaps but got {singleClaims.Count} claims.");
                return 0;
            }

            foreach (var c in singleClaims)
            {
                return c.Id;
            }

            throw new InvalidProgramException("No unique claims found");
        }
    }
}
