using System;
using System.Text.RegularExpressions;
using Isu.Tools;
using Group = Isu.Services.Group;

namespace IsuExtra.Service
{
    public class Stream
    {
        public Stream(string name)
        {
            CheckStreamName(name);

            Name = name;
            Group = new Group();
            Timetable = new Timetable();
        }

        public string Name { get; }
        public Timetable Timetable { get; }
        internal Group Group { get; }

        internal void AddPairInTimetable(string name, DateTime time)
        {
            Timetable.AddPair(name, time);
        }

        private static void CheckStreamName(string name)
        {
            if (!Regex.IsMatch(name, @"^[A-ZА-ЯЁ]+[1-9]\d{0}\.[1-9]\d{0}$"))
            {
                throw new InvalidGroupNameExeption();
            }
        }
    }
}