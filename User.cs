using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearn.Console
{
    using System;

    public abstract class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateRegistered { get; set; }

        // Constructor must be protected
        protected User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
            IsActive = true;
            DateRegistered = DateTime.Now;
        }

        // Regular method
        public bool ValidatePassword(string inputPassword)
        {
            return Password == inputPassword;
        }

        // Virtual method
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Username: {Username}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Registered On: {DateRegistered}");
            Console.WriteLine($"Active: {IsActive}");
        }

        // Abstract methods
        public abstract void DisplayDashboard();

        public abstract string GetUserType();

        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
            Console.WriteLine("Password updated successfully.");
        }

        public void DeactivateAccount()
        {
            IsActive = false;
            Console.WriteLine("Account deactivated.");
        }
    }
}
