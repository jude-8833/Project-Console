using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    public interface IEnrollable
    {
        void Enroll(Student student);
        void Drop(Student student);
        bool CanEroll(Student student);
        int GetAvailableSeats();
    }
}
