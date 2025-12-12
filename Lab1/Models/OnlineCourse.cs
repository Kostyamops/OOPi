namespace Lab1.Models
{
    public class OnlineCourse : Course
    {
        public string Platform { get; set; }

        public OnlineCourse(int id, string name, string platform) : base(id, name)
        {
            Platform = platform;
        }

        public override string GetInfo()
        {
            return $"[Онлайн] {Name}, платформа: {Platform}";
        }
    }
}