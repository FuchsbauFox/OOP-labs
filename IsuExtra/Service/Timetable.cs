using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Service
{
    public class Timetable
    {
        private readonly List<Pair> _pairs;

        public Timetable()
        {
            _pairs = new List<Pair>();
        }

        public IReadOnlyList<Pair> PairsOfTimetable => _pairs;

        public void AddPair(string name, string day, string time)
        {
            var pair = new Pair(name, day, time);
            CheckTimePair(pair);

            _pairs.Add(pair);
        }

        private void CheckTimePair(Pair newPair)
        {
            if (_pairs.Any(pair => pair.Time == newPair.Time && pair.Day == newPair.Day))
            {
                throw new TimeAlreadyTakenException();
            }
        }
    }
}