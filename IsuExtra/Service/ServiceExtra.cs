using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Isu.Services;
using Isu.Tools;
using IsuExtra.Tools;

namespace IsuExtra.Service
{
    public class ServiceExtra
    {
        private readonly List<MegaFaculty> _megaFaculties;

        public ServiceExtra()
        {
            _megaFaculties = new List<MegaFaculty>();
        }

        public MegaFaculty AddMegaFaculty(string name)
        {
            var megaFaculty = new MegaFaculty(name);
            CheckMegaFacultyOnExist(megaFaculty);

            _megaFaculties.Add(megaFaculty);
            return megaFaculty;
        }

        public void AddStudentsInStream(Student student, Stream stream)
        {
            MegaFaculty megaFaculty = FindMegaFacultyByStudent(student);
            GroupWithTimetable @group = FindGroupWithTimetable(student);
            CheckCourse(megaFaculty, stream);
            CheckTimetable(group.Timetable, stream.Timetable);
            CheckStreamTimetable(student, stream);

            stream.Group.AddStudent(student.NameStudent, student.IdStudent);
        }

        public void RemoveStudentFromStream(Student student, Stream stream)
        {
            Student findStudent = stream.Group.StudentsOfGroup.FirstOrDefault(groupStudent =>
                student.IdStudent == groupStudent.IdStudent &&
                student.NameStudent == groupStudent.NameStudent);

            stream.Group.RemoveStudent(findStudent);
        }

        public List<Stream> FindStreams(CourseOgnp course)
        {
            return course.StreamsOfCourse.ToList();
        }

        public List<Student> FindStudents(Stream stream)
        {
            return stream.Group.StudentsOfGroup.ToList();
        }

        public List<Student> UnsubscribedStudents(GroupWithTimetable @group)
        {
            MegaFaculty megaFaculty = FindMegaFacultyByGroup(group.Group);
            List<Student> groupStudents = megaFaculty.IsuService.FindStudents(group.Group.GroupName);

            var subscribedStudents =
                _megaFaculties.SelectMany(faculty => faculty.CoursesOfMegaFaculty)
                    .SelectMany(course => course.StreamsOfCourse)
                    .SelectMany(stream => stream.Group.StudentsOfGroup)
                    .SelectMany(student => groupStudents, (student, groupStudent) => new { student, groupStudent })
                    .Where(students =>
                        students.student.IdStudent == students.groupStudent.IdStudent &&
                        students.student.NameStudent == students.groupStudent.NameStudent)
                    .Select(@t => @t.groupStudent).ToList();

            foreach (Student student in subscribedStudents)
            {
                groupStudents.Remove(student);
            }

            return groupStudents;
        }

        private void CheckMegaFacultyOnExist(MegaFaculty newMegaFaculty)
        {
            if (_megaFaculties.Any(megaFaculty => megaFaculty.Name == newMegaFaculty.Name))
            {
                throw new StreamAlreadyExistExceptiom();
            }
        }

        private MegaFaculty FindMegaFacultyByStudent(Student student)
        {
            MegaFaculty megaFaculty = _megaFaculties.FirstOrDefault(megaFaculty =>
                megaFaculty.IsuService.FindStudent(student.NameStudent) != null);

            if (megaFaculty == null)
            {
                throw new GroupNotFoundException();
            }

            return megaFaculty;
        }

        private GroupWithTimetable FindGroupWithTimetable(Student student)
        {
            foreach (GroupWithTimetable @group in _megaFaculties
                .SelectMany(megaFaculty => megaFaculty.GroupsOfMegaFaculty)
                .SelectMany(@group => @group.Group.StudentsOfGroup, (@group, studentInGroup) => new { @group, studentInGroup })
                .Where(groupAndStudent =>
                    student.IdStudent == groupAndStudent.studentInGroup.IdStudent &&
                    student.NameStudent == groupAndStudent.studentInGroup.NameStudent)
                .Select(groupAndStudent => groupAndStudent.@group))
            {
                return @group;
            }

            throw new GroupNotFoundException();
        }

        private void CheckCourse(MegaFaculty megaFaculty, Stream verifiableStream)
        {
            if (megaFaculty.CoursesOfMegaFaculty
                .SelectMany(course => course.StreamsOfCourse)
                .Any(stream => stream == verifiableStream))
            {
                throw new StusentCannotAddInThisStreamException();
            }
        }

        private void CheckTimetable(Timetable timetable1, Timetable timetable2)
        {
            if (timetable1.PairsOfTimetable
                .SelectMany(pair1 => timetable2.PairsOfTimetable, (pair1, pair2) => new { pair1, pair2 })
                .Where(pairs => pairs.pair1.Time == pairs.pair2.Time)
                .Select(pairs => pairs.pair1).Any())
            {
                throw new StusentCannotAddInThisStreamException();
            }
        }

        private void CheckStreamTimetable(Student student, Stream stream)
        {
            foreach (Stream streamOfCourse in _megaFaculties
                .SelectMany(megaFaculty => megaFaculty.CoursesOfMegaFaculty)
                .SelectMany(course => course.StreamsOfCourse)
                .SelectMany(streamOfCourse => streamOfCourse.Group.StudentsOfGroup, (streamOfCourse, studentOfStream) => new { streamOfCourse, studentOfStream })
                .Where(streamAndStudent =>
                    student.IdStudent == streamAndStudent.studentOfStream.IdStudent &&
                    student.NameStudent == streamAndStudent.studentOfStream.NameStudent &&
                    stream.Name != streamAndStudent.streamOfCourse.Name)
                .Select(@t => @t.streamOfCourse))
            {
                CheckTimetable(stream.Timetable, streamOfCourse.Timetable);
            }
        }

        private MegaFaculty FindMegaFacultyByGroup(Group @group)
        {
            MegaFaculty megaFaculty = _megaFaculties.FirstOrDefault(megaFaculty =>
                megaFaculty.IsuService.FindGroup(@group.GroupName) != null);

            if (megaFaculty == null)
            {
                throw new GroupNotFoundException();
            }

            return megaFaculty;
        }
    }
}