using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _20104681JoshMkhariPOE.Models
{
    class ProgramDAL
    {
        string connectionStringDEV = "Data Source=DESKTOP-M2M40A2;Initial Catalog=20104681PROGTask2JoshMkhari;Integrated Security=True";

        //USER TABLE
        public IEnumerable<UserModel> GetAllUser()
        {
            List<UserModel> userList = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    UserModel use = new UserModel();
                    use.User_Name = Convert.ToString(dr["USERS_NAME"].ToString());
                    use.User_Password = Convert.ToString(dr["USER_PASSWORD"].ToString());
                    use.Start_Date = Convert.ToDateTime(dr["START_DATE"].ToString());
                    use.Weeks = Convert.ToInt32(dr["WEEKS"].ToString());

                    userList.Add(use);
                }

                con.Close();
            }

            return userList;
        }

        public void AddUser(UserModel use)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertUser", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@USERS_NAME", use.User_Name);
                cmd.Parameters.AddWithValue("@USER_PASSWORD", use.User_Password);
                cmd.Parameters.AddWithValue("@START_DATE", use.Start_Date);
                cmd.Parameters.AddWithValue("@WEEKS", use.Weeks);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //MODULES TABLES
        public void AddModule(String moduleCode, String moduleName, int credits, int hours, String userName)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertModule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MODULES_ID", moduleCode);
                cmd.Parameters.AddWithValue("@NAME", moduleName);
                cmd.Parameters.AddWithValue("@CREDITS", credits);
                cmd.Parameters.AddWithValue("@HOURS", hours);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertModuleList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MODULES_ID", moduleCode);
                cmd.Parameters.AddWithValue("@USERS_NAME", userName);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddModuleList(String moduleCode, String userName)
        {

            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertModuleList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MODULES_ID", moduleCode);
                cmd.Parameters.AddWithValue("@USERS_NAME", userName);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void GetModule(string UserName)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAModule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MODULES_ID", UserName);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ModuleAdderModel.ModuleCode = (dr["MODULES_ID"].ToString());
                    ModuleAdderModel.ModuleName = (dr["NAME"].ToString());
                    ModuleAdderModel.NumberOfCredits = Convert.ToInt32(dr["CREDITS"].ToString());
                    ModuleAdderModel.ClassHoursPerWeek = Convert.ToInt32(dr["HOURS"].ToString());
                }
                con.Close();
            }
        }

        public IEnumerable<ModuleAdderModel> GetAllModuleList()
        {
            List<ModuleAdderModel> modList = new List<ModuleAdderModel>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllModulesList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ModuleAdderModel mod = new ModuleAdderModel();
                    mod.modulleName = Convert.ToString(dr["MODULES_ID"].ToString());
                    mod.userName = Convert.ToString(dr["USERS_NAME"].ToString());
                    modList.Add(mod);
                }
                con.Close();
            }

            return modList;
        }

        //DATES STUFF
        public void AddDate(DateTime storeDate)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertStoredDates", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@STORED_DATE", storeDate);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddPlanned(int plannedID)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertPlanned", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANNED_ID", plannedID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public int GetPlannedID()
        {
            int planned = 0;
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetLastPlannedID", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ModuleAdderModel mod = new ModuleAdderModel();
                    planned = Convert.ToInt32(dr["PLANNED_ID"].ToString());
                }
                con.Close();
            }
            return planned;
        }

        public void AddDateToList(DateTime storeDate, String userName, int planned)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertDatesList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@STORED_DATE", storeDate);
                cmd.Parameters.AddWithValue("@USERS_NAME", userName);
                cmd.Parameters.AddWithValue("@PLANNED_ID", planned);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddPlannedList(int planned, string moduleID, int hours)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertPlannedList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HOURS", hours);
                cmd.Parameters.AddWithValue("@MODULES_ID", moduleID);
                cmd.Parameters.AddWithValue("@PLANNED_ID", planned);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //SP_UpdatePlannedList

        public void UpdatePlannedList(int planned, string moduleID, int hours)
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_UpdatePlannedList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HOURS", hours);
                cmd.Parameters.AddWithValue("@MODULES_ID", moduleID);
                cmd.Parameters.AddWithValue("@PLANNED_ID", planned);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public int GetCurrentPlanned(DateTime storeDate, String userName)
        {
            int planned = 0;
            List<ModuleAdderModel> modList = new List<ModuleAdderModel>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetPlannedFromDatesList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@STORED_DATE", storeDate);
                cmd.Parameters.AddWithValue("@USERS_NAME", userName);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ModuleAdderModel mod = new ModuleAdderModel();
                    planned = Convert.ToInt32(dr["PLANNED_ID"].ToString());
                }
                con.Close();
            }
            return planned;
        }

        public IEnumerable<CalendarModel> GetAllStoredDates(string username)
        {
            List<CalendarModel> datList = new List<CalendarModel>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllDatesList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERS_NAME", username);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CalendarModel mod = new CalendarModel();
                    mod.StoreDate = Convert.ToDateTime(dr["STORED_DATE"].ToString());
                    mod.plan = Convert.ToInt32(dr["PLANNED_ID"].ToString());
                    datList.Add(mod);
                }
                con.Close();
            }

            return datList;
        }

        public IList<DateTime> GetAllStoredDates()
        {
            List<DateTime> datList = new List<DateTime>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllStoreDates", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    datList.Add(Convert.ToDateTime(dr["STORED_DATE"].ToString()));
                }
                con.Close();
            }

            return datList;
        }

        public IEnumerable<CalendarModel> GetModuleHours(int planned)
        {
            List<CalendarModel> datList = new List<CalendarModel>();
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetModuleHours", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PLANNED_ID", planned);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CalendarModel mod = new CalendarModel();
                    mod.code = (dr["MODULES_ID"].ToString());
                    mod.hours = Convert.ToInt32(dr["HOURS"].ToString());
                    datList.Add(mod);
                }
                con.Close();
            }

            return datList;
        }

        public void Reset()
        {
            using (SqlConnection con = new SqlConnection(connectionStringDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_Reset", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                con.Close();
            }
        }
    }
}