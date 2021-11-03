using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Service
{
    public class CourseOgnp
    {
        private readonly List<Stream> _streams;

        public CourseOgnp(string name)
        {
            CheckName(name);

            Name = name;
            _streams = new List<Stream>();
        }

        public string Name { get; }
        public IReadOnlyList<Stream> StreamsOfCourse => _streams;

        internal Stream AddStream(string name)
        {
            var stream = new Stream(name);
            CheckStreamOnExist(stream);

            _streams.Add(stream);
            return stream;
        }

        private static void CheckName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
        }

        private void CheckStreamOnExist(Stream newStream)
        {
            if (_streams.Any(stream => stream.Name == newStream.Name))
            {
                throw new StreamAlreadyExistExceptiom();
            }
        }
    }
}