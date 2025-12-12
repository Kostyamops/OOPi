using System. Collections.Generic;
using System.Linq;
using Lab1.Models;

namespace Lab1.Core
{
    public class CourseManager
    {
        public List<Course> Courses { get; } = new List<Course>();
        public List<Teacher> Teachers { get; } = new List<Teacher>();
        public List<Student> Students { get; } = new List<Student>();

        public void AddCourse(Course c) => Courses.Add(c);

        public bool RemoveCourse(int id)
        {
            var c = Courses.FirstOrDefault(x => x.Id == id);
            if (c == null) return false;
            Courses.Remove(c);
            return true;
        }

        public void AddTeacher(Teacher t) => Teachers.Add(t);
        public void AddStudent(Student s) => Students.Add(s);

        public bool AssignTeacher(int courseId, int teacherId)
        {
            var c = Courses.FirstOrDefault(x => x.Id == courseId);
            var t = Teachers.FirstOrDefault(x => x.Id == teacherId);
            if (c == null || t == null) return false;
            c.Teacher = t;
            return true;
        }

        public bool EnrollStudent(int courseId, int studentId)
        {
            var c = Courses. FirstOrDefault(x => x. Id == courseId);
            var s = Students.FirstOrDefault(x => x.Id == studentId);
            if (c == null || s == null) return false;
            c.AddStudent(s);
            return true;
        }

        public List<Course> GetCoursesByTeacher(int teacherId)
        {
            return Courses.Where(c => c.Teacher != null && c.Teacher.Id == teacherId).ToList();
        }

        public Course GetCourse(int id) => Courses.FirstOrDefault(x => x.Id == id);
        public Teacher GetTeacher(int id) => Teachers.FirstOrDefault(x => x.Id == id);
    }
}