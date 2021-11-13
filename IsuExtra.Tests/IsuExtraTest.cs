using System;
using System.Collections.Generic;
using Isu.Services;
using IsuExtra.Service;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class Tests
    {
        private ServiceExtra _serviceExtra;
        private MegaFaculty _megaFaculty;
        private MegaFaculty _megaFaculty1;
        private MegaFaculty _megaFaculty2;

        [SetUp]
        public void Setup()
        {
            _serviceExtra = new ServiceExtra();
            _megaFaculty = _serviceExtra.AddMegaFaculty("M");
            _megaFaculty1 = _serviceExtra.AddMegaFaculty("N");
            _megaFaculty2 = _serviceExtra.AddMegaFaculty("R");
        }

        [Test]
        public void AddStudentToStream_StudentCannotAddToStream()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                GroupWithTimetable group1 = _megaFaculty.AddGroup("M3204");
                Student student1 = _megaFaculty.AddStudent(group1, "Васин Владимир");
                _megaFaculty.AddPairInGroup(group1, "ООП", new DateTime(2021, 11, 14, 11, 30, 0));
                
                CourseOgnp courseOgnp = _megaFaculty.AddCourseOgnp("Кибербезопасность");
                Stream stream1 = _megaFaculty.AddStreamInCourse(courseOgnp, "КИБ1.1");
                _megaFaculty.AddPairInStream(stream1, "Кибербезопасть практика", new DateTime(2021, 11, 14, 11, 30, 0));
                
                _serviceExtra.AddStudentsInStream(student1, stream1);
            });
        }
        
        [Test]
        public void AddStudentToStream_TimeCorrect_RemoveStudentFromStream()
        {
            GroupWithTimetable group1 = _megaFaculty.AddGroup("M3204");
            Student student1 = _megaFaculty.AddStudent(group1, "Васин Владимир");
            _megaFaculty.AddPairInGroup(group1, "ООП", new DateTime(2021, 11, 15, 11, 30, 0));
            
            CourseOgnp courseOgnp1 = _megaFaculty1.AddCourseOgnp("Кибербезопасность");
            Stream stream1 = _megaFaculty1.AddStreamInCourse(courseOgnp1, "КИБ1.1");
            _megaFaculty1.AddPairInStream(stream1, "Кибербезопасть практика", new DateTime(2021, 11, 14, 11, 30, 0));
            
            _serviceExtra.AddStudentsInStream(student1, stream1);
            var students = new List<Student>() {student1};
            List<Student> findStudents = _serviceExtra.FindStudents(stream1);
            
            Assert.AreEqual(students.Count, findStudents.Count);
            for (int i = 0; i < students.Count; i++)
            {
                Assert.AreEqual(students[i].IdStudent, findStudents[i].IdStudent);
                Assert.AreEqual(students[i].NameStudent, findStudents[i].NameStudent);
            }
        }

        [Test]
        public void AddStudentToStream_TimeIncorrect()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                GroupWithTimetable group1 = _megaFaculty.AddGroup("M3204");
                Student student1 = _megaFaculty.AddStudent(group1, "Васин Владимир");
                _megaFaculty.AddPairInGroup(group1, "ООП", new DateTime(2021, 11, 14, 11, 30, 0));
                
                CourseOgnp courseOgnp1 = _megaFaculty1.AddCourseOgnp("Кибербезопасность");
                Stream stream1 = _megaFaculty1.AddStreamInCourse(courseOgnp1, "КИБ1.1");
                _megaFaculty1.AddPairInStream(stream1, "Кибербезопасть практика", new DateTime(2021, 11, 14, 11, 30, 0));
                
                _serviceExtra.AddStudentsInStream(student1, stream1);
            });
        }

        [Test]
        public void AddStudentToDifferentStreams_TimeCorrect()
        {
            GroupWithTimetable group1 = _megaFaculty.AddGroup("M3204");
            Student student1 = _megaFaculty.AddStudent(group1, "Васин Владимир");
            _megaFaculty.AddPairInGroup(group1, "ООП", new DateTime(2021, 11, 15, 11, 30, 0));
            
            CourseOgnp courseOgnp1 = _megaFaculty1.AddCourseOgnp("Кибербезопасность");
            Stream stream1 = _megaFaculty1.AddStreamInCourse(courseOgnp1, "КИБ1.1");
            _megaFaculty1.AddPairInStream(stream1, "Кибербезопасть практика", new DateTime(2021, 11, 14, 11, 30, 0));
            
            CourseOgnp courseOgnp2 = _megaFaculty2.AddCourseOgnp("Биоинформатика");
            Stream stream2 = _megaFaculty2.AddStreamInCourse(courseOgnp2, "БИН1.1");
            _megaFaculty2.AddPairInStream(stream2, "Биоинформатика практика", new DateTime(2021, 11, 14, 15, 20, 0));
            
            _serviceExtra.AddStudentsInStream(student1, stream1);
            _serviceExtra.AddStudentsInStream(student1, stream2);
            
            var students = new List<Student>() {student1};
            List<Student> findStudents1 = _serviceExtra.FindStudents(stream1);
            List<Student> findStudents2 = _serviceExtra.FindStudents(stream1);
            
            Assert.AreEqual(students.Count, findStudents1.Count);
            Assert.AreEqual(students.Count, findStudents2.Count);
            for (int i = 0; i < students.Count; i++)
            {
                Assert.AreEqual(students[i].IdStudent, findStudents1[i].IdStudent);
                Assert.AreEqual(students[i].NameStudent, findStudents1[i].NameStudent);
                Assert.AreEqual(students[i].IdStudent, findStudents2[i].IdStudent);
                Assert.AreEqual(students[i].NameStudent, findStudents2[i].NameStudent);
            }
        }

        [Test]
        public void AddStudentToDifferentStreams_TimeIncorrect()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                GroupWithTimetable group1 = _megaFaculty.AddGroup("M3204");
                Student student1 = _megaFaculty.AddStudent(group1, "Васин Владимир");
                _megaFaculty.AddPairInGroup(group1, "ООП", new DateTime(2021, 11, 15, 11, 30, 0));
            
                CourseOgnp courseOgnp1 = _megaFaculty1.AddCourseOgnp("Кибербезопасность");
                Stream stream1 = _megaFaculty1.AddStreamInCourse(courseOgnp1, "КИБ1.1");
                _megaFaculty1.AddPairInStream(stream1, "Кибербезопасть практика", new DateTime(2021, 11, 14, 11, 30, 0));
            
                CourseOgnp courseOgnp2 = _megaFaculty2.AddCourseOgnp("Биоинформатика");
                Stream stream2 = _megaFaculty2.AddStreamInCourse(courseOgnp2, "БИН1.1");
                _megaFaculty2.AddPairInStream(stream2, "Биоинформатика практика", new DateTime(2021, 11, 14, 11, 30, 0));
            
                _serviceExtra.AddStudentsInStream(student1, stream1);
                _serviceExtra.AddStudentsInStream(student1, stream2);
            });
        }

        [Test]
        public void CheckListUnsubscribedStudents()
        {
            GroupWithTimetable group1 = _megaFaculty.AddGroup("M3204");
            var studentsList = new List<string>()
            {
                "Александров Даниил",
                "Антоненко Екатерина",
                "Арсентьев Даниил",
                "Базалий Иван",
                "Беззубцева Анастасия",
                "Беспалов Денис",
                "Васильев Артём",
                "Васин Владимир"
            };
            var students = new List<Student>();
            foreach (string student in studentsList)
            {
                students.Add(_megaFaculty.AddStudent(group1, student));
            }
            _megaFaculty.AddPairInGroup(group1, "ООП", new DateTime(2021, 11, 14, 11, 30, 0));
            
            CourseOgnp courseOgnp1 = _megaFaculty1.AddCourseOgnp("Кибербезопасность");
            Stream stream1 = _megaFaculty1.AddStreamInCourse(courseOgnp1, "КИБ1.1");
            _megaFaculty1.AddPairInStream(stream1, "Кибербезопасть практика", new DateTime(2021, 11, 15, 11, 30, 0));

            for (int i = 0; i < 5; i++)
            {
                _serviceExtra.AddStudentsInStream(students[i], stream1);
            }

            List<Student> listStudentsWhoHaveNotChosenCourse = _serviceExtra.UnsubscribedStudents(group1);
            Assert.AreEqual(listStudentsWhoHaveNotChosenCourse.Count, 3);
            for (int i = 5; i < 8; i++)
            {
                Assert.AreEqual(students[i].IdStudent, listStudentsWhoHaveNotChosenCourse[i-5].IdStudent);
                Assert.AreEqual(students[i].NameStudent, listStudentsWhoHaveNotChosenCourse[i-5].NameStudent);
            }
        }
    }
}