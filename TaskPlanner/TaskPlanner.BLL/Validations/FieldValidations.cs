using System;
using System.Text.RegularExpressions;

namespace TaskPlanner.BLL.Validations
{
    public static class FieldValidations
    {
        public static bool IsCorrectName(string? name)
        {
            string regexCheck = @"^(?<firstchar>(?=[A-Za-z]))((?<alphachars>[A-Za-z])|(?<specialchars>[A-Za-z]['-](?=[A-Za-z]))|(?<spaces> (?=[A-Za-z])))*$";
            return name is not null && Regex.IsMatch(name, regexCheck);
        }

        public static bool IsCorrectUserName(string? username)
        {
            string regexCheck = @"^[a-zA-Z\d-_]+$";
            return username is not null && Regex.IsMatch(username, regexCheck);
        }

        public static bool IsCorrectPassword(string? password)
        {
            string regexCheck = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{6,}$";
            return password is not null && Regex.IsMatch(password, regexCheck);
        }

        public static bool IsCorrectDate(DateTime? time)
        {
            return time is not null && time.Value.Year >= 2000;
        }
    }
}
