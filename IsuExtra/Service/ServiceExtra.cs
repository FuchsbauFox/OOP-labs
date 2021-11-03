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

            stream.AddStudent(student.NameStudent, student.IdStudent);
        }

        public void RemoveStudentFromStream(Student student, Stream stream)
        {
            foreach (Student groupStudent in stream.StudentsOfGroup)
            {
                if (student.IdStudent == groupStudent.IdStudent &&
                    student.NameStudent == groupStudent.NameStudent)
                {
                    student = groupStudent;
                }
            }

            stream.RemoveStudent(student);
        }

        public List<Stream> FindStreams(CourseOgnp course)
        {
            return course.StreamsOfCourse.ToList();
        }

        public List<Student> FindStudents(Stream stream)
        {
            return stream.StudentsOfGroup.ToList();
        }

        public List<Student> UnsubscribedStudents(GroupWithTimetable @group)
        {
            MegaFaculty megaFaculty = FindMegaFacultyByGroup(group.Group);
            List<Student> groupStudents = megaFaculty.IsuService.FindStudents(group.Group.GroupName);

            var subscribedStudents =
                (from groupStudent in groupStudents
                from faculty in _megaFaculties
                from courseOgnp in faculty.CoursesOfMegaFaculty
                from stream in courseOgnp.StreamsOfCourse
                from student in stream.StudentsOfGroup
                where student.IdStudent == groupStudent.IdStudent && student.NameStudent == groupStudent.NameStudent
                select groupStudent).ToList();

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
            foreach (MegaFaculty megaFaculty in
                _megaFaculties.Where(megaFaculty => megaFaculty.IsuService.FindStudent(student.NameStudent) != null))
            {
                return megaFaculty;
            }

            throw new StudentNotFoundExeption();
        }

        private GroupWithTimetable FindGroupWithTimetable(Student student)
        {
            foreach (GroupWithTimetable @group in
                from megaFaculty in _megaFaculties
                from @group in megaFaculty.GroupsOfMegaFaculty
                from groupStudent in @group.Group.StudentsOfGroup
                where student.IdStudent == groupStudent.IdStudent &&
                      student.NameStudent == groupStudent.NameStudent
                select @group)
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
            if ((from pairGroup in timetable1.PairsOfTimetable
                from pairStream in timetable2.PairsOfTimetable
                where pairStream.Time == pairGroup.Time && pairStream.Day == pairGroup.Day
                select pairGroup).Any())
            {
                throw new StusentCannotAddInThisStreamException();
            }
        }

        private void CheckStreamTimetable(Student student, Stream stream)
        {
            foreach (Stream courseStream in
                from megaFaculty in _megaFaculties
                from course in megaFaculty.CoursesOfMegaFaculty
                from courseStream in course.StreamsOfCourse
                from streamStudent in courseStream.StudentsOfGroup
                where student.IdStudent == streamStudent.IdStudent &&
                student.NameStudent == streamStudent.NameStudent &&
                stream.Name != courseStream.Name
                select courseStream)
            {
                CheckTimetable(stream.Timetable, courseStream.Timetable);
            }
        }

        private MegaFaculty FindMegaFacultyByGroup(Group @group)
        {
            foreach (MegaFaculty megaFaculty in
                _megaFaculties.Where(megaFaculty => megaFaculty.IsuService.FindGroup(@group.GroupName) != null))
            {
                return megaFaculty;
            }

            throw new GroupNotFoundException();
        }
    }
}