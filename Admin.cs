using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    public class Admin : User
    {
        public string AdminLevel { get; set; }
        public bool CanManageUsers { get; set; }
        public bool CanManageCourses { get; set; }

        public Admin(string username, string password, string email, string adminLevel) : base(username, password, email)
        {
            CanManageUsers = true;
            CanManageCourses = true;
            AdminLevel = adminLevel;
        }

        public override void DisplayDashboard()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════╗");
            Console.WriteLine("║         ADMIN DASHBOARD        ║");
            Console.WriteLine("╚════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine($"Welcome, Admin {Username}!");
            Console.WriteLine($"Level: {AdminLevel}");
            Console.WriteLine();

            Console.WriteLine("Admin Dashboard");
            Console.WriteLine("1. View All Users");
            Console.WriteLine("2. Deactivate User");
            Console.WriteLine("3. Reports");
            Console.WriteLine("4. Settings");
            Console.WriteLine("5. Logout");

        }

        public override string GetUserType()
        {
            return "Admin";
        }

        public virtual void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Admin Level: {AdminLevel}");
            Console.WriteLine($"Can Manage Users: {CanManageUsers}");
            Console.WriteLine($"Can Manage Courses: {CanManageCourses}");
        }
        public void DisplayPermissions()
        {
            Console.WriteLine("Admin Permissions:");
            Console.WriteLine($"- Manage Users: {CanManageUsers}");
            Console.WriteLine($"- Manage Courses: {CanManageCourses}");
        }
        public void ViewAllUsers(List<User> users)
        {
            Console.WriteLine("All Registered Users:");
            foreach (var user in users)
            {
                user.DisplayInfo();
                Console.WriteLine("--------------------");
            }
        }
        public void DeactivateUserAccount(User user)
        {
            user.DeactivateAccount();
            Console.WriteLine($"Deactivated account for user: {user.Username}");
        }
        public void GetSystemStats(List<User> users, List<Course> courses, List<Enrollment> enrollments)
        {
            Console.WriteLine("\n===== SYSTEM STATISTICS =====");

            // Students with 100% completion
            var completedStudents = enrollments.Where(e => e.ProgressPercentage == 100).Select(e => e.StudentUsername).Distinct().Count();

            Console.WriteLine($"Students with 100% completion rate: {completedStudents}");

            // Most popular course
            var mostPopularCourse = courses.OrderByDescending(c => enrollments.Count(e => e.CourseId == c.CourseId)).FirstOrDefault();

            if (mostPopularCourse != null)
            {
                int count = enrollments.Count(e => e.CourseId == mostPopularCourse.CourseId);
                Console.WriteLine($"Most Popular Course: {mostPopularCourse.CourseName} ({count} students)");
            }

            // Least popular course
            var leastPopularCourse = courses.OrderBy(c => enrollments.Count(e => e.CourseId == c.CourseId)).FirstOrDefault();

            if (leastPopularCourse != null)
            {
                int count = enrollments.Count(e => e.CourseId == leastPopularCourse.CourseId);
                Console.WriteLine($"Least Popular Course: {leastPopularCourse.CourseName} ({count} students)");
            }

            // Inactive users
            int inactiveUsers = users.Count(u => !u.IsActive);
            Console.WriteLine($"Inactive Users: {inactiveUsers}");
        }

    }
}
