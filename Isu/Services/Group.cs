using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        private readonly List<Student> _students;

        public Group(string name)
        {
            CheckGroupName(name);

            GroupName = name;
            _students = new List<Student>();
            StudentsOfGroup = _students.AsReadOnly();
        }

        protected Group()
        {
            GroupName = null;
            _students = new List<Student>();
            StudentsOfGroup = _students.AsReadOnly();
        }

        public string GroupName { get; }
        public IList<Student> StudentsOfGroup { get; }

        public Student AddStudent(string name, int id)
        {
            return AddStudent(new Student(name, id, this));
        }

        public Student AddStudent(Student student)
        {
            CheckOnMaxStudentsInGroup();
            _students.Add(student);
            student.CheckAndChangeStudentGroup(this);
            return _students.Last();
        }

        public void RemoveStudent(Student student)
        {
            _students.Remove(student);
        }

        private static void CheckGroupName(string name)
        {
            if (!Regex.IsMatch(name, @"^[A-Z][1-9]\d{0}[1-4]\d{0}[0-9]\d{1}$"))
            {
                throw new InvalidGroupNameExeption();
            }
        }

        private void CheckOnMaxStudentsInGroup()
        {
            if (_students.Count == 22)
            {
                throw new MaxStudentInGroupExeption();
            }
        }
    }
}