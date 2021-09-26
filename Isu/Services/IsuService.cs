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
            _courses = new List<CourseNumber>();
        }

        public Group AddGroup(string name)
        {
            foreach (CourseNumber course in _courses.Where(course => course.Course == name[2] - '0'))
            {
                return course.AddGroup(name);
            }

            _courses.Add(new CourseNumber(name));
            return _courses.Last().AddGroup(name);
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

            foreach (Student student in
                from course in _courses
                from @group in course.GroupsOfCourse.ToList()
                from student in @group.StudentsOfGroup.ToList()
                where student.IdStudent == id
                select student)
            {
                return student;
            }

            throw new StudentNotFoundExeption();
        }

        public Student FindStudent(string name)
        {
            return (from course in _courses
                from @group in course.GroupsOfCourse.ToList()
                from student in @group.StudentsOfGroup.ToList()
                select student)
                .FirstOrDefault(student => student.NameStudent == name);
        }

        public List<Student> FindStudents(string groupName)
        {
            return (from course in _courses
                from @group in course.GroupsOfCourse.ToList()
                where @group.GroupName == groupName
                from student in @group.StudentsOfGroup.ToList()
                select student).ToList();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return (from @group in courseNumber.GroupsOfCourse.ToList()
                from student in @group.StudentsOfGroup.ToList()
                select student).ToList();
        }

        public Group FindGroup(string groupName)
        {
            return (from course in _courses
                    from @group in course.GroupsOfCourse.ToList()
                    select @group)
                .FirstOrDefault(@group => @group.GroupName == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return courseNumber.GroupsOfCourse.ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            student.StudentGroup.RemoveStudent(student);
            newGroup.AddStudent(student);
        }
    }
}