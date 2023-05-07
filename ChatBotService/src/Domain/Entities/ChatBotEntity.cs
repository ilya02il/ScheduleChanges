using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Domain.Dtos;
using Domain.Events;
using Domain.ValueObjects;
using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.Entities
{
    internal delegate Task<ChatBotResponseDto> StateActionDelegate(string message = "");

    public class ChatBotEntity : BaseEntity
    {
        public event GetDatedScheduleEventHandler GetDatedSchedule;
        public event GetEducOrgsListEventHandler GetEducOrgsList;
        public event GetGroupNumbersListEventHandler GetGroupNumbersList;

        public long PlatformSpecificChatId { get; private set; }
        public long PlatformHash { get; private set; }
        public UserInfo UserInfo { get; private set; }
        public BotStates State { get; private set; } = BotStates.Selecting;
        public bool IsAwaiting { get; private set; } = false;

        private readonly Dictionary<BotStates, StateActionDelegate> _statesActions;
        private readonly IDictionary<string, string> _responses = BotResponsesStrings.ResponsesStrings;
        private readonly List<string> _messages = new();

        private DateTimeOffset _messageDate;

        private const string StartCommand = "/start";
        private const string CurrentDateCommand = "/currentDate";
        private const string CancelCommand = "/cancel";
        private const string GoBackCommand = "/back";
        private const string TommorowCommand = "/tommorow";
        private const string HelpCommand = "/help";
        private const string EditCommand = "/editinfo";

        private readonly KeyboardButton[] _standartMarkup = new[]
        {
            KeyboardButton.GetCurrentDateSchedule,
            KeyboardButton.GetTommorowSchedule
        };
        private readonly KeyboardButton[] _editingMarkup = new[]
        {
            KeyboardButton.GoBack,
            KeyboardButton.Cancel
        };
        private readonly KeyboardButton[] _emptyMarkup = Array.Empty<KeyboardButton>();

        private const string DateRegexPattern = @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]|(?:Jan|Mar|May|Jul|Aug|Oct|Dec)))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2|(?:Feb))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9]|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep))|(?:1[0-2]|(?:Oct|Nov|Dec)))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$";

        private ChatBotEntity()
        {
            _statesActions = new()
            {
                { BotStates.Selecting, SelectCommand },
                { BotStates.EducOrgEditing, EditEducOrgName },
                { BotStates.SelectingYearOfStudy, SelectYearOfStudy },
                { BotStates.GroupNumEditing, EditGroupNumber }
            };
        }

        public ChatBotEntity(long platformSpecificChatId, long platformHash, UserInfo userInfo) : this()
        {
            PlatformSpecificChatId = platformSpecificChatId;
            PlatformHash = platformHash;
            UserInfo = userInfo;
        }

        public async Task<ChatBotResponseDto> HandleMessage(string message, DateTimeOffset messageDate)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("Message is null or empty.");

            switch (message)
            {
                case CancelCommand:
                    State = BotStates.Selecting;
                    IsAwaiting = false;
                    _messages.Add(_responses["cancelMsg"]);
                    return await Task.FromResult(new ChatBotResponseDto(this, _messages, _standartMarkup));
                case GoBackCommand:
                    State -= 1;
                    IsAwaiting = false;
                    return await _statesActions[State].Invoke();
            }

            _messageDate = messageDate;

            return await _statesActions[State].Invoke(message);
        }

        private async Task<ChatBotResponseDto> SelectCommand(string message)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

            switch (message)
            {
                case StartCommand:
                    return await StartCommandHandler();
                case HelpCommand:
                    return HelpCommandHandler();
                case EditCommand:
                    return await EditCommandHandler();
                case TommorowCommand:
                    State = BotStates.Selecting;
                    return await ShowDatedSchedule(_messageDate.AddDays(1));
                case CurrentDateCommand:
                    State = BotStates.Selecting;
                    return await ShowDatedSchedule(_messageDate);
                default:
                    return await DefaultCommandHandler(message);
            }
        }

        private async Task<ChatBotResponseDto> StartCommandHandler()
        {
            _messages.Add(_responses["startMsg"]);

            State = BotStates.EducOrgEditing;
            return await _statesActions[State].Invoke();
        }

        private ChatBotResponseDto HelpCommandHandler()
        {
            State = BotStates.Selecting;
            _messages.AddRange(new[]
            {
                _responses["helpMsg1"],
                _responses["helpMsg2"]
            });

            return new(this, _messages, _standartMarkup);
        }

        private async Task<ChatBotResponseDto> EditCommandHandler()
        {
            State = BotStates.EducOrgEditing;
            return await _statesActions[State].Invoke();
        }

        private async Task<ChatBotResponseDto> DefaultCommandHandler(string message)
        {
            State = BotStates.Selecting;

            var dateRegex = new Regex(DateRegexPattern, RegexOptions.Compiled);

            if (!dateRegex.IsMatch(message))
            {
                _messages.Add(_responses["unsupportedFormatMsg"]);
                return new(this, _messages, _standartMarkup);
            }

            var date = (DateTimeOffset)Convert.ToDateTime(message);

            return await ShowDatedSchedule(date);
        }

        private async Task<IList<string>> ShowEducOrgsList()
        {
            var messages = new List<string>();

            var educOrgNames = await GetEducOrgsList(this);

            int i = 0;

            if (!educOrgNames.Any())
            {
                messages.Add(_responses["editInfoEmptyListMsg"]);
                return messages;
            }

            foreach (var educOrgName in educOrgNames)
            {
                messages.Add(string.Format(_responses["editInfoMsgTemplate1"], i + 1, educOrgName));
                i++;
            }

            return messages;
        }

        private async Task<IList<string>> ShowGroupNumbersList()
        {
            var messages = new List<string>();

            var args = new GetGroupNumbersListEventArgs(UserInfo.EducationalInfo.EducOrgName, UserInfo.EducationalInfo.YearOfStudy);

            var groupNumbers = await GetGroupNumbersList(this, args);

            int i = 0;

            if (!groupNumbers.Any())
            {
                messages.Add(_responses["editInfoEmptyListMsg"]);
                return messages;
            }

            foreach (var groupNumber in groupNumbers)
            {
                messages.Add(string.Format(_responses["editInfoMsgTemplate1"], i + 1, groupNumber));
                i++;
            }

            return messages;
        }

        private async Task<ChatBotResponseDto> EditEducOrgName(string message)
        {
            if (!IsAwaiting)
            {
                _messages.AddRange(await ShowEducOrgsList());
                _messages.Add(_responses["editInfoMsg1"]);

                IsAwaiting = true;

                return new(this, _messages, _editingMarkup);
            }

            try
            {
                int idx = Convert.ToInt32(message);
                idx--;

                var educOrgNames = await GetEducOrgsList(this);

                UserInfo.EducationalInfo = new()
                {
                    EducOrgName = educOrgNames[idx]
                };

                State = BotStates.SelectingYearOfStudy;

                IsAwaiting = false;

                return await _statesActions[State].Invoke();
            }
            catch
            {
                _messages.AddRange(new[]
                {
                    _responses["editInfoFailedMsg1"],
                    _responses["editInfoFailedMsg2"]
                });

                return new(this, _messages, _emptyMarkup);
            }
        }

        private async Task<ChatBotResponseDto> SelectYearOfStudy(string message)
        {
            if (!IsAwaiting)
            {
                _messages.Add(_responses["editInfoMsg3"]);

                IsAwaiting = true;

                return new(this, _messages, _editingMarkup);
            }

            try
            {
                int idx = Convert.ToInt32(message);

                UserInfo.EducationalInfo.YearOfStudy = idx;

                State = BotStates.GroupNumEditing;

                IsAwaiting = false;

                return await _statesActions[State].Invoke();
            }
            catch
            {
                _messages.AddRange(new[]
                {
                    _responses["unsupportedFormatMsg"],
                    _responses["editInfoFailedMsg6"]
                });

                return new(this, _messages, _emptyMarkup);
            }
        }

        private async Task<ChatBotResponseDto> EditGroupNumber(string message)
        {
            if (!IsAwaiting)
            {
                _messages.AddRange(await ShowGroupNumbersList());
                _messages.Add(_responses["editInfoMsg2"]);

                IsAwaiting = true;

                return new(this, _messages, _editingMarkup);
            }

            try
            {
                int idx = Convert.ToInt32(message);
                idx--;

                var args = new GetGroupNumbersListEventArgs(UserInfo.EducationalInfo.EducOrgName, UserInfo.EducationalInfo.YearOfStudy);

                var groupNumbers = await GetGroupNumbersList(this, args);

                UserInfo.EducationalInfo.GroupNumber = groupNumbers[idx];
                State = BotStates.Selecting;

                _messages.AddRange(new[]
                {
                    _responses["succeedEditMsg"],
                    _responses["helpMsg1"],
                    _responses["helpMsg2"]
                });

                IsAwaiting = false;

                return new(this, _messages, _standartMarkup);
            }
            catch
            {
                _messages.AddRange(new[]
                {
                    _responses["editInfoFailedMsg3"],
                    _responses["editInfoFailedMsg4"]
                });

                return new(this, _messages, _emptyMarkup);
            }
        }

        private async Task<ChatBotResponseDto> ShowDatedSchedule(DateTimeOffset date)
        {
            if (UserInfo.EducationalInfo.EducOrgName is null ||
                UserInfo.EducationalInfo.GroupNumber is null)
            {
                _messages.Add(_responses["notCompleteEducInfoMsg"]);

                return new(this, _messages, _standartMarkup);
            }

            var args = new GetDatedScheduleEventArgs(UserInfo.EducationalInfo.EducOrgName, UserInfo.EducationalInfo.GroupNumber, date.LocalDateTime);

            if (GetDatedSchedule is null)
                throw new NullReferenceException("The get dated schedule event wasn't assigned.");

            var datedSchedule = await GetDatedSchedule(this, args);

            if (datedSchedule.ScheduleItems.Any())
            {
                _messages.Add(
                string.Format(_responses["scheduleMsgTemplate1"],
                    datedSchedule.Date.LocalDateTime.ToShortDateString()
                    )
                );

                var timeFormatString = @"hh\:mm";

                for (int i = 0; i < datedSchedule.ScheduleItems.Count; i++)
                {
                    _messages.Add(string.Format("\n" + _responses["scheduleMsgTemplate2"],
                            datedSchedule.ScheduleItems[i].Position,
                            datedSchedule.ScheduleItems[i].StartTime.ToString(timeFormatString),
                            datedSchedule.ScheduleItems[i].EndTime.ToString(timeFormatString),
                            datedSchedule.ScheduleItems[i].Discipline,
                            datedSchedule.ScheduleItems[i].Teacher,
                            datedSchedule.ScheduleItems[i].Auditorium
                            )
                        );
                }
            }

            else
            {
                _messages.Add(string.Format(_responses["emptyScheduleMsg"],
                        datedSchedule.Date.LocalDateTime.ToShortDateString())
                    );
            }

            return new(this, _messages, _standartMarkup);
        }
    }
}
