using Domain.Common;

namespace Domain.ValueObjects
{
    public enum KeyboardButton
    {
        GetCurrentDateSchedule = 0,
        GetTommorowSchedule,
        Cancel,
        GoBack

        //public static KeyboardButton GetCurrentDateSchedule = new(0, nameof(GetCurrentDateSchedule), "Расписание на сегодня", "/getCurrentDateSchedule");
        //public static KeyboardButton GetCallSchedule        = new(1, nameof(GetCallSchedule), "Расписание звонков", "/getCallSchedule");
    }
}
