using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class CourseNumber
    {
        private readonly List<Group> _groups;

        public CourseNumber(string groupName)
        {
            Course = CheckCourseNumber(groupName);
            _groups = new List<Group>();
            GroupsOfCourse = _groups.AsReadOnly();
        }

        public int Course { get; }
        public IList<Group> GroupsOfCourse { get; }

        public Group AddGroup(string name)
        {
            CheckGroupOnExist(name);
            _groups.Add(new Group(name));
            return _groups.Last();
        }

        private static int CheckCourseNumber(string groupName)
        {
            if (!int.TryParse(groupName[2].ToString(), out int courseNumber)
                || courseNumber < 1
                || courseNumber > 4)
            {
                throw new InvalidGroupNameExeption();
            }

            return courseNumber;
        }

        private void CheckGroupOnExist(string name)
        {
            if (_groups.Any(@group => @group.GroupName == name))
            {
                throw new GroupAlreadyExistExeption();
            }
        }
    }
}