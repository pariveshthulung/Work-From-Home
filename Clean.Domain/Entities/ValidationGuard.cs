using System.Text.RegularExpressions;
using Clean.Domain.Exceptions;
using WorkFromHome.Domain.Exceptions;

namespace Clean.Domain.Entities
{
    class ValidationGuard
    {
        public static string ValidateString(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} can't be null or empty");
            else
                return value;
        }

        public static int ValidateIntPositive(int value, string paramName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException($"{paramName} can not be negative.");
            return value;
        }

        public static int ValidateIntPositiveOrGreaterThanZero(int value, string paramName)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException($"{paramName} can not be less than 1.");
            return value;
        }

        public static string ValidatePhoneNumber(string phoneNumber)
        {
            if (Regex.Match(phoneNumber, @"([0-9]{10})$").Success)
            {
                return phoneNumber;
            }

            throw new InvalidPhoneNoException("Phone number should contain 10 numbers");
        }

        public static string ValidateEmail(string email)
        {
            try
            {
                var newEmail = IsValidateEmail(email);
                return newEmail;
            }
            catch (InvalidEmailException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string IsValidateEmail(string email)
        {
            bool isEmail = Regex.IsMatch(
                email,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase
            );

            if (!isEmail)
                throw new InvalidEmailException(
                    "Invalid email should meet 'johnchena16@gmail.com' format!!!"
                );
            return email;
        }

        public static DateTime ValidateFromDate(DateTime dateTime, string paramName)
        {
            if (dateTime < DateTime.Now.Date)
                throw new DateTimeException($"{paramName} can't be less than current Date ");
            return dateTime;
        }

        public static DateTime ValidateToDate(DateTime toDate, DateTime fromDate, string paramName)
        {
            if (toDate < fromDate)
                throw new DateTimeException($"{paramName} cant be less than FromDate");
            return toDate;
        }

        public static DateTime ValidateFromDate(string dateTime, string paramName)
        {
            DateTime userFromdate;
            bool sucess = DateTime.TryParse(dateTime, out userFromdate);
            if (!sucess)
                throw new DateTimeException("Invalid date format should meet(yyyy-mm-dd)");
            if (userFromdate < DateTime.Now.AddMinutes(1))
                throw new DateTimeException($"{paramName} can't be less than current Date ");
            return userFromdate;
        }

        public static DateTime ValidateToDate(string toDate, DateTime fromDate, string paramName)
        {
            DateTime userTodate;
            bool sucess = DateTime.TryParse(toDate, out userTodate);
            if (!sucess)
                throw new DateTimeException("Invalid date format should meet(yyyy-mm-dd)");
            if (userTodate < fromDate)
                throw new DateTimeException($"{paramName} cant be less than FromDate");
            return userTodate;
        }
    }
}
