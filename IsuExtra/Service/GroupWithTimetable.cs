using System;
using Isu.Services;

namespace IsuExtra.Service
{
    public class GroupWithTimetable
    {
        public GroupWithTimetable(Group @group)
        {
            Group = @group;
            Timetable = new Timetable();
        }

        public Timetable Timetable { get; }
        internal Group Group { get; }

        internal void AddPairInTimetable(string name, DateTime time)
        {
            Timetable.AddPair(name, time);
        }
    }
}