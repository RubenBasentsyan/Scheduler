using System;

namespace WebApplication1.Helpers
{
    public class ConcurrencyLimitExceededException : Exception
    {
        public override string Message => "Tried to assign a time slot that is fully booked.";
    }

    public class ImpossibleScheduleException : Exception
    {
        public override string Message =>
            "Exam scheduling failed, " +
            "a timetable with the current parameters (Days, Time Slots, Rooms) is impossible," +
            " try making these parameters less strict";
    }
}