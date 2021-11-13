using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Service
{
    public class Timetable
    {
        private readonly List<Lesson> _pairs;

        public Timetable()
        {
            _pairs = new List<Lesson>();
        }

        public IReadOnlyList<Lesson> PairsOfTimetable => _pairs;

        internal void AddPair(string name, DateTime time)
        {
            var pair = new Lesson(name, time);
            CheckTimePair(pair);

            _pairs.Add(pair);
        }

        private void CheckTimePair(Lesson newLesson)
        {
            if (_pairs.Any(pair => pair.Time == newLesson.Time))
            {
                throw new TimeAlreadyTakenException();
            }
        }
    }
}