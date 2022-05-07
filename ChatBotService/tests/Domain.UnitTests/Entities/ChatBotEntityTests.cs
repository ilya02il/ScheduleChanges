using Domain.Entities;
using Domain.Events;
using Domain.ValueObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Domain.UnitTests.Entities
{
    //public class ChatBotEntityTests
    //{
    //    private readonly Dictionary<string, string> _responses = BotResponsesStrings.GetResponsesStrings();

    //    private const string DefaultEducOrgName = "Default education organization name";
    //    private const string DefaultGroupNumber = "GROUP";

    //    private string[] SetupChatEducOrgInfo(ChatBotEntity chatBot)
    //    {
    //        var userInfo = new UserInfo()
    //        {
    //            EducationalInfo = new(),
    //            Username = "ilya02il"
    //        };

    //        var message = DefaultEducOrgName;

    //        chatBot.CheckEducOrgName += (source, args) =>
    //            args.EducOrgName == message;

    //        chatBot.HandleMessage("/edit");
    //        return chatBot.HandleMessage(message);
    //    }

    //    private string[] SetupChatEducInfo(ChatBotEntity chatBot)
    //    {
    //        SetupChatEducOrgInfo(chatBot);

    //        var groupNumber = DefaultGroupNumber;

    //        chatBot.CheckGroupNumber += (source, args) =>
    //            args.GroupNumber == groupNumber;

    //        return chatBot.HandleMessage(groupNumber);
    //    }

    //    private DatedScheduleEntity SetupDatedSchedule(object source, GetDatedScheduleEventArgs args)
    //    {
    //        var educInfo = new EducationalInfo()
    //        {
    //            EducOrgName = DefaultEducOrgName,
    //            GroupNumber = DefaultGroupNumber
    //        };

    //        var datedSchedule = new DatedScheduleEntity()
    //        {
    //            EducationalInfo = educInfo,
    //            Date = new DateTimeOffset(2022, 02, 21, 0, 0, 0, TimeSpan.FromHours(7)),
    //            ScheduleItems = new()
    //            {
    //                new(1, new()
    //                {
    //                    Discipline = "lesson1",
    //                    Teacher = "teacher1",
    //                    Auditorium = "aud1"
    //                }),
    //                new(1, new()
    //                {
    //                    Discipline = "lesson2",
    //                    Teacher = "teacher2",
    //                    Auditorium = "aud2"
    //                })
    //            }
    //        };

    //        return datedSchedule;
    //    }

    //    [Test]
    //    public void Response_Should_Be_HelpMsg1_And_State_Should_Be_Selecting()
    //    {
    //        var chatBot = new ChatBotEntity();

    //        var result = chatBot.HandleMessage("/help");

    //        Assert.AreEqual(BotStates.Selecting, chatBot.State);
    //        Assert.AreEqual(new[]
    //        {
    //            _responses["helpMsg1"],
    //            _responses["helpMsg2"]
    //        }, result);
    //    }
        
    //    [Test]
    //    public void Response_Should_Be_EditInfoMsg1()
    //    {
    //        var chatBot = new ChatBotEntity();

    //        var result = chatBot.HandleMessage("/edit");

    //        Assert.AreEqual(_responses["editInfoMsg1"], result[0]);
    //    }

    //    [Test]
    //    public void State_Should_Be_EducOrgEditing()
    //    {
    //        var chatBot = new ChatBotEntity();

    //        chatBot.HandleMessage("/edit");

    //        Assert.AreEqual(BotStates.EducOrgEditing, chatBot.State);
    //    }

    //    [Test]
    //    public void State_Should_Be_GroupNumEditing_And_Msg_Should_Be_EditInfoMsg2()
    //    {
    //        var userInfo = new UserInfo()
    //        {
    //            EducationalInfo = new(),
    //            Username = "ilya02il"
    //        };
    //        var chatBot = new ChatBotEntity(1, userInfo);

    //        var result = SetupChatEducOrgInfo(chatBot);

    //        Assert.AreEqual(BotStates.GroupNumEditing, chatBot.State);
    //        Assert.AreEqual(_responses["editInfoMsg2"], result[0]);
    //    }

    //    [Test]
    //    public void State_Should_Be_Selecting_And_Msg_Should_Be_SucceedEditMsg()
    //    {
    //        var userInfo = new UserInfo()
    //        {
    //            EducationalInfo = new(),
    //            Username = "ilya02il"
    //        };
    //        var chatBot = new ChatBotEntity(1, userInfo);

    //        var result = SetupChatEducInfo(chatBot);

    //        Assert.AreEqual(BotStates.Selecting, chatBot.State);
    //        Assert.AreEqual(new[]
    //        {
    //            _responses["succeedEditMsg"],
    //            _responses["helpMsg1"],
    //            _responses["helpMsg2"]
    //        }, result);
    //    }

    //    [Test]
    //    public void Response_Should_Be_FormattedSchedule()
    //    {
    //        var userInfo = new UserInfo()
    //        {
    //            EducationalInfo = new(),
    //            Username = "ilya02il"
    //        };

    //        var chatBot = new ChatBotEntity(1, userInfo);

    //        SetupChatEducInfo(chatBot);
    //        chatBot.GetDatedSchedule += SetupDatedSchedule;

    //        var assertion = "Расписание на 21.02.2022 с учетом изменений:\n1 лента - lesson1, teacher1, aud1\n2 лента - lesson2, teacher2, aud2";

    //        var result = chatBot.HandleMessage("21.02.2022");
    //        Assert.AreEqual(assertion, result[0]);
    //    }

    //    [Test]
    //    public void Response_Should_Be_UnsupportedFormatMsg()
    //    {
    //        var userInfo = new UserInfo()
    //        {
    //            EducationalInfo = new(),
    //            Username = "ilya02il"
    //        };

    //        var chatBot = new ChatBotEntity(1, userInfo);

    //        var result = chatBot.HandleMessage("sdfgfsdgsdf");
    //        Assert.AreEqual(_responses["unsupportedFormatMsg"], result[0]);
    //    }
    //}
}
