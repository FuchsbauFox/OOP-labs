using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        public Group(string name)
        {
            CheckGroupName(name);

            GroupName = name;
            Students = new List<Student>();
        }

        public List<Student> Students { get; }
        public string GroupName { get; }

        public Student AddStudent(string name, int id)
        {
            return AddStudent(new Student(name, id));
        }

        public Student AddStudent(Student student)
        {
            CheckOnMaxStudentsInGroup();
            Students.Add(student);
            return Students.Last();
        }

        private static void CheckGroupName(string name)
        {
            if (!Regex.IsMatch(name, @"^M+3+[1-4]\d{0}[0-9]\d{1}$"))
            {
                throw new InvalidGroupNameExeption();
            }
        }

        private void CheckOnMaxStudentsInGroup()
        {
            if (Students.Count == 22)
            {
                throw new MaxStudentInGroupExeption();
            }
        }
    }
}