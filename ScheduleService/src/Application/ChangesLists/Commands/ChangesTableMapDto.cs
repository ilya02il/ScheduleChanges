namespace Application.ChangesLists.Commands
{
    public record ChangesTableMapDto
    {
        public string GroupNumber { get; init; }
        public int Position { get; init; }
        public string SubjectName { get; init; }
        public string TeacherInitials { get; init; }
        public string Auditorium { get; init; }

    }
}
