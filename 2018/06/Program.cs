using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public struct Coordinate
    {
        public int X;
        public int Y;

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Day06
    {
        static void Main(string[] args)
        {
            var coords = new List<Coordinate>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    var split = line.Split(new[] {',', ' '});
                    coords.Add(new Coordinate(int.Parse(split[0]), int.Parse(split[2])));
                }
            }
            
            int part1 = Part1(coords);
            int part2 = Part2(coords);

            Console.WriteLine($"Part One: Largest area {part1}");
            Console.WriteLine($"Part Two: {part2}");
        }

        /// <summary>
        /// Determines whether the coordinate at "index" has at least one direction that it will
        /// infinitely dominate. For a given coordinate A, if there is no other coordinate in the 90
        /// degree arc out in a cardinal direction, then A will dominate infinitely.
        /// </summary>
        private static bool IsInfinite(List<Coordinate> coords, int index)
        {
            Coordinate center = coords[index];
            bool infiniteNorth = true;
            bool infiniteEast = true;
            bool infiniteSouth = true;
            bool infiniteWest = true;

            for (int i = 0; i < coords.Count; i++)
            {
                // Best not worry about ourself
                if (i == index)
                    continue;

                int deltaX = Math.Abs(coords[i].X - center.X);
                int deltaY = Math.Abs(coords[i].Y - center.Y);

                // North
                if (coords[i].Y < center.Y && deltaY >= deltaX)
                {
                    // At least one coordinate is in the northern arc, so we cannot dominate infinitely
                    infiniteNorth = false;
                }

                // East
                if (coords[i].X > center.X && deltaX >= deltaY)
                    infiniteEast = false;

                // South
                if (coords[i].Y > center.Y && deltaY >= deltaX)
                    infiniteSouth = false;

                // West
                if (coords[i].X < center.X && deltaX >= deltaY)
                    infiniteWest = false;
            }

            return infiniteNorth || infiniteEast || infiniteSouth || infiniteWest;
        }

        static int ManHattanDistance(Coordinate coordinate1, Coordinate coordinate2)
            => Math.Abs(coordinate1.X - coordinate2.X) + Math.Abs(coordinate1.Y - coordinate2.Y);
        
        /// <summary>
        /// Returns the top-left and bottom-right coordinates forming a box containing all the coordinates
        /// in "coords". The amount of extra padding around the coordinates (mainly to visualize infinity)
        /// is controlled by "padding".
        /// </summary>
        static (Coordinate, Coordinate) GetViewExtents(List<Coordinate> coords, int padding)
        {
            // Figure out the most extra values for -x, x, -y, y to build the box
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            foreach (Coordinate c in coords)
            {
                minX = Math.Min(minX, c.X);
                minY = Math.Min(minY, c.Y);
                maxX = Math.Max(maxX, c.X);
                maxY = Math.Max(maxY, c.Y);
            }

            Coordinate topLeft = new Coordinate(minX - padding, minY - padding);
            Coordinate bottomRight = new Coordinate(maxX + padding, maxY + padding);
            return (topLeft, bottomRight);
        }

        static int ClosestCoordinateIndexFromPoint(Coordinate startingPoint, List<Coordinate> coords)
        {
            int closestDistance = int.MaxValue;
            int closestIndex = -1;
            var distanceCounter = new Dictionary<int, int>(); // Maps a distance from startingPoint to the number of coordinates that far away to check for ties
            for (int i = 0; i < coords.Count; i++)
            {
                int distance = ManHattanDistance(startingPoint, coords[i]);
                
                distanceCounter[distance] = distanceCounter.ContainsKey(distance) ? distanceCounter[distance] + 1 : 1;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            if (distanceCounter[closestDistance] > 1)
                return -1;

            return closestIndex;
        }

        private static char GetCoordinateLabel(List<Coordinate> coords, int index, Coordinate currentPosition)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return alphabet[index];
        }

        public static int Part1(List<Coordinate> input)
        {
            var extents = GetViewExtents(input, 3);
            
            char[,] view = new char[Math.Abs(extents.Item2.X - extents.Item1.X) + 1, Math.Abs(extents.Item2.Y - extents.Item1.Y) + 1];
            int[] territoryCounter = new int[input.Count];
            // Wastefully fill in this extent rectangle with the location of the closest coordinate for each point
            for (int x = extents.Item1.X; x <= extents.Item2.X; x++)
            {
                for (int y = extents.Item1.Y; y <= extents.Item2.Y; y++)
                {
                    Coordinate thisCoord = new Coordinate(x, y);
                    // What's the closest coordinate in input from here
                    int closestIndex = ClosestCoordinateIndexFromPoint(thisCoord, input);
                    if (closestIndex >= 0)
                    {
                        territoryCounter[closestIndex]++;
                        view[x - extents.Item1.X, y - extents.Item1.Y] = GetCoordinateLabel(input, closestIndex, thisCoord);
                    }
                    else
                    {
                        view[x - extents.Item1.X, y - extents.Item1.Y] = '.';
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < view.GetLength(0); x++)
            {
                for (int y = 0; y < view.GetLength(1); y++)
                {
                    sb.Append(view[x, y]);
                }

                sb.AppendLine("");
            }

            // Emit the graphical view of coordinates (cool to look at)
            //Console.WriteLine(sb.ToString());

            int max = 0;
            int maxIndex = 0;
            for (int i = 0; i < territoryCounter.Length; i++)
            {
                if (IsInfinite(input, i))
                    continue;

                if (territoryCounter[i] > max)
                {
                    max = territoryCounter[i];
                    maxIndex = i;
                }
            }

            Console.WriteLine($"Largest territory is {maxIndex} with area {max}");
            return max;
        }

        public static int Part2(List<Coordinate> input)
        {
            int targetDistance = 10000;
            // Set the extend padding to be 10000 / the number of coordinates, reasoning that if they're all clustered together,
            // the furthest point whose sum of Manhattan distances is <= 10000 cannot be more than that away in optimal case.
            var extents = GetViewExtents(input, targetDistance / input.Count);
            char[,] view = new char[Math.Abs(extents.Item2.X - extents.Item1.X) + 1, Math.Abs(extents.Item2.Y - extents.Item1.Y) + 1];
            int regionCounter = 0;
            // Wastefully fill in this extent rectangle with the location of the closest coordinate for each point
            for (int x = extents.Item1.X; x <= extents.Item2.X; x++)
            {
                for (int y = extents.Item1.Y; y <= extents.Item2.Y; y++)
                {
                    Coordinate thisCoord = new Coordinate(x, y);

                    int totalDistance = 0;
                    bool skipViewUpdate = false;
                    for (int i = 0; i < input.Count; i++)
                    {
                        int manhattanDistance = ManHattanDistance(thisCoord, input[i]);
                        totalDistance += manhattanDistance;

                        if (manhattanDistance == 0)
                        {
                            // We're on an input coordinate
                            view[x - extents.Item1.X, y - extents.Item1.Y] = GetCoordinateLabel(input, i, thisCoord);
                            skipViewUpdate = true;
                        }
                        if (totalDistance >= targetDistance)
                            break;
                    }

                    if (totalDistance < targetDistance)
                    {
                        regionCounter++;
                        if (!skipViewUpdate)
                            view[x - extents.Item1.X, y - extents.Item1.Y] = '#';
                    }
                    else if (!skipViewUpdate)
                    {
                        view[x - extents.Item1.X, y - extents.Item1.Y] = '.';
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < view.GetLength(1); y++)
            {
                for (int x = 0; x < view.GetLength(0); x++)
                {
                    sb.Append(view[x, y]);
                }

                sb.AppendLine("");
            }

            // Emit the graphical view of coordinates (cool to look at)
            //Console.WriteLine(sb.ToString());

            return regionCounter;
        }
    }
}
