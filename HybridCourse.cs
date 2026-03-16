using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    public class HybridCourse : Course
    {
        public int MaxStudents { get; set; }
        public int OnlineVideoDuration { get; set; } // in minutes
        public List<string> VideoTitles { get; set; }
        public string RoomNumber { get; set; }
        public string Building { get; set; }
        public DateTime Schedule { get; set; }
        public List<DateTime> InPersonSessions { get; set; } = new List<DateTime>();
        public HybridCourse(int courseId, string courseName, string description, string instructor, int maxStudents, int minStudents, string category, int videoDuration, string roomNumber, string building, DateTime schedule) : base(courseId, courseName, description, instructor, maxStudents, minStudents, category)
        {
            OnlineVideoDuration = videoDuration;
            VideoTitles = new List<string>();
            RoomNumber = roomNumber;
            Building = building;
            Schedule = schedule;
            MaxStudents = maxStudents;
        }
        public override bool CanEnroll(Student student)
        {
            return CurrentEnrollment < MaxStudents;
        }

        public override int GetAvailableSeats()
        {
            return MaxStudents - CurrentEnrollment;
        }
        public virtual void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Video Duration: {OnlineVideoDuration} minutes");
            Console.WriteLine($"Number of Videos: {VideoTitles.Count}");
            Console.WriteLine($"Location: {Building} Room {RoomNumber}");
            Console.WriteLine($"Schedule: {Schedule:dddd, MMMM dd, yyyy h:mm tt}");
        }
        public override void DisplayCourseInfo()
        {
            Console.Clear();
            Console.WriteLine(" ╔═════════════════════════════════════╗");
            Console.WriteLine($"║    HYBRID COURSE : {CourseName}     ║");
            Console.WriteLine(" ╚═════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"Course ID: {CourseId}");
            Console.WriteLine($"Instructor: {Instructor}");
            Console.WriteLine($"Duration: {OnlineVideoDuration} minutes ({OnlineVideoDuration / 60.0:F1} hours)");
            Console.WriteLine($"Location: {Building} Room {RoomNumber}");
            Console.WriteLine($"Schedule: {Schedule:dddd, MMMM dd, yyyy h:mm tt}");
            Console.WriteLine($"Average Rating : {(Ratings.Count > 0 ? Ratings.Average().ToString("F1") : "No ratings yet")} stars");
            Console.WriteLine($"Seats Remaining: {GetAvailableSeats()}");
            Console.WriteLine(CurrentEnrollment < MaxStudents ? "Status: Open for Enrollment" : "Status: Full");
        }
        public override string GetCourseType()
        {
            return "Hybrid";
        }
    }
}
