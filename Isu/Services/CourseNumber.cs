using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class CourseNumber
    {
        public CourseNumber(int numberCourse)
        {
            Course = numberCourse;
            Groups = new List<Group>();
        }

        public List<Group> Groups { get; }
        public int Course { get; }

        public Group AddGroup(string name)
        {
            CheckGroupOnExist(name);
            Groups.Add(new Group(name));
            return Groups.Last();
        }

        private void CheckGroupOnExist(string name)
        {
            if (Groups.Any(@group => @group.GroupName == name))
            {
                throw new GroupAlreadyExistExeption();
            }
        }
    }
}