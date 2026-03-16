using SmartLearn.Console;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    // ================= SESSION STATE =================
    static bool running = true;
    static User currentUser = null;

    // ================= USER STORAGE =================
    static List<User> users = [];
    static List<Course> courses = [];
    static List<Enrollment> enrollments = [];

    static void LoadSampleCourses()
    {
        courses.Clear();
        courses.Add(new Course(101, "C# Programming Fundamentals", "Learn C# from scratch", "Prof. Smith", 30, 0, "Programming"));
        courses.Add(new Course(102, "Advanced C# Techniques", "Master advanced C# concepts", "Prof. Johnson", 25, 0, "Programming"));
        courses.Add(new Course(103, "Introduction to SQL Server", "Database fundamentals", "Prof. Williams", 30, 0, "Database"));
        courses.Add(new Course(104, "Web Development with ASP.NET", "Build web applications", "Prof. Brown", 20, 0, "Web Development"));
        courses.Add(new Course(105, "Entity Framework Core", "ORM for .NET", "Prof. Davis", 25, 0, "Database"));
    }

    static void Main()
    {
        LoadSampleCourses();

        while (running)
        {
            if (currentUser == null)
            {
                Console.Clear();
                Console.WriteLine("Welcome to SmartLearn Console Application!");
                Console.WriteLine("[1] Login");
                Console.WriteLine("[2] Register");
                Console.WriteLine("[3] Exit");

                switch (Console.ReadLine())
                {
                    case "1":
                        currentUser = LoginUser(users);
                        break;

                    case "2":
                        RegisterNewUser(users);
                        break;

                    case "3":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        return;
                }
            }

            else
            {
                currentUser.DisplayDashboard();

                string choice = Console.ReadLine();

                if (!(choice == "1" || choice == "2" || choice == "3"))
                {
                    currentUser = null;
                }
            }
        }
    }

    // ================= REGISTRATION =================
    static void RegisterNewUser(List<User> users)
    {
        Console.Clear();
        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        if (!ValidateUsername(username))
        {
            Console.WriteLine("Invalid username.");
            Pause();
            return;
        }


        bool exists = users.Any(u => u.Username == username);

        if (exists)
        {
            Console.WriteLine("Username already exists.");
            Pause();
            return;
        }

        Console.Write("Password: ");
        string password = Console.ReadLine()!;
        if (!IsPasswordStrong(password))
        {
            Console.WriteLine("Invalid password.");
            Pause();
            return;
        }

        Console.Write("Email: ");
        string email = Console.ReadLine()!;
        if (!ValidateEmail(email))
        {
            Console.WriteLine("Invalid email.");
            Pause();
            return;
        }

        Console.Write("Role (Student / Instructor / Admin): ");
        string role = Console.ReadLine()!;
        if (role != "Student" && role != "Instructor" && role != "Admin")
        {
            Console.WriteLine("Invalid role.");
            Pause();
            return;
        }

        // Create and store the new user
        User newUser;

        if (role == "Student")
        {
            newUser = new Student(username, password, email);
        }
        else if (role == "Instructor")
        {
            Console.Write("Department: ");
            string department = Console.ReadLine()!;

            newUser = new Instructor(username, password, email, department);
        }
        else
        {
            Console.Write("Admin Level (Super / Standard): ");
            string level = Console.ReadLine()!;

            newUser = new Admin(username, password, email, level);

        }

        users.Add(newUser);

        Console.WriteLine("Registration successful!");
        Pause();
    }

    // ================= LOGIN =================
    static User LoginUser(List<User> users)
    {
        Console.Clear();
        Console.Write("Username: ");
        string username = Console.ReadLine()!;

        //Search for the user
        User foundUsername = users.FirstOrDefault(u => u.Username == username);

        if (foundUsername == null)
        {
            Console.WriteLine("User not found.");
            Pause();
            return null;
        }

        //Password validation
        Console.Write("Password: ");
        string password = Console.ReadLine()!;

        // Check password
        if (!foundUsername.ValidatePassword(password))
        {
            Console.WriteLine("Invalid password. ");
            Pause();
            return null;
        }

        if (!foundUsername.IsActive)
        {
            Console.WriteLine("Account is deactivated. Contact admin.");
            Pause();
            return null;
        }

        //Session setup and welcome message
        Console.WriteLine($"Welcome {foundUsername.Username} ({foundUsername.GetUserType()})!");
        Pause();

        return foundUsername;
    }

    // ================= STUDENT DASHBOARD =================
    static void ShowStudentDashboard(Student student)
    {
        while (true)
        {
            student.DisplayDashboard();

            switch (Console.ReadLine())
            {
                case "1":
                    BrowseCourses();
                    break;
                case "2":
                    EnrollStudentInCourse(student);
                    break;
                case "3":
                    ShowEnrollments();
                    break;
                case "4":
                    UpdateStudentProgress(student);
                    break;
                case "5":
                    DropStudentCourse(student);
                    break;
                case "6":
                    ShowStudentStats(student);
                    break;
                case "7":
                    Logout();
                    return;
                default:
                    ComingSoon();
                    break;
            }
        }
    }

    // ================= COURSE BROWSING =================
    static void BrowseCourses()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. View All Courses");
            Console.WriteLine("2. Search Courses");
            Console.WriteLine("3. Back");

            switch (Console.ReadLine())
            {
                case "1":
                    DisplayCourses();
                    break;
                case "2":
                    SearchCourses();
                    break;
                case "3":
                    return;
                default:
                    InvalidOption();
                    break;
            }
        }
    }

    static void DisplayCourses()
    {
        Console.Clear();
        if (courses.Count == 0)
        {
            Console.WriteLine("No courses available.");
            Pause();
            return;
        }
        for (int i = 0; i < courses.Count; i++)
        {
            Course c = courses[i];
            string status = c.CanEnroll() ? "Open" : "Full";

            Console.WriteLine($"{i + 1}. {c.CourseName} | {c.Category} | {c.CurrentEnrollment}/{c.MaxStudents} | {status}"); ;
        }
        Pause();
    }

    static void SearchCourses()
    {
        Console.Clear();
        Console.Write("Keyword: ");
        string keyword = Console.ReadLine()!.ToLower();

        bool found = false;

        foreach (Course c in courses)
        {
            if (c.CourseName.ToLower().Contains(keyword) || c.Category.ToLower().Contains(keyword))
            {
                string status = c.CanEnroll() ? "Open" : "Full";
                Console.WriteLine($"{c.CourseName} | {c.Category} | {c.CurrentEnrollment}/{c.MaxStudents} | {status}");
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No results found.");

        Pause();
    }

    static void EnrollStudentInCourse(Student student)
    {
        Console.Clear();
        Console.WriteLine("\n=== AVAILABLE COURSES ===");
        // Display all courses 
        foreach (Course course in courses)
        {
            Console.WriteLine($"\n[{course.CourseId}] {course.CourseName}");
            Console.WriteLine($"Category: {course.Category}");
            Console.WriteLine($"Instructor: {course.Instructor}");
            Console.WriteLine($"Enrollment: {course.CurrentEnrollment}/{course.MaxStudents}");
            Console.WriteLine($"Available: {(course.CanEnroll() ? "✓ Yes" : "✗ Full")}");
            Console.WriteLine("---");
        }

        Console.Write("\nEnter Course ID to enroll: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine(" Invalid course ID.");
            Pause();
            return;
        }

        // Find the course 
        Course selectedCourse = courses.Find(c => c.CourseId == courseId);
        if (selectedCourse == null)
        {
            Console.WriteLine("Course not found.");
            Pause();
            return;
        }

        // Check if course can accept enrollments 
        if (!selectedCourse.CanEnroll())
        {
            Console.WriteLine("Course is full!");
            Pause();
            return;
        }

        // Check if student already enrolled 
        if (student.EnrolledCourseIds.Contains(courseId))
        {
            Console.WriteLine("Already enrolled in this course!");
            Pause();
            return;
        }

        // Enroll student 
        student.EnrollInCourse(courseId);

        // Update course enrollment count 
        selectedCourse.IncrementEnrollement();

        // Create enrollment record 
        int enrollmentId = enrollments.Count + 1;
        Enrollment newEnrollment = new Enrollment(enrollmentId, student.Username, courseId, 0, selectedCourse.CourseName);

        enrollments.Add(newEnrollment);

        Console.WriteLine($"✓ Successfully enrolled in '{selectedCourse.CourseName}'!");
        Pause();
    }

    static void ShowEnrollments()
    {
        Console.Clear();
        Console.WriteLine("My Enrollments:");
        var myEnrollments = enrollments.Where(e => e.StudentUsername == currentUser.Username).ToList();
        if (myEnrollments.Count == 0)
        {
            Console.WriteLine("No enrollments found.");
            Pause();
            return;
        }
        foreach (var enrollment in myEnrollments)
        {
            Console.WriteLine($"- {enrollment.CourseName}");
        }
        Pause();
    }

    static void UpdateStudentProgress(Student student)
    {
        student.ShowEnrolledCourses(courses, enrollments);

        Console.Write("\nEnter Course ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Invalid course ID.");
            Pause();
            return;
        }

        Console.Write("Enter progress percentage (0-100): ");
        if (!int.TryParse(Console.ReadLine(), out int progress) || progress < 0 || progress > 100)
        {
            Console.WriteLine("Invalid progress value.");
            Pause();
            return;
        }
        // Update student's progress
        student.UpdateCourseProgress(courseId, progress);

        // Find and update enrollment record 
        Enrollment enrollment = enrollments.Find(e => e.StudentUsername == student.Username && e.CourseId == courseId);

        if (enrollment != null)
        {
            enrollment.UpdateProgress(progress);
            Console.WriteLine("✓ Progress updated!");
        }
        Pause();
    }

    static void DropStudentCourse(Student student)
    {

        Console.Clear();
        var myEnrollments = enrollments.Where(e => e.StudentUsername == currentUser.Username).ToList();
        if (myEnrollments.Count == 0)
        {
            Console.WriteLine("No courses to drop.");
            Pause();
            return;
        }

        student.ShowEnrolledCourses(courses, enrollments);

        Console.Write("\nEnter Course ID to drop: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine(" Invalid course ID.");
            Pause();
            return;
        }

        // Drop from student 
        student.DropCourse(courseId);

        // Find the course and decrement enrollment 
        Course course = courses.Find(c => c.CourseId == courseId);
        if (course != null)
        {
            course.DecrementEnrollement();
        }

        // Remove enrollment record 
        Enrollment enrollment = enrollments.Find(e => e.StudentUsername == student.Username && e.CourseId == courseId);

        if (enrollment != null)
        {
            enrollments.Remove(enrollment);
        }
    }

    static void ShowStudentStats(Student student)
    {
        Console.WriteLine("\n=== YOUR STATISTICS ===");
        Console.WriteLine($"Username: {student.Username}");
        Console.WriteLine($"Total Courses Enrolled: {student.EnrolledCourseIds.Count}");
        Console.WriteLine($"Completed Courses: {student.GetCompletedCourses().Count}");
        Console.WriteLine($"Average Progress: {student.GetAverageProgress():F2}% ");

        var completed = student.GetCompletedCourses();
        if (completed.Count > 0)
        {
            Console.WriteLine("\nCompleted Courses:");
            foreach (int courseId in completed)
            {
                Course course = courses.Find(c => c.CourseId == courseId);
                if (course != null)
                {
                    Console.WriteLine($"  ✓ {course.CourseName}");
                }
            }
        }
        Pause();
    }


    // ================= INSTRUCTOR DASHBOARD =================
    static void ShowInstructorDashboard(Instructor instructor)
    {
        while (true)
        {
            instructor.DisplayDashboard();

            switch (Console.ReadLine())
            {
                case "1":
                    ShowInstructorCourses(instructor);
                    break;
                case "2":
                    AddInstructorCourse(instructor);
                    break;
                case "3":
                    ShowInstructorStudentCount(instructor);
                    break;
                case "5":
                    Logout();
                    break;
                default:
                    ComingSoon();
                    break;
            }
        }
    }

    // ================= INSTRUCTOR COURSE MANAGMENT =================

    static void ShowInstructorCourses(Instructor instructor)
    {
        Console.Clear();
        Console.WriteLine("My Courses:");
        var myCourses = courses.Where(c => c.Instructor == instructor.Username).ToList();
        if (myCourses.Count == 0)
        {
            Console.WriteLine("No courses found.");
            Pause();
            return;
        }
        foreach (var course in myCourses)
        {
            Console.WriteLine($"- {course.CourseName} | {course.Category} | Enrolled: {course.CurrentEnrollment}/{course.MaxStudents}");
        }
        Pause();
    }

    static void AddInstructorCourse(Instructor instructor)
    {
        //DisplayAllCourses();

        Console.Clear();

        Console.WriteLine("Enter Course Title: ");
        string title = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Course title cannot be empty.");
            Pause();
            return;
        }

        bool exists = courses.Any(c => c.Instructor == instructor.Username && c.CourseName.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            Console.WriteLine("A course with this name already exists..");
            Pause();
            return;
        }

        Console.WriteLine("Enter Course ID:");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Invalid course ID.");
            Pause();
            return;
        }

        if (courses.Any(c => c.CourseId == courseId))
        {
            Console.WriteLine("Course ID already exists.");
            Pause();
            return;
        }

        Console.WriteLine("Enter Course Category:");
        string category = Console.ReadLine()!;

        Console.WriteLine("Enter Course Description:");
        string description = Console.ReadLine()!;

        Console.WriteLine("Enter Max Students:");
        if (!int.TryParse(Console.ReadLine(), out int maxStudents) || maxStudents <= 0)
        {
            Console.WriteLine("Invalid number for max students.");
            Pause();
            return;
        }

        Course newCourse = new Course(courseId, title, description, instructor.Username, maxStudents, 0, category);

        courses.Add(newCourse);
        instructor.AddCourse(courseId);

        Console.WriteLine($"✓ Course '{title}' with Course ID '{courseId}' created successfully!");

        Pause();

        //Console.Write("\nEnter Course ID to add: ");
        //if (!int.TryParse(Console.ReadLine(), out int courseId))
        //{
        //    Console.WriteLine("  Invalid course ID.");
        //    Pause();
        //    return;
        //}

        //Course course = courses.Find(c => c.CourseId == courseId);
        //if (course == null)
        //{
        //    Console.WriteLine("  Course not found.");
        //    Pause();
        //    return;
        //}

        //instructor.AddCourse(courseId);
    }

    static void ShowInstructorStudentCount(Instructor instructor)
    {
        Console.Clear();
        int totalStudents = 0;

        foreach (int courseId in instructor.CourseIds)
        {
            totalStudents += enrollments.Count(e => e.CourseId == courseId);
        }

        Console.WriteLine($"\nTotal students in your courses: {totalStudents}");
        Pause();

        //int count = instructor.GetStudentCount(enrollments);
        //Console.WriteLine($"\nTotal students in your courses: {count}");
    }

    static void DisplayAllCourses()
    {
        Console.WriteLine("\n=== ALL COURSES ===");
        foreach (Course course in courses)
        {
            Console.WriteLine($"\n[{course.CourseId}] {course.CourseName}");
            Console.WriteLine($"Category: {course.Category}");
            Console.WriteLine("---");
        }
    }

    // ================= ADMIN DASHBOARD =================
    static void ShowAdminDashboard(Admin admin)
    {
        while (true)
        {
            admin.DisplayDashboard();

            switch (Console.ReadLine())
            {
                case "1":
                    ViewAllUsers(admin);
                    break;
                case "2":
                    DeactivateUserAsAdmin(admin);
                    break;
                case "5":
                    Logout();
                    return;
                default:
                    ComingSoon();
                    break;
            }
        }
    }

    //================= ADMIN USER MANAGEMENT =================

    static void ViewAllUsers(Admin admin)
    {
        Console.Clear();
        Console.WriteLine("All Registered Users:");
        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            Pause();
            return;
        }
        foreach (User u in users)
        {
            Console.WriteLine($"- {u.Username} | {u.Email} | {u.GetUserType()} | Status : {u.IsActive}");
        }
        Pause();
    }

    static void DeactivateUserAsAdmin(Admin admin)
    {
        admin.ViewAllUsers(users);
        Console.Write("\nEnter username to deactivate: ");
        string username = Console.ReadLine();

        User userToDeactivate = users.Find(u => u.Username == username);

        if (userToDeactivate == null)
        {
            Console.WriteLine(" User not found.");
            Pause();
            return;
        }

        if (!userToDeactivate.IsActive)
        {
            Console.WriteLine("Account is deactivated.");
            Pause();
            return;
        }

        if (userToDeactivate == currentUser)
        {
            Console.WriteLine("You cannot deactivate yourself!");
            Pause();
            return;
        }

        admin.DeactivateUserAccount(userToDeactivate);
        Pause();
    }

    // ================= LOGOUT =================
    static void Logout()
    {
        running = false;
        currentUser = null;

        Console.WriteLine("Logged out successfully.");
        Pause();
    }

    // ================= VALIDATION =================
    static bool ValidateUsername(string username) =>
        username.Length >= 4;

    static bool IsPasswordStrong(string password) =>
        password.Length >= 6;

    static bool ValidateEmail(string email) =>
        Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    // ================= HELPERS =================
    static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static void ComingSoon()
    {
        Console.WriteLine("Feature coming in Week 3.");
        Pause();
    }

    static void InvalidOption()
    {
        Console.WriteLine("Invalid option.");
        Pause();
    }

}
