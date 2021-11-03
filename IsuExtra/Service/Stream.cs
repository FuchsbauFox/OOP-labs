using System.Text.RegularExpressions;
using Isu.Tools;
using Group = Isu.Services.Group;

namespace IsuExtra.Service
{
    public class Stream : Group
    {
        public Stream(string name)
        {
            CheckStreamName(name);

            Name = name;
            Timetable = new Timetable();
        }

        public string Name { get; }
        public Timetable Timetable { get; }

        public void AddPairInTimetable(string name, string day, string time)
        {
            Timetable.AddPair(name, day, time);
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