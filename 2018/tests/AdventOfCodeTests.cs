using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
