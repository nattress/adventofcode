using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace _20
{
    class Program
    {
        class Point3D
        {
            public long X {get;set;}
            public long Y {get;set;}
            public long Z {get;set;}

            public Point3D(long x, long y, long z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return $"<{X},{Y},{Z}>";
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Point3D))
                    return false;

                Point3D other = obj as Point3D;
                return X == other.X && Y == other.Y && Z == other.Z;
            }

            public static Point3D operator +(Point3D p1, Point3D p2)
            {
                return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
            }

            public override int GetHashCode()
            {
                return (int)(X ^ 13 + Y ^ 29 + Z ^ 31);
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

                // Part 2 - eliminate particles that collide.
                // There might be a clever equation we can plug in like Part 1 but 
                // the inaccuracy of floating math over large tick counts seems to
                // be preventing collision detection.
                // Let's step by tick, measure each position, add positions to hash
                // and eliminate collisions
                int tickCount = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                long lastReportedCollision = 0;
                while (true)
                {
                    Dictionary<Point3D, List<int>> collisions = new Dictionary<Point3D, List<int>>();
                    for (int i = 0; i < particles.Count; i++)
                    {
                        particles[i].Velocity += particles[i].Acceleration;
                        particles[i].Position += particles[i].Velocity;
                        var pos = particles[i].Position;
                        
                        if (collisions.ContainsKey(pos))
                        {
                            collisions[pos].Add(i);
                        }
                        else
                        {
                            collisions.Add(pos, new List<int>() {i});
                        }
                    }
                    
                    List<Particle> collidedParticles = new List<Particle>();
                    // Report any collisions
                    // Lordy this is going to be awful perf :)
                    foreach (var x in collisions)
                    {
                        if (x.Value.Count > 1)
                        {
                            Console.WriteLine($"Tick {tickCount}: collisions at {x.Key.ToString()}:");
                            // Collision
                            foreach (var y in x.Value)
                            {
                                Console.WriteLine($"\t{particles[y].ToString()}");
                                collidedParticles.Add(particles[y]);
                            }
                            lastReportedCollision = sw.ElapsedMilliseconds;
                        }
                    }

                    // Remove collided particles by object from the particles list
                    // since indexes will be shifting around. We could order the indices from 
                    // highest to lowest and remove that way, too.
                    foreach (var x in collidedParticles)
                    {
                        particles.Remove(x);
                    }

                    if (sw.ElapsedMilliseconds - lastReportedCollision > 10000)
                    {
                        // Every 10 seconds report our tick count
                        Console.WriteLine($"Tick {tickCount}: remaining particles: {particles.Count}");
                        lastReportedCollision = sw.ElapsedMilliseconds;
                    }

                    ++tickCount;
                }
            }
        }

        static void DisplayClosestAfterTickCount(List<Particle> particles, int timeInTicks)
        {
            var positions = ComputePositionsAfterTickCount(particles, timeInTicks, out int minIndex);
            
            Console.WriteLine($"Closest particle after {timeInTicks} ticks: {minIndex}. p={particles[minIndex].Position} Distance={positions[minIndex]}");
        }

        static Point3D PositionAfterTicks(Particle p, int timeInTicks)
        {
            var position = p.Position;
            var velocity = p.Velocity;
            var acceleration = p.Acceleration;
            var posX = (double)position.X + (double)((double)velocity.X * (double)timeInTicks) + (double)(0.5f * (double)acceleration.X * Math.Pow(timeInTicks, 2));
            var posY = (double)position.Y + (double)((double)velocity.Y * (double)timeInTicks) + (double)(0.5f * (double)acceleration.Y * Math.Pow(timeInTicks, 2));
            var posZ = (double)position.Z + (double)((double)velocity.Z * (double)timeInTicks) + (double)(0.5f * (double)acceleration.Z * Math.Pow(timeInTicks, 2));
            return new Point3D((long)posX, (long)posY, (long)posZ);
        }

        static List<double> ComputePositionsAfterTickCount(List<Particle> particles, int timeInTicks, out int minIndex)
        {
            double minManhattanDistance = double.MaxValue;
            minIndex = -1;
            List<double> distancesFromOrigin = new List<double>();
            for (int i = 0; i < particles.Count; i++)
            {
                var newPos = PositionAfterTicks(particles[i], timeInTicks);
                double manhattanDistance = Math.Abs(newPos.X) + Math.Abs(newPos.Y) + Math.Abs(newPos.Z);
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
