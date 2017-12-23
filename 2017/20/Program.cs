using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace _20
{
    class Program
    {
        class Point3D
        {
            public int X {get;set;}
            public int Y {get;set;}
            public int Z {get;set;}

            public Point3D(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return $"<{X},{Y},{Z}>";
            }
        }

        class Particle
        {
            public Point3D Position {get;set;}
            public Point3D Velocity {get;set;}
            public Point3D Acceleration {get;set;}

            public Particle(Point3D position, Point3D velocity, Point3D acceleration)
            {
                Position = position;
                Velocity = velocity;
                Acceleration = acceleration;
            }

            public override string ToString()
            {
                return $"p={Position.ToString()}, v={Velocity.ToString()}, a={Acceleration.ToString()}";
            }
        }

        static void Main(string[] args)
        {
            List<double> distancesFromOrigin = new List<double>();
            
            using (TextReader tr = File.OpenText(args[0]))
            {
                string dataPattern = "(p=<)([-0-9]+),([-0-9]+),([-0-9]+)(>, v=<)([-0-9]+),([-0-9]+),([-0-9]+)(>, a=<)([-0-9]+),([-0-9]+),([-0-9]+)>"; //,71,1322>, v=<174,17,-93>, a=<-6,-1,3>";
                string line = "";
                List<Particle> particles = new List<Particle>();

                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    var matches = Regex.Matches(line, dataPattern);
                    foreach (Match m in matches)
                    {
                        var position = new Point3D(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));
                        var velocity = new Point3D(int.Parse(m.Groups[6].Value), int.Parse(m.Groups[7].Value), int.Parse(m.Groups[8].Value));
                        var acceleration = new Point3D(int.Parse(m.Groups[10].Value), int.Parse(m.Groups[11].Value), int.Parse(m.Groups[12].Value));
                        Particle p = new Particle(position, velocity, acceleration);
                        particles.Add(p);
                        Console.WriteLine(p.ToString());
                    }
                }

                DisplayClosestAfterTickCount(particles, 1000000);

            }
        }

        static void DisplayClosestAfterTickCount(List<Particle> particles, int timeInTicks)
        {
            var positions = ComputePositionsAfterTickCount(particles, timeInTicks, out int minIndex);
            
            Console.WriteLine($"Closest particle after {timeInTicks} ticks: {minIndex}. p={particles[minIndex].Position} Distance={positions[minIndex]}");

            // positions.Sort();
            // for (int i = 0; i < 10; i++)
            // {
            //     Console.WriteLine($"{positions[i]}");
            // }
        }

        static List<double> ComputePositionsAfterTickCount(List<Particle> particles, int timeInTicks, out int minIndex)
        {
            double minManhattanDistance = double.MaxValue;
            minIndex = -1;
            List<double> distancesFromOrigin = new List<double>();
            for (int i = 0; i < particles.Count; i++)
            {
                var position = particles[i].Position;
                var velocity = particles[i].Velocity;
                var acceleration = particles[i].Acceleration;
                var posX = position.X + (velocity.X * timeInTicks) + 0.5f * acceleration.X * Math.Pow(timeInTicks, 2);
                var posY = position.Y + (velocity.Y * timeInTicks) + 0.5f * acceleration.Y * Math.Pow(timeInTicks, 2);
                var posZ = position.Z + (velocity.Z * timeInTicks) + 0.5f * acceleration.Z * Math.Pow(timeInTicks, 2);

                double manhattanDistance = Math.Abs(posX) + Math.Abs(posY) + Math.Abs(posZ);
                distancesFromOrigin.Add(manhattanDistance);
                if (manhattanDistance < minManhattanDistance)
                {
                    minManhattanDistance = manhattanDistance;
                    minIndex = i;
                }
            }

            return distancesFromOrigin;
        }
    }
}
