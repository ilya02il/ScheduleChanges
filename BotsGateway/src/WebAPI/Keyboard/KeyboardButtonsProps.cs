using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using static ChatBot.HandleMessageResponse.Types;

namespace WebAPI.Keyboard
{
    public static class KeyboardButtonsProps
    {
        public static readonly Dictionary<KeyboardButtons?, (string, string)> _keboardButtonsProps;

        static KeyboardButtonsProps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

            _keboardButtonsProps = new()
            {
                { KeyboardButtons.GetCurrentDateSchedule, ("Расписание на сегодня", "/currentDate") },
                { KeyboardButtons.GetTommorowSchedule, ("Расписание на завтра", "/tommorow") },
                { KeyboardButtons.GoBack, ("Назад", "/back") },
                { KeyboardButtons.Cancel, ("Отмена", "/cancel") }
            };
        }

        public static KeyboardButtons? GetButtonFromTitle(string title)
        {
            return _keboardButtonsProps.FirstOrDefault(b => b.Value.Item1 == title).Key;
        }

        public static string GetButtonTitle(KeyboardButtons button)
        {
            return _keboardButtonsProps[button].Item1;
        }

        public static string GetButtonCommand(KeyboardButtons button)
        {
            return _keboardButtonsProps[button].Item2;
        }
    }
}
