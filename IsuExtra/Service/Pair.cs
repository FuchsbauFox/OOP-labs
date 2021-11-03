using System;
using System.Text.RegularExpressions;

namespace IsuExtra.Service
{
    public class Pair
    {
        public Pair(string name, string day, string time)
        {
            CheckCoupleName(name);
            CheckDay(day);
            CheckTime(time);

            Name = name;
            Day = day;
            Time = time;
        }

        public string Name { get; }
        public string Day { get; }
        public string Time { get; }

        private static void CheckCoupleName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
        }

        private static void CheckDay(string day)
        {
            if (day == null)
            {
                throw new ArgumentNullException();
            }
        }

        private static void CheckTime(string time)
        {
            if (!Regex.IsMatch(time, @"[0-9]\d{1}:[0-9]\d{1}$"))
            {
                throw new ArgumentException();
            }
        }
    }
}