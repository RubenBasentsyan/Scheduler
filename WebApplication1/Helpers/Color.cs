using System;

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
        private static Color[,] Colors = new Color[Enum.GetNames(typeof(Day)).Length, Enum.GetNames(typeof(TimeSlot)).Length];
        private static int[,] Limits = new int[Enum.GetNames(typeof(Day)).Length, Enum.GetNames(typeof(TimeSlot)).Length];
        private static readonly int ConcurrencyLimit = 5;
        public Day day { get; private set; }
        public TimeSlot timeSlot { get; private set; }
        private Color(Day day, TimeSlot timeSlot)
        {
            this.day = day;
            this.timeSlot = timeSlot;
        }
        public static bool GetColor(Day d, TimeSlot t, out Color color)
        {
            if (Colors[(int)d, (int)t] == null)
                Colors[(int)d, (int)t] = new Color(d, t);
            if (Limits[(int)d, (int)t] < ConcurrencyLimit)
            {
                color = Colors[(int)d, (int)t];
                Limits[(int)d, (int)t]++;
                return true;
            }
            color = null;
            return false;
        }
    }
    

}