using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Mappings.TableMappings;
using Domain.ValueObjects;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var defaultTableMap = new TableMap<ItemInfo>();

            defaultTableMap.AddColumnMap(item => item.Position, "Лента", "Lesson position");
            defaultTableMap.AddColumnMap(item => item.SubjectName, "Дисциплина", "Subject");
            defaultTableMap.AddColumnMap(item => item.TeacherInitials, "Преподаватель", "Teacher");
            defaultTableMap.AddColumnMap(item => item.Auditorium, "Аудитория", "Аудит.", "Auditorium", "Audit.");

            services.AddTransient<ITableMap<ItemInfo>>(f => defaultTableMap);

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
