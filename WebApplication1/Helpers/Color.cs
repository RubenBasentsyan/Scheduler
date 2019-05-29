using WebApplication1.Classes;

namespace WebApplication1.Helpers
{
    public class Color
    {
        private static int[,] _limits = new int[MaxDays, MaxTimeSlots];

        private Color(int day, int timeSlot)
        {
            Day = day;
            TimeSlot = timeSlot;
        }

        public int Day { get; }
        public int TimeSlot { get; }

        public static int MaxDays { get; private set; } = 6;
        public static int MaxTimeSlots { get; private set; } = 3;
        public static int ConcurrencyLimit { get; private set; } = 6;

        /// <summary>
        ///     Sets the concurrency limit, if it's stricter than the previous,
        ///     the parent method should call for an automatic reschedule of the graph
        /// </summary>
        /// <param name="limit">The new concurrency limit</param>
        /// <returns>Whether or not the new concurrency limit is stricter(smaller than the prev. limit)</returns>
        public static bool SetConcurrencyLimit(int limit)
        {
            var isStricter = limit <= ConcurrencyLimit;
            ConcurrencyLimit = limit;
            return isStricter;
        }

        /// <summary>
        ///     Sets the maximum days. If the new number of days is less stricter than the old one,
        ///     repopulates the concurrency limits of the schedule into the new _limits table,
        ///     otherwise, the parent method should for an automatic reschedule of the graph
        /// </summary>
        /// <param name="days">The maximum number of days.</param>
        /// <returns>Whether or not the new number of days is stricter(smaller no. of allocated days).</returns>
        public static bool SetMaxDays(int days)
        {
            var isStricter = days < MaxDays;
            MaxDays = days;
            _limits = new int[days, MaxTimeSlots];
            return isStricter;
        }

        /// <summary>
        ///     Sets the maximum time slots in a day. If the new number of time slots is less stricter than the old one,
        ///     repopulates the concurrency limits of the schedule into the new _limits table,
        ///     otherwise, the parent method should call for an automatic reschedule of the graph
        /// </summary>
        /// <param name="timeSlots">The time slots.</param>
        /// <returns>True if value changed, false ow</returns>
        public static bool SetMaxTimeSlots(int timeSlots)
        {
            var isStricter = timeSlots < MaxTimeSlots;
            MaxTimeSlots = timeSlots;
            _limits = new int[MaxDays, MaxTimeSlots];
            return isStricter;
        }

        /// <summary>
        ///     Resets the concurrency limits.
        /// </summary>
        public static void ResetLimits()
        {
            _limits = new int[MaxDays, MaxTimeSlots];
        }

        /// <summary>
        ///     Checks if enough room exists.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static bool EnoughRoomExists(int d, int t)
        {
            return _limits[d, t] != ConcurrencyLimit;
        }

        public static bool SetCourseColor(int d, int t, Course course)
        {
            if (_limits[d, t] >= ConcurrencyLimit) throw new ConcurrencyLimitExceededException();
            course.Color = new Color(d, t);
            _limits[d, t]++;
            return true;
        }
    }
}