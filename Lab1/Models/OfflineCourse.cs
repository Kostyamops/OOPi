namespace Lab1.Models
{
    public class OfflineCourse : Course
    {
        public string Room { get; set; }

        public OfflineCourse(int id, string name, string room) : base(id, name)
        {
            Room = room;
        }

        public override string GetInfo()
        {
            return $"[Офлайн] {Name}, аудитория: {Room}";
        }
    }
}