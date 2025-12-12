using System;
using Lab1.Core;
using Lab1.Models;

namespace Lab1
{
    class Program
    {
        static CourseManager mgr = new CourseManager();
        static int courseId = 1, teacherId = 1, studentId = 1;

        static void Main()
        {
            InitTestData();

            while (true)
            {
                Console.WriteLine("\n======= МЕНЮ =======");
                Console.WriteLine("1. Добавить преподавателя");
                Console.WriteLine("2. Добавить студента");
                Console.WriteLine("3. Создать онлайн-курс");
                Console.WriteLine("4. Создать офлайн-курс");
                Console.WriteLine("5. Удалить курс");
                Console.WriteLine("6. Назначить преподавателя на курс");
                Console.WriteLine("7. Записать студента на курс");
                Console.WriteLine("8. Все преподаватели");
                Console.WriteLine("9. Все студенты");
                Console.WriteLine("10. Все курсы");
                Console.WriteLine("11. Курсы преподавателя");
                Console.WriteLine("12. Студенты курса");
                Console.WriteLine("0. Выход");
                Console. Write("> ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        Console.Write("Имя преподавателя:  ");
                        mgr.AddTeacher(new Teacher(teacherId++, Console.ReadLine()));
                        Console.WriteLine("Добавлен!");
                        break;

                    case "2": 
                        Console.Write("Имя студента: ");
                        mgr.AddStudent(new Student(studentId++, Console. ReadLine()));
                        Console. WriteLine("Добавлен!");
                        break;

                    case "3":
                        Console.Write("Название:  ");
                        string n1 = Console.ReadLine();
                        Console.Write("Платформа: ");
                        mgr.AddCourse(new OnlineCourse(courseId++, n1, Console.ReadLine()));
                        Console.WriteLine("Курс создан!");
                        break;

                    case "4": 
                        Console.Write("Название: ");
                        string n2 = Console.ReadLine();
                        Console.Write("Аудитория: ");
                        mgr.AddCourse(new OfflineCourse(courseId++, n2, Console. ReadLine()));
                        Console. WriteLine("Курс создан!");
                        break;

                    case "5":
                        Console.Write("ID курса: ");
                        if (int.TryParse(Console.ReadLine(), out int delId))
                            Console.WriteLine(mgr. RemoveCourse(delId) ? "Удалён!" :  "Не найден!");
                        break;

                    case "6":
                        Console.Write("ID курса: ");
                        int.TryParse(Console.ReadLine(), out int cId);
                        Console.Write("ID преподавателя:  ");
                        int.TryParse(Console.ReadLine(), out int tId);
                        Console.WriteLine(mgr.AssignTeacher(cId, tId) ? "Назначен!" :  "Ошибка!");
                        break;

                    case "7":
                        Console. Write("ID курса: ");
                        int.TryParse(Console.ReadLine(), out int cId2);
                        Console.Write("ID студента: ");
                        int.TryParse(Console.ReadLine(), out int sId);
                        Console.WriteLine(mgr.EnrollStudent(cId2, sId) ? "Записан!" :  "Ошибка!");
                        break;

                    case "8":
                        if (mgr.Teachers.Count == 0) { Console.WriteLine("Нет преподавателей"); break; }
                        foreach (var t in mgr.Teachers)
                            Console.WriteLine($"  [{t.Id}] {t.Name}");
                        break;

                    case "9":
                        if (mgr.Students.Count == 0) { Console.WriteLine("Нет студентов"); break; }
                        foreach (var s in mgr.Students)
                            Console.WriteLine($"  [{s. Id}] {s.Name}");
                        break;

                    case "10":
                        if (mgr.Courses. Count == 0) { Console.WriteLine("Нет курсов"); break; }
                        foreach (var c in mgr. Courses)
                        {
                            string t = c.Teacher != null ? c.Teacher.Name :  "не назначен";
                            Console.WriteLine($"  [{c.Id}] {c. GetInfo()} | Препод: {t}");
                        }
                        break;

                    case "11":
                        Console.Write("ID преподавателя: ");
                        if (int.TryParse(Console.ReadLine(), out int tid))
                        {
                            var teacher = mgr.GetTeacher(tid);
                            if (teacher == null) { Console.WriteLine("Не найден! "); break; }
                            var list = mgr.GetCoursesByTeacher(tid);
                            Console.WriteLine($"Курсы преподавателя {teacher.Name}:");
                            if (list.Count == 0) Console.WriteLine("  Нет курсов");
                            foreach (var c in list)
                                Console.WriteLine($"  [{c.Id}] {c.GetInfo()}");
                        }
                        break;

                    case "12":
                        Console.Write("ID курса: ");
                        if (int.TryParse(Console. ReadLine(), out int cid))
                        {
                            var course = mgr.GetCourse(cid);
                            if (course == null) { Console.WriteLine("Не найден!"); break; }
                            Console. WriteLine($"Студенты курса {course.Name}:");
                            if (course.Students. Count == 0) Console.WriteLine("  Нет студентов");
                            foreach (var s in course.Students)
                                Console.WriteLine($"  [{s.Id}] {s.Name}");
                        }
                        break;

                    case "0": 
                        return;
                }
            }
        }

        static void InitTestData()
        {
            mgr.AddTeacher(new Teacher(teacherId++, "Иванов Иван Иванович"));
            mgr.AddTeacher(new Teacher(teacherId++, "Петрова Мария Сергеевна"));
            mgr.AddTeacher(new Teacher(teacherId++, "Сидоров Алексей Петрович"));

            mgr.AddStudent(new Student(studentId++, "Козлов Дмитрий"));
            mgr.AddStudent(new Student(studentId++, "Смирнова Анна"));
            mgr.AddStudent(new Student(studentId++, "Новиков Артём"));
            mgr.AddStudent(new Student(studentId++, "Морозова Елена"));

            mgr.AddCourse(new OnlineCourse(courseId++, "Математический анализ", "Zoom"));
            mgr.AddCourse(new OnlineCourse(courseId++, "ООП", "Телемост"));
            mgr.AddCourse(new OfflineCourse(courseId++, "Физика Базовая", "1112"));
            mgr.AddCourse(new OfflineCourse(courseId++, "Базы данных", "202"));

            mgr.AssignTeacher(1, 1);
            mgr.AssignTeacher(2, 2);
            mgr.AssignTeacher(3, 1);
            mgr.AssignTeacher(4, 3);

            mgr.EnrollStudent(1, 1);
            mgr.EnrollStudent(1, 2);
            mgr.EnrollStudent(2, 1);
            mgr.EnrollStudent(2, 3);
            mgr.EnrollStudent(2, 4);
            mgr.EnrollStudent(3, 2);
            mgr.EnrollStudent(3, 3);
            mgr.EnrollStudent(4, 1);
            mgr.EnrollStudent(4, 4);

            Console.WriteLine("Тестовые данные загружены!");
        }
    }
}