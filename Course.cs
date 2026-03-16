using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    public abstract class Course : IEnrollable, ISearchable, IRatable
    {
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public int MaxStudents { get; set; }
        public int MinStudents { get; set; }
        public string Category { get; set; }
        public int CurrentEnrollment { get; set; }
        public List<string> Lessons { get; set; }
        public List<string> Quizzes { get; set; }
        public DateTime DateCreated { get; set; }
        public List<int> Ratings { get; set; } = new List<int>();
        public List<string> Reviews { get; set; } = new List<string>();

        protected Course(int courseId, string courseName, string description, string instructor, int maxStudents, int minStudents, string category)
        {
            CourseName = courseName;
            Description = description;
            Instructor = instructor;
            Lessons = new List<string>();
            Quizzes = new List<string>();
            DateCreated = DateTime.Now;
            MaxStudents = maxStudents;
            MinStudents = minStudents;
            CurrentEnrollment = 0;
            Category = category;
            CourseId = courseId;
        }


        public virtual void DisplayInfo()
        {
            Console.WriteLine($"CouseName : {CourseName}");
            Console.WriteLine($"Description : {Description}");
            Console.WriteLine($"Instructor : {Instructor}");
            Console.WriteLine($"Maximun Students : {MaxStudents}");
            Console.WriteLine($"Minimum Students : {MinStudents}");
            Console.WriteLine($"Category : {Category}");
            Console.WriteLine($"Number of students Enrolled : {CurrentEnrollment}");
            Console.WriteLine($"Course Created On : {DateCreated}");
        }

        public void IncrementEnrollement()
        {
            if (CurrentEnrollment < MaxStudents)
            {
                CurrentEnrollment++;

            }
        }

        public void DecrementEnrollement()
        {
            if (CurrentEnrollment > 0)
            {
                CurrentEnrollment--;
            }
        }

        public abstract void DisplayCourseInfo();
        public abstract string GetCourseType();
        public abstract int GetAvailableSeats();
        public abstract bool CanEnroll(Student student);

        public void Enroll(Student student)
        {
            if (!CanEnroll(student))
            {
                Console.WriteLine("Enrollment not allowed.");
                return;
            }

            CurrentEnrollment++;

            student.EnrollInCourse(CourseId);

            if (student is INotifiable notify)
            {
                notify.SendNotification($"You enrolled in {CourseName}");
            }

            Console.WriteLine("Enrollment successful.");
        }

        public void Drop(Student student)
        {
            if (CurrentEnrollment > 0)
            {
                CurrentEnrollment--;
                Console.WriteLine("Course dropped.");
            }
        }

        public bool MatchesSearch(string keyword)
        {
            keyword = keyword.ToLower();
            return CourseName.ToLower().Contains(keyword) || Description.ToLower().Contains(keyword) || Category.ToLower().Contains(keyword) || Instructor.ToLower().Contains(keyword);
        }

        public string GetSearchSummary()
        {
            return $"{CourseName} - Instructor : {Instructor} - {Category}";
        }

        public void AddRating(int stars, string review)
        {
            if (stars < 1 && stars > 5)
            {
                Console.WriteLine("Rating must be between 1 and 5 stars.");)
            }

            Ratings.Add(stars);
            Reviews.Add(review);
        }

        public void GetAverageRating()
        {
            if (Ratings.Count == 0)
            {
                Console.WriteLine("No ratings yet.");
                return;
            }
            double average = Ratings.Average();
            Console.WriteLine($"Average Rating: {average:F1} stars based on {Ratings.Count} ratings.");
        }
    }
}
