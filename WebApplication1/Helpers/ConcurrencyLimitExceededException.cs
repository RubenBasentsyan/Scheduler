using System;

namespace WebApplication1.Helpers
{
    public class ConcurrencyLimitExceededException : Exception
    {
        public override string Message => "Tried to assign a time slot that is fully booked.";
    }
}