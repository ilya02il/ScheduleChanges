using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Common.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Events;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.ChatEntities.Commands.HandleMessageCommand
{
    public class HandleMessageCommand : IRequest<ChatBotResponseDto>
    {
        public long ChatId { get; init; }
        public long PlatformHash { get; init; }
        public string Username { get; init; }
        public DateTimeOffset Date { get; init; }
        public string Text { get; init; }
    }

    public class HanldeMessageCommandHandler 
        : IRequestHandler<HandleMessageCommand, ChatBotResponseDto>
    {
        private readonly IWriteDbContext _context;
        private readonly IDistributedCacheWrapper _distributedCache;
        //private readonly ILogger<HanldeMessageCommandHandler> _logger;
        private readonly IGrpcScheduleClientWrapper _grpcClient;

        public HanldeMessageCommandHandler(IWriteDbContext context,
            IDistributedCacheWrapper distributedCache,
            IGrpcScheduleClientWrapper grpcClient)
        {
            _context = context;
            _distributedCache = distributedCache;
            _grpcClient = grpcClient;
        }

        public async Task<ChatBotResponseDto> Handle(HandleMessageCommand request,
            CancellationToken cancellationToken)
        {

            var chat = _context.Chats.FirstOrDefault(c => c.PlatformSpecificChatId == request.ChatId &&
                c.PlatformHash == request.PlatformHash);

            if (chat is null)
            {
                var userInfo = new UserInfo()
                {
                    Username = request.Username
                };

                chat = new ChatBotEntity(request.ChatId, request.PlatformHash, userInfo);
                _context.Chats.Add(chat);
            }

            chat.GetDatedSchedule += GetDatedSchedule;
            chat.GetEducOrgsList += GetEducOrgsList;
            chat.GetGroupNumbersList += GetGroupNumbersList;

            var response = await chat.HandleMessage(request.Text, request.Date);

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }

        private async Task<IList<string>> GetEducOrgsList(object source)
        {
            return await _grpcClient.GetEducOrgsList();
        }

        private async Task<IList<string>> GetGroupNumbersList(object source, GetGroupNumbersListEventArgs args)
        {
            var response = await _grpcClient.GetGroupNumbersList(args.EducOrgName, args.YearOfStudy);

            return response;
        }

        private async Task<DatedScheduleDto> GetDatedSchedule(object sender, GetDatedScheduleEventArgs args)
        {
            //var cacheKey = CacheKeyHelper.CreateHashedCacheKeyFromProps(args,
            //    arg => arg.Date.Date,
            //    arg => arg.EducOrgName,
            //    arg => arg.GroupNumber);

            //var result = await _distributedCache.GetStringAsync<DatedScheduleDto>(cacheKey);

            //if (result is null)
            //{
                var educInfo = new EducationalInfo()
                {
                    EducOrgName = args.EducOrgName,
                    GroupNumber = args.GroupNumber
                };

                var result = await _grpcClient.GetDatedSchedule(educInfo, args.Date);

                if (result is null)
                    throw new Exception("Dated schedule is not found.");

                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(6))
                    .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(12));

            //    await _distributedCache.SetStringAsync(result, cacheKey, options);
            //}

            return result;
        }
    }
}
