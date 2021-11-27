using System;
using System.Text.RegularExpressions;

namespace IsuExtra.Service
{
    public class Lesson
    {
        public Lesson(string name, DateTime time)
        {
            CheckCoupleName(name);
            CheckTime(time);

            Name = name;
            Time = time;
        }

        public string Name { get; }
        public DateTime Time { get; }

        private static void CheckCoupleName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
        }

        private static void CheckTime(DateTime time)
        {
            if (time == null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}