using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Services
{
    public class Student
    {
        public Student(string name, int id)
        {
            CheckStudentName(name);

            IdStudent = id;
            NameStudent = name;
        }

        public int IdStudent { get; }
        public string NameStudent { get;  }

        private static void CheckStudentName(string name)
        {
            if (!Regex.IsMatch(name, @"^(?=.{1,41}$)[А-ЯЁ]\d{0}[а-яё]+(?:[-' ][А-ЯЁ]\d{0}[а-яё]+)$"))
            {
                throw new InvalidStudentNameExeption();
            }
        }
    }
}