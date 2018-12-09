using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class AdventOfCodeTests
    {
        [Fact]
        public void Day01Part1()
        {
            List<int> numberList = new List<int>();
            ReadFile(TestInputFileFromDay(1), x => numberList.Add(int.Parse(x)));
            var result = Day01.Part1(numberList);
            Assert.Equal(411, result);
            
        }

        [Fact]
        public void Day01Part2()
        {
            List<int> numberList = new List<int>();
            ReadFile(TestInputFileFromDay(1), x => numberList.Add(int.Parse(x)));
            var result = Day01.Part2(numberList);
            Assert.Equal(56360, result);
        }

        [Fact]
        public void Day02Part1()
        {
            List<string> boxList = new List<string>();
            ReadFile(TestInputFileFromDay(2), x => boxList.Add(x));
            var result = Day02.Part1(boxList);
            Assert.Equal(7192, result);
        }

        [Fact]
        public void Day02Part2()
        {
            List<string> boxList = new List<string>();
            ReadFile(TestInputFileFromDay(2), x => boxList.Add(x));
            var result = Day02.Part2(boxList);
            Assert.Equal("mbruvapghxlzycbhmfqjonsie", result);
        }

        [Fact]
        public void Day03Part1()
        {
            List<Claim> claimList = new List<Claim>();
            ReadFile(TestInputFileFromDay(3), line => 
            {
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
            });
            var result = Day03.Part1(claimList);
            Assert.Equal(118539, result);
        }

        [Fact]
        public void Day03Part2()
        {
            List<Claim> claimList = new List<Claim>();
            ReadFile(TestInputFileFromDay(3), line => 
            {
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
            });
            var result = Day03.Part2(claimList);
            Assert.Equal(1270, result);
        }

        [Fact]
        public void Day04Part1()
        {
            List<ShiftEntry> shifts = new List<ShiftEntry>();
            ReadFile(TestInputFileFromDay(4), line => 
            {
                shifts.Add(Day04.EntryFromInputLine(line));
            });
            Day04.OrganizeShifts(shifts);
            var result = Day04.Part1(shifts);
            Assert.Equal(71748, result);
        }

        [Fact]
        public void Day04Part2()
        {
            List<ShiftEntry> shifts = new List<ShiftEntry>();
            ReadFile(TestInputFileFromDay(4), line => 
            {
                shifts.Add(Day04.EntryFromInputLine(line));
            });
            Day04.OrganizeShifts(shifts);
            var result = Day04.Part2(shifts);
            Assert.Equal(106850, result);
        }

        [Fact]
        public void Day05Part1()
        {
            string input = "";
            ReadFile(TestInputFileFromDay(5), line => 
            {
                input = line;
            });
            var result = Day05.Part1(input);
            Assert.Equal(10766, result);
        }

        [Fact]
        public void Day05Part2()
        {
            string input = "";
            ReadFile(TestInputFileFromDay(5), line => 
            {
                input = line;
            });
            var result = Day05.Part2(input);
            Assert.Equal(6538, result);
        }

        [Fact]
        public void Day06Part1()
        {
            var coords = new List<Coordinate>();
            ReadFile(TestInputFileFromDay(6), line => 
            {
                var split = line.Split(new[] {',', ' '});
                coords.Add(new Coordinate(int.Parse(split[0]), int.Parse(split[2])));
            });
            var result = Day06.Part1(coords);
            Assert.Equal(3293, result);
        }

        [Fact]
        public void Day06Part2()
        {
            var coords = new List<Coordinate>();
            ReadFile(TestInputFileFromDay(6), line => 
            {
                var split = line.Split(new[] {',', ' '});
                coords.Add(new Coordinate(int.Parse(split[0]), int.Parse(split[2])));
            });
            var result = Day06.Part2(coords);
            Assert.Equal(45176, result);
        }

        [Fact]
        public void Day07Part1()
        {
            var steps = new List<StepRequirement>();
            ReadFile(TestInputFileFromDay(7), line => 
            {
                steps.Add(Day07.EntryFromInput(line));
            });
            var result = Day07.Part1(steps);
            Assert.Equal("FMOXCDGJRAUIHKNYZTESWLPBQV", result);
        }

        [Fact]
        public void Day07Part2()
        {
            var steps = new List<StepRequirement>();
            ReadFile(TestInputFileFromDay(7), line => 
            {
                steps.Add(Day07.EntryFromInput(line));
            });
            var result = Day07.Part2(steps);
            Assert.Equal(1053, result);
        }

        [Fact]
        public void Day08Part1()
        {
            string input = "";
            ReadFile(TestInputFileFromDay(8), line => 
            {
                input = line;
            });
            var result = Day08.Part1(new NodeDataProvider(input));
            Assert.Equal(46962, result);
        }

        [Fact]
        public void Day08Part2()
        {
            string input = "";
            ReadFile(TestInputFileFromDay(8), line => 
            {
                input = line;
            });
            var result = Day08.Part2(new NodeDataProvider(input));
            Assert.Equal(22633, result);
        }

        private string TestInputFileFromDay(int day)
        {
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(folder, $"{day:00}_input.txt");
        }

        private void ReadFile(string fileName, Action<string> onLineRead)
        {
            using (TextReader tr = File.OpenText(fileName))
            {
                string line = "";
                
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    onLineRead.Invoke(line);
                }
            }
        }
    }
}
