using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace _20104681JoshMkhariPOE.Models
{
    public class UserModel
    {
        [Required]
        public string User_Name { get; set; }
        [Required]
        public string User_Password { get; set; }
        [Required]
        public DateTime Start_Date { get; set; }
        [Required]
        public int Weeks { get; set; }

        public static List<string> usersList = new List<string>();

        static List<UserModel> userList = new List<UserModel>();

        public static Boolean loggedIn = false;
        public static Boolean signedIn = false;


        //check if in database
        public static Boolean checkLogin(string userName, string userPassword)
        {
            Boolean result = false;

            for (int i = 0; i < userList.Count; i++)
            {
                var currentName = userList.ElementAt(i);
                if (currentName.User_Name.Equals(userName))
                {
                    if (currentName.User_Password.Equals(encryptPass(userPassword)))
                    {
                        //Logged in
                        StartModel.semesterPeriodDone = true;
                        StartModel.semesterStartDate = currentName.Start_Date;
                        StartModel.semesterWeeks = currentName.Weeks;
                        DateTime endDay = StartModel.semesterStartDate;
                        DateTime answer = endDay.AddDays(StartModel.semesterWeeks * 7);
                        StartModel.semesterEndDate = answer;
                        result = true;
                        break;
                    }
                    else
                    {

                        result = false;
                    }
                }

            }
            return result;
        }

        public static void populateUsersList()
        {
            ProgramDAL progDal = new ProgramDAL();
            userList = progDal.GetAllUser().ToList();
        }

        public static Boolean SignUp(string username, string userPassword)
        {
            Boolean result = true;
            for (int i = 0; i < userList.Count; i++)
            {
                var currentName = userList.ElementAt(i);
                if (currentName.User_Name.Equals(username))
                {
                    Console.WriteLine("Name already in database");
                    result = false;
                    break;
                }

            }

            if (result)
            {
                StartModel.Users[0] = username;
                StartModel.Users[1] = encryptPass(userPassword);
                //store user name and password
                //proceed to saving weeks and date

            }
            return result;
        }

        static string encryptPass(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static void SaveDetails()
        {
            UserModel use = new UserModel();
            use.User_Name = StartModel.Users[0];
            use.User_Password = StartModel.Users[1];
            use.Start_Date = StartModel.semesterStartDate;
            use.Weeks = Convert.ToInt32(StartModel.semesterWeeks);

            ProgramDAL pal = new ProgramDAL();
            pal.AddUser(use);
            CalendarModel.addingToDatabase = true;
        }

        public static void Reset()
        {
            ProgramDAL dal = new ProgramDAL();
            dal.Reset();
        }
    }
}
