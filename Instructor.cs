using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    public class Instructor : User
    {
        public List<int> CourseIds { get; set; }
        public string Department { get; set; }

        public Instructor(string username, string password, string email, string department) : base(username, password, email)
        {
            CourseIds = new List<int>();
            Department = department;

        }

        public override void DisplayDashboard()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════╗");
            Console.WriteLine("║      INSTRUCTOR DASHBOARD      ║");
            Console.WriteLine("╚════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine($"Welcome, Professor {Username}!");
            Console.WriteLine($"Teaching {CourseIds.Count} course(s)");
            Console.WriteLine($"Department: {Department}");
            Console.WriteLine();

            Console.WriteLine("[1] My Courses");
            Console.WriteLine("[2] Create Course");
            Console.WriteLine("[3] View Students");
            Console.WriteLine("[4] Grade");
            Console.WriteLine("[5] Logout");
        }

        public override string GetUserType()
        {
            return "Instructor";
        }

        public void AddCourse(int courseId)
        {
            if (!CourseIds.Contains(courseId))
            {
                CourseIds.Add(courseId);
                Console.WriteLine($"Added course with ID: {courseId}");
            }
            else
            {
                Console.WriteLine($"Already added course with ID: {courseId}");
            }
        }

        public void RemoveCouse(int courseId)
        {
            if (CourseIds.Contains(courseId))
            {
                CourseIds.Remove(courseId);
                Console.WriteLine($"Removed course with ID: {courseId}");
            }
            else
            {
                Console.WriteLine($"Course with ID: {courseId} not found in instructor's course list.");
            }
        }

        public virtual void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Department: {Department}");
            Console.WriteLine("Courses Taught: " + string.Join(", ", CourseIds));
        }

        public void ShowCoursesTaught(List<Course> courses, List<Enrollment> enrollments)
        {
            var myCourses = courses.Where(c => c.Instructor == this.Username).ToList();

            if (myCourses.Count == 0)
            {
                Console.WriteLine("No courses assigned.");
                return;
            }

            foreach (var course in myCourses)
            {
                var courseEnrollments = enrollments.Where(e => e.CourseId == course.CourseId).ToList();

                int studentCount = courseEnrollments.Count;
                double avgProgress = studentCount == 0 ? 0 : courseEnrollments.Average(e => e.ProgressPercentage);

                Console.WriteLine($"Course Title       : {course.CourseName}");
                Console.WriteLine($"Students Enrolled  : {studentCount}");
                Console.WriteLine($"Average Progress   : {avgProgress:F2}%");
            }
        }


        public int GetStudentCount(List<Enrollment> enrollments)
        {
            int studentCount = 0;
            foreach (var courseId in CourseIds)
            {
                studentCount += enrollments.Count(e => e.CourseId == courseId);
            }
            return studentCount;
        }

        public Course GetCourseById(int id, List<Course> courses)
        {
            return courses.FirstOrDefault(c => c.CourseId == id);
        }
    }
}
