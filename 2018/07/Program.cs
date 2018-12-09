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
    public class StepRequirement
    {
        public string InStep;
        public string OutStep;

        public StepRequirement(string inStep, string outStep)
        {
            InStep = inStep;
            OutStep = outStep;
        }
    }
    public class Day07
    {
        static void Main(string[] args)
        {
            var steps = new List<StepRequirement>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                while (true)
                {
                    string line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    steps.Add(EntryFromInput(line));
                }
            }
            
            string part1 = Part1(steps);
            int part2 = Part2(steps);

            Console.WriteLine($"Part One: {part1}");
            Console.WriteLine($"Part Two: {part2}");
        }

        public static StepRequirement EntryFromInput(string line)
        {
            Regex pattern = new Regex(@"Step (.+) must be finished before step (.+) can begin.");
            MatchCollection matches = pattern.Matches(line);
            foreach (Match thisMatch in matches)
            {
                string step1 = thisMatch.Groups[1].Value;
                string step2 = thisMatch.Groups[2].Value;
                return new StepRequirement(step1, step2);
            }

            throw new InvalidDataException();
        }

        public static string Part1(List<StepRequirement> input)
        {
            var sb = new StringBuilder();
            var steps = new HashSet<string>();
            foreach (var s in input)
            {
                if (!steps.Contains(s.InStep))
                    steps.Add(s.InStep);

                if (!steps.Contains(s.OutStep))
                    steps.Add(s.OutStep);
            }

            // Set of steps that have been completed
            var doneSet = new HashSet<string>();

            while (doneSet.Count < steps.Count)
            {
                var available = new HashSet<string>(steps);
                foreach (var s in input)
                {
                    if (doneSet.Contains(s.InStep))
                    {
                        available.Remove(s.InStep);
                    }

                    if (doneSet.Contains(s.OutStep))
                    {
                        available.Remove(s.OutStep);
                    }

                    if (!doneSet.Contains(s.InStep))
                    {
                        available.Remove(s.OutStep);
                    }
                }

                List<string> remainingSteps = new List<string>(available);
                remainingSteps.Sort();

                sb.Append(remainingSteps[0]);
                doneSet.Add(remainingSteps[0]);
            }
            
            return sb.ToString();
        }

        private static string GetNextStep(HashSet<string> doneSet, List<StepRequirement> stepRequirements, HashSet<string> stepSet, HashSet<string> inProgress)
        {
            var available = new HashSet<string>(stepSet);
            foreach (var s in stepRequirements)
            {
                if (doneSet.Contains(s.InStep))
                {
                    available.Remove(s.InStep);
                }

                if (doneSet.Contains(s.OutStep))
                {
                    available.Remove(s.OutStep);
                }

                if (!doneSet.Contains(s.InStep))
                {
                    available.Remove(s.OutStep);
                }
            }

            List<string> remainingSteps = new List<string>(available);
            remainingSteps.Sort();

            foreach (var candidateStep in remainingSteps)
            {
                if (!inProgress.Contains(candidateStep))
                    return candidateStep;
            }

            // No remaining steps
            return null;
        }

        public static int Part2(List<StepRequirement> input)
        {
            int workerCount = 5;
            int stepBaseWorkLength = 60;

            var sb = new StringBuilder();
            var steps = new HashSet<string>();
            foreach (var s in input)
            {
                if (!steps.Contains(s.InStep))
                    steps.Add(s.InStep);

                if (!steps.Contains(s.OutStep))
                    steps.Add(s.OutStep);
            }

            var inProgressSteps = new HashSet<string>();
            var stepToTimeLeftMap = new Dictionary<string, int>();

            // Set of steps that have been completed
            var doneSet = new HashSet<string>();
            for (int time = 0; ; time++)
            {
                var incrementor = new HashSet<string>(inProgressSteps);
                // Decrement time remaining on active workers
                foreach (var step in incrementor)
                {
                    stepToTimeLeftMap[step]--;
                    if (stepToTimeLeftMap[step] == 0)
                    {
                        doneSet.Add(step);
                        stepToTimeLeftMap.Remove(step);
                        inProgressSteps.Remove(step);
                        sb.Append(step);

                        // Exit condition
                        if (doneSet.Count == steps.Count)
                        {
                            Console.WriteLine($"Completed after {time} seconds. Final ordering {sb.ToString()}");
                            return time;
                        }
                    }
                }

                // Try to assign workers
                while (inProgressSteps.Count < workerCount)
                {
                    string nextStep = GetNextStep(doneSet, input, steps, inProgressSteps);
                    
                    // There are no steps we can assign to workers right now
                    if (nextStep == null)
                        break;

                    inProgressSteps.Add(nextStep);
                    stepToTimeLeftMap[nextStep] = stepBaseWorkLength + "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(nextStep) + 1;
                }
            }

            // Unreachable
        }
    }
}
