using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public string StudentUsername { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }

        public Enrollment(int enrollmentId, string studentUsername, int courseId, int progressPercentage, string courseName)
        {
            EnrollmentId = enrollmentId;
            StudentUsername = studentUsername;
            CourseId = courseId;
            EnrollmentDate = DateTime.Now;
            ProgressPercentage = progressPercentage;
            IsCompleted = false;
            CourseName = courseName;
        }

        public void UpdateProgress(int newProgress)
        {
            if (newProgress >= 0 && newProgress <= 100)
            {
                ProgressPercentage = newProgress;
                if (ProgressPercentage == 100)
                {
                    IsCompleted = true;
                }
            }
            else
            {
                Console.WriteLine("Progress must be between 0 and 100.");
            }
        }

        public void MarkAsCompleted()
        {
            ProgressPercentage = 100;
            IsCompleted = true;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Enrollment ID: {EnrollmentId}");
            Console.WriteLine($"Student Username: {StudentUsername}");
            Console.WriteLine($"Course ID: {CourseId}");
            Console.WriteLine($"Enrollment Date: {EnrollmentDate}");
            Console.WriteLine($"Progress: {ProgressPercentage}%");
            Console.WriteLine($"Completed: {IsCompleted}");
        }


    }



}
