using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using IsuExtra.Tools;

namespace IsuExtra.Service
{
    public class MegaFaculty
    {
        private readonly List<GroupWithTimetable> _groups;
        private readonly List<CourseOgnp> _courses;

        public MegaFaculty(string name)
        {
            IsuService = new IsuService();
            _courses = new List<CourseOgnp>();
            _groups = new List<GroupWithTimetable>();

            CheckName(name);
            Name = name;
        }

        public string Name { get; }
        public IReadOnlyList<CourseOgnp> CoursesOfMegaFaculty => _courses;
        public IReadOnlyList<GroupWithTimetable> GroupsOfMegaFaculty => _groups;
        internal IsuService IsuService { get; }

        public GroupWithTimetable AddGroup(string groupName)
        {
            _groups.Add(new GroupWithTimetable(IsuService.AddGroup(groupName)));
            return _groups.Last();
        }

        public Student AddStudent(GroupWithTimetable @group, string name)
        {
            CheckGroupOnExist(group);

            return IsuService.AddStudent(group.Group, name);
        }

        public void AddPairInGroup(GroupWithTimetable @group, string name, string day, string time)
        {
            CheckGroupOnExist(group);

            group.AddPairInTimetable(name, day, time);
        }

        public CourseOgnp AddCourseOgnp(string name)
        {
            var course = new CourseOgnp(name);
            CheckCourseOnExist(course);

            _courses.Add(course);
            return course;
        }

        public Stream AddStreamInCourse(CourseOgnp course, string nameStream)
        {
            CheckCourseOnExist(course, true);

            return course.AddStream(nameStream);
        }

        public void AddPairInStream(Stream stream, string name, string day, string time)
        {
            CheckStream(stream);

            stream.AddPairInTimetable(name, day, time);
        }

        private void CheckName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
        }

        private void CheckGroupOnExist(GroupWithTimetable newGroup)
        {
            bool groupExist = _groups.Any(@group => group == newGroup);
            if (!groupExist)
            {
                throw new GroupNotExistException();
            }
        }

        private void CheckCourseOnExist(CourseOgnp verifiableCourse, bool shouldExist = false)
        {
            bool courseExist = _courses.Any(course => course.Name == verifiableCourse.Name);
            switch (shouldExist)
            {
                case false when courseExist:
                    throw new CourseAlreadyExistException();
                case true when !courseExist:
                    throw new CourseNotExitException();
            }
        }

        private void CheckStream(Stream verifiableStream)
        {
            if (_courses.SelectMany(course => course.StreamsOfCourse).Any(stream => stream == verifiableStream))
            {
                return;
            }

            throw new StreamNotExistException();
        }
    }
}