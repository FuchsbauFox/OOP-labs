using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService
    {
        private readonly List<CourseNumber> _courses;
        private int _idLastStudent;

        public IsuService()
        {
            _idLastStudent = 0;
            _courses = new List<CourseNumber>
                {
                    new CourseNumber(1),
                    new CourseNumber(2),
                    new CourseNumber(3),
                    new CourseNumber(4),
                };
        }

        public Group AddGroup(string name)
        {
            if (!int.TryParse(name[2].ToString(), out int courseNumber)
                || courseNumber < 1
                || courseNumber > 4)
            {
                throw new InvalidGroupNameExeption();
            }

            return _courses[courseNumber - 1].AddGroup(name);
        }

        public Student AddStudent(Group group, string name)
        {
            return group.AddStudent(name, _idLastStudent++);
        }

        public Student GetStudent(int id)
        {
            if (id >= _idLastStudent)
            {
                throw new InvalidIdStudentExeption();
            }

            return (from course in _courses
                    from @group in course.Groups
                    from student in @group.Students
                    select student)
                .FirstOrDefault(student => student.IdStudent == id);
        }

        public Student FindStudent(string name)
        {
            return (from course in _courses
                from @group in course.Groups
                from student in @group.Students
                select student)
                .FirstOrDefault(student => student.NameStudent == name);
        }

        public List<Student> FindStudents(string groupName)
        {
            return (from course in _courses
                from @group in course.Groups
                where @group.GroupName == groupName
                from student in @group.Students
                select student).ToList();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return (from @group in courseNumber.Groups
                from student in @group.Students
                select student).ToList();
        }

        public Group FindGroup(string groupName)
        {
            return (from course in _courses
                    from @group in course.Groups
                    select @group)
                .FirstOrDefault(@group => @group.GroupName == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return courseNumber.Groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group @group in
                from course in _courses
                from @group in course.Groups
                from findStudent in @group.Students
                where findStudent == student
                select @group)
            {
                newGroup.AddStudent(student);
                @group.Students.Remove(student);
                break;
            }
        }
    }
}