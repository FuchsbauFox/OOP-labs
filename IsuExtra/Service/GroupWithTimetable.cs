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

        public Group Group { get; }
        public Timetable Timetable { get; }

        internal void AddPairInTimetable(string name, string day, string time)
        {
            Timetable.AddPair(name, day, time);
        }
    }
}