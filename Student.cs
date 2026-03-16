using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    public class Student : User, ISearchable, INotifiable
    {
        public List<int> EnrolledCourseIds { get; set; }
        public Dictionary<int, int> CourseProgress { get; set; }

        public Student(string username, string password, string email) : base(username, password, email)
        {
            EnrolledCourseIds = new List<int>();
            CourseProgress = new Dictionary<int, int>();
        }

        public override void DisplayDashboard()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════╗");
            Console.WriteLine("║       STUDENT DASHBOARD        ║");
            Console.WriteLine("╚════════════════════════════════╝");

            Console.WriteLine($"Welcome, {Username}!");
            Console.WriteLine($"Enrolled Courses: {EnrolledCourseIds.Count}");

            Console.WriteLine("[1] Browse Courses");
            Console.WriteLine("[2] Enroll In a Course");
            Console.WriteLine("[3] MyCourses");
            Console.WriteLine("[4] Progress");
            Console.WriteLine("[5] Drop Course");
            Console.WriteLine("[6] Statistics");
            Console.WriteLine("[7] Logout");
        }

        public override string GetUserType()
        {
            return "Student";
        }

        public void EnrollInCourse(int courseId)
        {
            if (!EnrolledCourseIds.Contains(courseId))
            {
                EnrolledCourseIds.Add(courseId);
                CourseProgress[courseId] = 0; // Initialize progress to 0%
                Console.WriteLine($"Enrolled in course with ID: {courseId}");
            }
            else
            {
                Console.WriteLine($"Already enrolled in course with ID: {courseId}");
            }
        }

        public void UpdateCourseProgress(int courseId, int progress)
        {
            if (EnrolledCourseIds.Contains(courseId))
            {
                CourseProgress[courseId] = progress;
                Console.WriteLine($"Updated progress for course ID {courseId} to {progress}%");
            }
            else
            {
                Console.WriteLine($"Not enrolled in course with ID: {courseId}");
            }
        }
        public virtual void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Enrolled Courses: " + string.Join(", ", EnrolledCourseIds));
            Console.WriteLine("Course Progress:");
            foreach (var entry in CourseProgress)
            {
                Console.WriteLine($"Course ID: {entry.Key}, Progress: {entry.Value}%");
            }
        }

        public void ShowEnrolledCourses(List<Course> courses, List<Enrollment> enrollments)
        {
            var myEnrollments = enrollments.Where(e => e.StudentUsername == this.Username).ToList();

            if (myEnrollments.Count == 0)
            {
                Console.WriteLine("No courses enrolled.");
                return;
            }

            foreach (var enrollment in myEnrollments)
            {
                Course course = courses.FirstOrDefault(c => c.CourseId == enrollment.CourseId);

                int daysSinceEnrollment = (DateTime.Now - enrollment.EnrollmentDate).Days;
                int progress = enrollment.ProgressPercentage;

                Console.WriteLine("\n------------------------------");
                Console.WriteLine($"Course Name   : {course.CourseName}");
                Console.WriteLine($"Category      : {course.Category}");
                Console.WriteLine($"Instructor    : {course.Instructor}");
                Console.WriteLine($"Course ID     : {course.CourseId}");
                Console.WriteLine($"Days Enrolled : {daysSinceEnrollment}");
                Console.WriteLine($"Progress      : {progress}%");

                if (daysSinceEnrollment > 30 && progress < 50)
                {
                    Console.WriteLine("WARNING: You are enrolled for over 30 days but your progress is below 50%");
                }
            }
        }


        public List<int> GetCompletedCourses()
        {
            List<int> completedCourses = new List<int>();
            foreach (var entry in CourseProgress)
            {
                if (entry.Value == 100)
                {
                    completedCourses.Add(entry.Key);
                }
            }
            return completedCourses;
        }

        public double GetAverageProgress()
        {
            if (CourseProgress.Count == 0)
            {
                return 0;
            }
            else
            {
                double totalProgress = CourseProgress.Values.Sum();
                return totalProgress / CourseProgress.Count;
            }
        }

        public void DropCourse(int courseId)
        {
            if (EnrolledCourseIds.Contains(courseId))
            {
                EnrolledCourseIds.Remove(courseId);
                CourseProgress.Remove(courseId);
                Console.WriteLine($"Dropped course with ID: {courseId}");
            }
            else
            {
                Console.WriteLine($"Not enrolled in course with ID: {courseId}");
            }
        }

    }
}
