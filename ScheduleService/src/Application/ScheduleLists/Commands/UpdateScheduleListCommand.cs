using Application.Common.Interfaces;
using Application.ScheduleLists.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleLists.Commands
{
    public class UpdateScheduleListCommand : IRequest<bool>
    {
        public Guid Id { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
        public IList<ScheduleListItemDto> ListItems { get; init; }
    }

    public class UpdateScheduleListCommandHandler : IRequestHandler<UpdateScheduleListCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateScheduleListCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateScheduleListCommand request, CancellationToken cancellationToken)
        {
            var scheduleList = _context.ScheduleLists
                .FirstOrDefault(sl => sl.Id == request.Id);

            var scheduleListItems = _context.ScheduleListItems
                .Where(item => item.ScheduleListId == scheduleList.Id)
                .ToList();

            scheduleList.UpdateDayOfWeek(request.DayOfWeek);

            if (request.ListItems.Any())
            {
                ItemInfo itemInfoBuffer;

                for (int i = 0; i < request.ListItems.Count; i++)
                {
                    if (request.ListItems[i].Id == Guid.Empty)
                    {
                        itemInfoBuffer = _mapper.Map<ItemInfo>(request.ListItems[i]);

                        var entity = new ScheduleListItemEntity(scheduleList.Id,
                                       request.ListItems[i].IsOddWeek,
                                       itemInfoBuffer);

                        _context.ScheduleListItems.Add(entity);
                        //_context.Entry(entity).State = EntityState.Added;
                        //scheduleList.AppendItem(entity);
                        request.ListItems.RemoveAt(i);
                        i--;
                    }
                }

                if (request.ListItems.Any())
                {
                    foreach (var listItem in scheduleListItems)
                    {
                        var updatedListItem = request.ListItems
                            .FirstOrDefault(li => li.Id == listItem.Id);

                        if (updatedListItem is not null)
                        {
                            itemInfoBuffer = _mapper.Map<ItemInfo>(updatedListItem);

                            listItem.UpdateItemInfo(itemInfoBuffer);
                            continue;
                        }

                        _context.ScheduleListItems.Remove(listItem);
                    }
                }
            }

            else
            {
                _context.ScheduleListItems.RemoveRange(scheduleListItems);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
