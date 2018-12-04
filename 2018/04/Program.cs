using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public enum ShiftEvent
    {
        ShiftBegins,
        FallsAsleep,
        WakesUp
    }

    public class ShiftEntry
    {
        public int Id;
        public ShiftEvent Event;
        public DateTime Date;

        public ShiftEntry(int id, ShiftEvent shiftEvent, DateTime date)
        {
            Id = id;
            Event = shiftEvent;
            Date = date;
        }
    }

    public class Day04
    {
        static void Main(string[] args)
        {
            var shifts = new List<ShiftEntry>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                string line = "";
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    shifts.Add(EntryFromInputLine(line));
                }
            }
            
            OrganizeShifts(shifts);

            int part1 = Part1(shifts);
            int part2 = Part2(shifts);

            Console.WriteLine($"Part One: Guard Id * [Most Frequent Sleep Minute] = {part1}");
            Console.WriteLine($"Part Two: Guard Id * [Most Frequent Sleep Minute Across All Guards] = {part2}");
        }

        public static ShiftEntry EntryFromInputLine(string line)
        {
            Regex pattern = new Regex(@"\[([0-9]+)-([0-9]+)-([0-9]+) ([0-9]+):([0-9]+)\] (.+)");
            MatchCollection matches = pattern.Matches(line);
            foreach (Match thisMatch in matches)
            {
                int year = int.Parse(thisMatch.Groups[1].Value);
                int month = int.Parse(thisMatch.Groups[2].Value);
                int day = int.Parse(thisMatch.Groups[3].Value);
                int hour = int.Parse(thisMatch.Groups[4].Value);
                int minute = int.Parse(thisMatch.Groups[5].Value);
                string eventText = thisMatch.Groups[6].Value;

                ShiftEntry newEntry;
                DateTime entryDate = new DateTime(year, month, day, hour, minute, 0);
                if (eventText == "wakes up")
                {
                    newEntry = new ShiftEntry(-1, ShiftEvent.WakesUp, entryDate);
                }
                else if (eventText == "falls asleep")
                {
                    newEntry = new ShiftEntry(-1, ShiftEvent.FallsAsleep, entryDate);
                }
                else
                {
                    // Guard #10 begins shift
                    int id = int.Parse(eventText.Split(new[] {'#', ' '})[2]);
                    newEntry = new ShiftEntry(id, ShiftEvent.ShiftBegins, entryDate);
                }

                return newEntry;
            }

            throw new InvalidDataException("Invalid input line doesn't match regex format");
        }

        public static void OrganizeShifts(List<ShiftEntry> shifts)
        {
            shifts.Sort((shift1, shift2) => shift1.Date.CompareTo(shift2.Date));
            int currentId = 0;
            foreach (var entry in shifts)
            {
                if (entry.Event == ShiftEvent.ShiftBegins)
                {
                    currentId = entry.Id;
                }
                else
                {
                    entry.Id = currentId;
                }
            }
        }

        public static Dictionary<int, int[]> GetGuardSleepTimes(IList<ShiftEntry> shifts)
        {
            // For each guard, for each minute, how many days were they sleeping on that minute
            var guardSleepTimes = new Dictionary<int, int[]>();

            int sleepStartedTime = 0;
            foreach (var entry in shifts)
            {
                if (entry.Event == ShiftEvent.ShiftBegins)
                {
                    Debug.Assert(sleepStartedTime == 0, "Guard was still sleeping at the end of the previous shift, violating assumptions.");
                    //awakeStartedTime = 0;
                }
                else if (entry.Event == ShiftEvent.FallsAsleep)
                {
                    sleepStartedTime = entry.Date.Minute;
                }
                else if (entry.Event == ShiftEvent.WakesUp)
                {
                    int napLength = entry.Date.Minute - sleepStartedTime;
                    if (!guardSleepTimes.ContainsKey(entry.Id))
                    {
                        guardSleepTimes.Add(entry.Id, new int[60]);
                    }

                    for (int i = sleepStartedTime; i < entry.Date.Minute; i++)
                    {
                        guardSleepTimes[entry.Id][i]++;
                    }

                    sleepStartedTime = 0;
                }
            }

            return guardSleepTimes;
        }

        public static int Part1(IList<ShiftEntry> shifts)
        {
            var guardSleepTimes = GetGuardSleepTimes(shifts);
            
            int maxMinutesAsleep = 0;
            int guardSleepingMost = 0;
            int overallMostCommonMinute = 0;
            foreach (var guard in guardSleepTimes.Keys)
            {
                int minutesAsleep = 0;
                int mostCommonMinute = 0;
                int mostCommonMinuteValue = 0;
                for (int i = 0; i < 60; i++)
                {
                    minutesAsleep += guardSleepTimes[guard][i];
                    
                    if (guardSleepTimes[guard][i] > mostCommonMinuteValue)
                    {
                        mostCommonMinuteValue = guardSleepTimes[guard][i];
                        mostCommonMinute = i;
                    }
                }

                if (minutesAsleep > maxMinutesAsleep)
                {
                    maxMinutesAsleep = minutesAsleep;
                    guardSleepingMost = guard;
                    overallMostCommonMinute = mostCommonMinute;
                }
            }

            Console.WriteLine($"Guard {guardSleepingMost} slept the most at {maxMinutesAsleep} minutes. Most common minute is {overallMostCommonMinute}");
            return guardSleepingMost * overallMostCommonMinute;
        }

        public static int Part2(IList<ShiftEntry> shifts)
        {
            var guardSleepTimes = GetGuardSleepTimes(shifts);
            
            int maxMinutesAsleepValue = 0;
            int maxMinutesAsleep = 0;
            int guardSleepingMost = 0;
            foreach (var guard in guardSleepTimes.Keys)
            {
                int mostCommonMinute = 0;
                int mostCommonMinuteValue = 0;
                for (int i = 0; i < 60; i++)
                {
                    if (guardSleepTimes[guard][i] > mostCommonMinuteValue)
                    {
                        mostCommonMinuteValue = guardSleepTimes[guard][i];
                        mostCommonMinute = i;
                    }
                }

                if (mostCommonMinuteValue > maxMinutesAsleepValue)
                {
                    maxMinutesAsleep = mostCommonMinute;
                    maxMinutesAsleepValue = mostCommonMinuteValue;
                    guardSleepingMost = guard;
                }
            }

            Console.WriteLine($"Guard {guardSleepingMost} slept the most on minute {maxMinutesAsleep} with {maxMinutesAsleepValue} minutes");
            return guardSleepingMost * maxMinutesAsleep;
        }
    }
}
