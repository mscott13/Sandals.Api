using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class User
    {
        public int userId { get; set; }
        public DateTime createdDate { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string firmName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string hash { get; set; }
        public string password { get; set; }
        public string userType { get; set; }
        public bool isVerified { get; set; }
        public bool recoveryState { get; set; }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public string ValidateRegistrationFields(User user)
        {
            string output = "";
            if (user.username == null || user.username == "")
            {
                output += "Field: username empty\n";
            }
            else if (user.username.Length < 5)
            {
                output += "Field: username must be at least 5 characters\n";
            }

            if (user.firstName == null || user.firstName == "")
            {
                output += "Field: firstName empty\n";
            }

            if (user.lastName == null || user.lastName == "")
            {
                output += "Field: lastName empty\n";
            }

            if (user.address == null || user.address == "")
            {
                output += "Field: address empty\n";
            }
            else if (user.address.Length < 5)
            {
                output += "Field: address must be at least 5 characters\n";
            }

            if (user.email == null || user.email == "")
            {
                output += "Field: email empty\n";
            }
            else if (!IsValidEmail(user.email))
            {
                output += "Field: email is an invalid format\n";
            }

            if (user.country == null || user.country == "")
            {
                output += "Field: country is empty\n";
            }

            if (user.phone == null || user.phone == "")
            {
                output += "Field: phone is empty\n";
            }
            else
            {
                //validate phone format here
            }

            if (user.password == null || user.password == "")
            {
                output += "Field: password is empty\n";
            }
            else if (user.password.Length < 6)
            {
                output += "Field: password must be at least 6 characters\n";
            }
            return output;
        }
    }
}
