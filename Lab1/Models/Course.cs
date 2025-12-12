using System. Collections.Generic;

namespace Lab1.Models
{
    public abstract class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();

        public Course(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract string GetInfo();

        public void AddStudent(Student s)
        {
            if (!Students.Contains(s))
                Students.Add(s);
        }
    }
}