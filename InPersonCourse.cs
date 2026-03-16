using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    public class InPersonCourse : Course
    {
        public int MaxStudents { get; set; }
        public string RoomNumber { get; set; }
        public string Building { get; set; }
        public DateTime Schedule { get; set; }
        protected InPersonCourse(int courseId, string courseName, string description, string instructor, int maxStudents, int minStudents, string category, string roomNumber, string building, DateTime schedule) : base(courseId, courseName, description, instructor, maxStudents, minStudents, category)
        {
            RoomNumber = roomNumber;
            Building = building;
            Schedule = schedule;
            MaxStudents = maxStudents;
        }

        public virtual void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Location: {Building} Room {RoomNumber}");
            Console.WriteLine($"Schedule: {Schedule:dddd, MMMM dd, yyyy h:mm tt}");
        }

        public override bool CanEnroll(Student student)
        {
            return CurrentEnrollment < MaxStudents;
        }

        public override int GetAvailableSeats()
        {
            return MaxStudents - CurrentEnrollment;
        }
        public override void DisplayCourseInfo()
        {
            Console.Clear();
            Console.WriteLine(" ╔═════════════════════════════════════╗");
            Console.WriteLine($"║    IN-PERSON COURSE : {CourseName}  ║");
            Console.WriteLine(" ╚═════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"Course ID: {CourseId}");
            Console.WriteLine($"Instructor: {Instructor}");
            Console.WriteLine($"Location: {Building} Room {RoomNumber}");
            Console.WriteLine($"Schedule: {Schedule:dddd, MMMM dd, yyyy h:mm tt}");
            Console.WriteLine($"Capacity: {CurrentEnrollment}/{MaxStudents}");
            Console.WriteLine($"Seats Remaining: {GetAvailableSeats()}");
            Console.WriteLine($"Average Rating : {(Ratings.Count > 0 ? Ratings.Average().ToString("F1") : "No ratings yet")} stars");
            Console.WriteLine(CurrentEnrollment < MaxStudents ? "Status: Open for Enrollment" : "Status: Full");
        }

        public override string GetCourseType()
        {
            return "In-Person";
        }
    }
}
