using System;

namespace Application.ChangesLists.Commands.CreateChangesList
{
    public class ChangesListItemDto
    {
        public Guid Id { get; init; }
        public int Position { get; init; }
        public string GroupNumber { get; init; }
        public string SubjectName { get; init; }
        public string TeacherInitials { get; init; }
        public string Auditorium { get; init; }
    }
}
