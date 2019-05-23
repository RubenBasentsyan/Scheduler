using System;
using WebApplication1.Classes;

namespace WebApplication1.Helpers
{
    public enum Day
    {
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5
    }
    public enum TimeSlot
    {
        Morning = 0,
        Afternoon = 1,
        Evening = 2
    }
    public class Color
    {
        private static readonly Color[,] Colors = new Color[Enum.GetNames(typeof(Day)).Length, Enum.GetNames(typeof(TimeSlot)).Length];
        private static int[,] _limits = new int[Enum.GetNames(typeof(Day)).Length, Enum.GetNames(typeof(TimeSlot)).Length];
        private static int _concurrencyLimit = 5;
        /// <summary>
        /// Sets the concurrency limit
        /// </summary>
        /// <param name="limit">The new concurrency limit</param>
        /// <returns>True if the value changed, false otherwise</returns>
        public static bool SetConcurrencyLimit(int limit)
        {
            if (limit == _concurrencyLimit)
                return false;
            _concurrencyLimit = limit;
            return true;
        }

        public static void ResetLimits()
        {
            _limits =  new int[Enum.GetNames(typeof(Day)).Length, Enum.GetNames(typeof(TimeSlot)).Length];
        }

        public static bool EnoughRoomExists(Day d, TimeSlot t)
        {
            return _limits[(int) d, (int) t] != _concurrencyLimit;
        }

        public Day Day { get; }
        public TimeSlot TimeSlot { get; }
        private Color(Day day, TimeSlot timeSlot)
        {
            this.Day = day;
            this.TimeSlot = timeSlot;
        }
        public static bool SetCourseColor(Day d, TimeSlot t, Course course)
        {
            if (Colors[(int)d, (int)t] == null) Colors[(int)d, (int)t] = new Color(d, t);
            if (_limits[(int) d, (int) t] >= _concurrencyLimit) throw new ConcurrencyLimitExceededException();
            course.Color = Colors[(int)d, (int)t];
            _limits[(int)d, (int)t]++;
            return true;
        }
    }
    

}