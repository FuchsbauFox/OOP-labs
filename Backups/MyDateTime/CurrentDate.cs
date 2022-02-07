using System;
using Newtonsoft.Json;

namespace Backups.MyDateTime
{
    public class CurrentDate
    {
        [JsonProperty]
        private static CurrentDate _instance;

        private CurrentDate()
        {
            Date = DateTime.Now;
        }

        public DateTime Date { get; private set; }

        public static CurrentDate GetInstance()
        {
            return _instance ??= new CurrentDate();
        }

        public void AddDays(int days)
        {
            CheckData(days);
            Date = Date.AddDays(days);
        }

        public void AddMonths(int months)
        {
            CheckData(months);
            Date = Date.AddMonths(months);
        }

        public void AddYears(int years)
        {
            CheckData(years);
            Date = Date.AddYears(years);
        }

        private void CheckData(int data)
        {
            if (data < 0)
            {
                throw new ArgumentException();
            }
        }
    }
}