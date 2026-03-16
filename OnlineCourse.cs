using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    public class OnlineCourse : Course
    {
        public int VideoDuration { get; set; } // in minutes
        public List<string> VideoTitles { get; set; }
        public string StreamingUrl { get; set; }
        public OnlineCourse(int courseId, string courseName, string description, string instructor, int maxStudents, int minStudents, string category, int videoDuration) : base(courseId, courseName, description, instructor, maxStudents, minStudents, category)
        {
            VideoDuration = videoDuration;
            VideoTitles = new List<string>();
            StreamingUrl = "https://smartlearn.com/stream/" + CourseId;
        }

        public override bool CanEnroll(Student student)
        {
            return true;
        }

        public override int GetAvailableSeats()
        {
            return int.MaxValue; // Unlimited seats for online courses
        }

        public virtual void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Video Duration: {VideoDuration} minutes");
            Console.WriteLine($"Number of Videos: {VideoTitles.Count}");

        }

        public override void DisplayCourseInfo()
        {
            Console.Clear();
            Console.WriteLine(" ╔═════════════════════════════════════╗");
            Console.WriteLine($"║    ONLINE COURSE : {CourseName}     ║");
            Console.WriteLine(" ╚═════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine($"Course ID: {CourseId}");
            Console.WriteLine($"Instructor: {Instructor}");
            Console.WriteLine($"Category: {Category}");
            Console.WriteLine($"Duration: {VideoDuration} minutes ({VideoDuration / 60.0:F1} hours)");
            Console.WriteLine("Capacity: Unlimited");
            Console.WriteLine($"Current Enrollment: {CurrentEnrollment}");
            Console.WriteLine($"Average Rating : {(Ratings.Count > 0 ? Ratings.Average().ToString("F1") : "No ratings yet")} stars");
            Console.WriteLine($"Streaming URL: {StreamingUrl}");
            Console.WriteLine("Status: Open for Enrollment");
        }

        public override string GetCourseType()
        {
            return "Online";
        }
    }
}
