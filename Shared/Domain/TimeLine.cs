using Faith.Shared.DTOs;
using System;
using System.Collections.Generic;

namespace Faith.Shared.Domain
{
    public class TimeLine
    {
        public Guid TimeLineID { get; set; } = new Guid();
        public ICollection<Post> Posts { get; set; }

        public TimeLine()
        {
            Posts = new List<Post>();
        }

        public TimeLine(TimeLineDTO timeline)
        {
            TimeLineID = timeline.TimeLineID;
            if (timeline.Posts != null)
            {
                Posts = new List<Post>();
                foreach (var post in timeline.Posts)
                {
                    Posts.Add(new Post(post));
                }
            }
        }
    }
}

/*        public DateTime GetDate()
        {
            return new DateTime(Date.Year, Date.Month, Date.Day);
        }

        public int GetWeek()
        {
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        public int GetMonth()
        {
            return CultureInfo.InvariantCulture.Calendar.GetMonth(Date);
        }

        public bool PlannedOnWeek(int week)
        {
            return PlannedOnWeek(DateTime.Now.Year, week);
        }

        public bool PlannedOnWeek(int year, int week)
        {
            return Date.Year == year && GetWeek() == week;
        }

        public bool PlannedOnMonth(int month)
        {
            return PlannedOnMonth(DateTime.Now.Year, month);
        }

        public bool PlannedOnMonth(int year, int month)
        {
            return Date.Year == year && GetMonth() == month;
        }

        public bool PlannedOnDate(int year, int month, int day)
        {
            return GetDate() == new DateTime(year, month, day);
        }*/