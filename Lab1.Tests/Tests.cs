using Xunit;
using Lab1.Core;
using Lab1.Models;

namespace Lab1.Tests
{
    public class Tests
    {
        [Fact]
        public void AddCourse()
        {
            var mgr = new CourseManager();
            mgr.AddCourse(new OnlineCourse(1, "Математический анализ", "Zoom"));
            Assert.Single(mgr.Courses);
        }

        [Fact]
        public void RemoveCourse()
        {
            var mgr = new CourseManager();
            mgr.AddCourse(new OfflineCourse(1, "Физика Базовая", "1112"));
            Assert.True(mgr.RemoveCourse(1));
            Assert.Empty(mgr.Courses);
        }

        [Fact]
        public void RemoveCourseNotFound()
        {
            var mgr = new CourseManager();
            Assert.False(mgr.RemoveCourse(999));
        }

        [Fact]
        public void AssignTeacher()
        {
            var mgr = new CourseManager();
            mgr.AddCourse(new OnlineCourse(1, "ООП", "Телемост"));
            mgr.AddTeacher(new Teacher(1, "Слюсаренко"));
            Assert.True(mgr.AssignTeacher(1, 1));
            Assert.Equal("Иванов", mgr. Courses[0].Teacher.Name);
        }

        [Fact]
        public void EnrollStudent()
        {
            var mgr = new CourseManager();
            mgr.AddCourse(new OfflineCourse(1, "Проектирование и реализация баз данных", "202"));
            mgr.AddStudent(new Student(1, "Грицкевич"));
            Assert.True(mgr.EnrollStudent(1, 1));
            Assert.Single(mgr. Courses[0].Students);
        }

        [Fact]
        public void GetCoursesByTeacher()
        {
            var mgr = new CourseManager();
            mgr.AddTeacher(new Teacher(1, "Слюсаренко"));
            mgr.AddCourse(new OnlineCourse(1, "Курс1", "Zoom"));
            mgr.AddCourse(new OfflineCourse(2, "Курс2", "101"));
            mgr.AssignTeacher(1, 1);
            mgr.AssignTeacher(2, 1);
            Assert.Equal(2, mgr. GetCoursesByTeacher(1).Count);
        }

        [Fact]
        public void OnlineCourseInfo()
        {
            var c = new OnlineCourse(1, "История. Тестирование", "Moodle");
            Assert.Contains("Онлайн", c.GetInfo());
        }

        [Fact]
        public void OfflineCourseInfo()
        {
            var c = new OfflineCourse(1, "Тест", "303");
            Assert.Contains("Офлайн", c.GetInfo());
        }

        [Fact]
        public void NoDuplicateStudents()
        {
            var c = new OnlineCourse(1, "Курс", "Zoom");
            var s = new Student(1, "Иванова");
            c.AddStudent(s);
            c.AddStudent(s);
            Assert. Single(c.Students);
        }
    }
}