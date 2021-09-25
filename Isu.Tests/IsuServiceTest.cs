using System;
using System.Collections.Generic;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IsuService _isuService;

        [SetUp]
        public void Setup()
        {
            //TODO: implement
            _isuService = null;
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            _isuService = new IsuService();
            Group group1 = _isuService.AddGroup("M3104");
            Assert.AreEqual(group1, _isuService.FindGroup("M3104"));
            Student student1 = _isuService.AddStudent(group1, "Олег Смирнов");
            Assert.AreEqual(student1, _isuService.GetStudent(0));
            Assert.AreEqual(student1, _isuService.FindStudent("Олег Смирнов"));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService = new IsuService();
                Group group1 = _isuService.AddGroup("M3204");
                var studentsList = new List<string>()
                {
                    "Александров Даниил",
                    "Антоненко Екатерина",
                    "Арсентьев Даниил",
                    "Базалий Иван",
                    "Беззубцева Анастасия",
                    "Беспалов Денис",
                    "Васильев Артём",
                    "Васин Владимир",
                    "Гурман Тимофей",
                    "Ершов Александр",
                    "Заборов Даниэль",
                    "Захарова Виктория",
                    "Иванов Алексей",
                    "Иванов Сергей",
                    "Казанцев Данил",
                    "Климачёва Екатерина",
                    "Красильников Михаил",
                    "Митрофанова Анастасия",
                    "Решетникова Анна",
                    "Суслов Михаил",
                    "Титов Даниил",
                    "Шатинский Григорий"
                };
                foreach (string student in studentsList)
                {
                    _isuService.AddStudent(group1, student);
                }
                _isuService.AddStudent(group1, "Иван Иванович");
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService = new IsuService();
                Group invalidGroup = _isuService.AddGroup("M3004");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            _isuService = new IsuService();
            Group group1 = _isuService.AddGroup("M3104");
            Group group2 = _isuService.AddGroup("M3204");
            Student student1 = _isuService.AddStudent(group1, "Иван Петров");
            Student student2 = _isuService.AddStudent(group1, "Анна Петрова");
            var newListGroup1 = new List<Student>() {student2};
            var newListGroup2 = new List<Student>() {student1};
            _isuService.ChangeStudentGroup(student1, group2);
            Assert.AreEqual(newListGroup1, _isuService.FindStudents("M3104"));
            Assert.AreEqual(newListGroup2, _isuService.FindStudents("M3204"));
        }
    }
}