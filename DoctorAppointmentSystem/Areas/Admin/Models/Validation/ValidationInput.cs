using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.Validation
{
    public static class ValidationInput
    {
        private const string patternName = @"^[\p{L}\p{N}\s]*$"; 
        private const string patternMobile = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
        private const string patternUsername = @"^[a-zA-Z0-9-]*$";
        private const string patternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$";

        /// <summary>
        /// validation full name
        /// is repuired , max lenght 50, no special characters
        /// </summary>
        /// <param name="name"> field will validation </param>
        /// <param name="textName"> </param>
        /// <returns></returns>
        public static ValidationResult NameIsValid(string name, string textName )
        {
            if (String.IsNullOrEmpty(name))
            {
                return new ValidationResult() { Success = false, ErrorMessage = $"{textName} is required !"}; 
            }
            Match strname = Regex.Match(name, patternName, RegexOptions.IgnoreCase);
            if (!strname.Success)
            {
                return new ValidationResult() { Success = false, ErrorMessage = $"{textName} does not contain any special characters !"};
            }
            if (name.Length >= 50)
            {
                return new ValidationResult() { Success = false, ErrorMessage = $"{textName} charater max lenght is 50!" };
            }
            return new ValidationResult() {Success = true, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="textName"></param>
        /// <returns></returns>
        public static ValidationResult UserNameIsValid(string userName, string textName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                return new ValidationResult() { Success = false, ErrorMessage = $"{textName} is required !" };
            }
            Match strusername = Regex.Match(userName, patternUsername, RegexOptions.IgnoreCase);
            if (!strusername.Success)
            {
                return new ValidationResult() { Success = false, ErrorMessage = $"{textName} does not contain any special characters !" };
            }
            if (userName.Length >= 50 || userName.Length < 3)
            {
                return new ValidationResult() { Success = false, ErrorMessage = $"{textName} charater lenght is 3 to 50!" };
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="Gender"></param>
       /// <param name="sysParamGender"></param>
       /// <returns></returns>
        public static ValidationResult GenderIsValid(int Gender, List<SYSTEM_PARA> sysParamGender)
        {
            if (Gender <= 0)
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Gender not match!" };
            }
            else
            {
                var result = sysParamGender.Where(pr => pr.GROUPID == "Gender" && pr.ID == Gender).FirstOrDefault();
                if (result == null)
                {
                    return new ValidationResult() { Success = false, ErrorMessage = "Gender not match!" };
                }
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ValidationResult PasswordIsValid(string password)
        {
            if (String.IsNullOrEmpty(password))
            {

                return new ValidationResult() { Success = false, ErrorMessage = "Password is required!" };
            }

            Match strpawd = Regex.Match(password, patternPassword, RegexOptions.IgnoreCase);
            if (!strpawd.Success)
            {
                return new ValidationResult() { Success = false, ErrorMessage = @"Password charater at least one uppercase letter, one lowercase letter, one number and one special character: [a - z],[A - Z],[0 - 9],[@$!%*?&]" };
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="national"></param>
        /// <returns></returns>
        public static ValidationResult NationalIsValid(string national)
        {
            if (String.IsNullOrEmpty(national))
            {
                return new ValidationResult() { Success = false, ErrorMessage = "National ID is required!" };
            }
            if (national.Length >= 20)
            {
                return new ValidationResult() { Success = false, ErrorMessage = "National ID charater max lenght is 20!" };
            }

            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static ValidationResult DateOfBirthIsValid(DateTime date)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Date of birth smaller than current date !" };
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static ValidationResult MobileIsValid(string mobile)
        {
            if (!String.IsNullOrEmpty(mobile))
            {
                Match m = Regex.Match(mobile, patternMobile, RegexOptions.IgnoreCase);

                if (!m.Success) //mobile
                {
                    return new ValidationResult() { Success = false, ErrorMessage = $"Mobile number is required !" };
                }
            }
            else
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Mobile number is required !" };

            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static ValidationResult EmailIsValid(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Email is required!" };
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static ValidationResult AddressIsValid(string address)
        {
            if (String.IsNullOrEmpty(address))
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Address is required !" };
            }
            if (address.Length >= 265)
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Doctor address charater max lenght is 256 !" };
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speciality"></param>
        /// <returns></returns>
        public static ValidationResult SpecialityIsValid(string speciality)
        {
            if (String.IsNullOrEmpty(speciality))
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Specialy is required!" };
            }
            if (speciality.Length >= 265)
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Specialy charater max lenght is 256 !" };
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WorkingStartDate"></param>
        /// <param name="WorkingEndDate"></param>
        /// <returns></returns>
        public static ValidationResult WorkingIsValid(DateTime WorkingStartDate, DateTime WorkingEndDate)
        {
            if (WorkingStartDate >=  WorkingEndDate)
            {
                return new ValidationResult() { Success = false, ErrorMessage = "Working start date smaller than Working end date !" };
            }
            return new ValidationResult() { Success = true, ErrorMessage = " " };
        }
    }
}