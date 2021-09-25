using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Services
{
    public class Student
    {
        public Student(string name, int id, Group @group)
        {
            CheckStudentName(name);

            IdStudent = id;
            NameStudent = name;
            StudentGroup = @group;
        }

        public int IdStudent { get; }
        public string NameStudent { get;  }
        public Group StudentGroup { get; private set; }

        public void CheckAndChangeStudentGroup(Group @group)
        {
            if (StudentInGroup(StudentGroup)) return;
            if (StudentInGroup(@group))
            {
                StudentGroup = @group;
            }
            else
            {
                throw new StudentNotInThisGroupExeption();
            }
        }

        private static void CheckStudentName(string name)
        {
            if (!Regex.IsMatch(name, @"^(?=.{1,41}$)[А-ЯЁ]\d{0}[а-яё]+(?:[-' ][А-ЯЁ]\d{0}[а-яё]+)$"))
            {
                throw new InvalidStudentNameExeption();
            }
        }

        private bool StudentInGroup(Group @group)
        {
            return @group.StudentsOfGroup.ToList()
                .Any(studentOnGroup => studentOnGroup == this);
        }
    }
}