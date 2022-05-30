using _20104681JoshMkhariPOE.Logic;
using _20104681JoshMkhariPOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Windows;

namespace _20104681JoshMkhariPOE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            //Clearing all stored data within app
            StartModel.Reset();
            UserModel.loggedIn = false;
            UserModel.signedIn = false;
            ModuleAdderModel.clearData();
            Program.ResetModuleList();
            CalendarModel.datesList.Clear();
            CalendarModel.usedDatesList.Clear();

            UserModel.populateUsersList();
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult Login(FormCollection formData)
        {
            String userName = formData["Username"] == "" ? null : formData["Username"];
            String userPassword = formData["Password"] == "" ? null : formData["Password"];

            if (UserModel.checkLogin(userName, userPassword))//If the user is attempting to login
            {
                StartModel.Users[0] = userName;
                ModuleAdderModel mad = new ModuleAdderModel();
                Thread retModules = new Thread(new ThreadStart(mad.retrieveModules));
                retModules.Start();
                CalendarModel cm = new CalendarModel();
                Thread popDates = new Thread(new ThreadStart(cm.populateFromDatabase));
                popDates.Start();
                UserModel.loggedIn = true;
                Thread.Sleep(400);
                return RedirectToAction("ViewModules");//go to dashboard with all their stuff //redirect to action dashboard
            }
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            UserModel.populateUsersList();
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult Register(FormCollection formData)
        {
            String userName = formData["Username"] == "" ? null : formData["Username"];
            String userPassword = formData["Password"] == "" ? null : formData["Password"];
            if (UserModel.SignUp(userName, userPassword))
            {
                UserModel.loggedIn = true;
                UserModel.signedIn = true;
                StartModel sm = new StartModel();
                sm.signInName = userName;
                return RedirectToAction("SetSemester");
            }
            else
            {
                return RedirectToAction("Register");
            }

        }

        public ActionResult AddModule()
        {
            if (!UserModel.loggedIn)
            {
                return RedirectToAction("Login");
            }
            UserModel um = new UserModel();
            um.User_Name = StartModel.Users[0];
            return View(um);
        }

        [HttpPost]
        public ActionResult AddModule(FormCollection formData)
        {
            String ModuleCode = formData["moduleCode"] == "" ? null : formData["moduleCode"];
            String ModuleName = formData["moduleName"] == "" ? null : formData["moduleName"];
            int NumCredits = Convert.ToInt32(formData["moduleCredits"] == "" ? null : formData["moduleCredits"]);
            int NumHours = Convert.ToInt32(formData["moduleHours"] == "" ? null : formData["moduleHours"]);
            Boolean codePass = false;
            Boolean namePass = false;
            if (Program.ValidateInput(1, ModuleCode))//Check if the ModuleCode entered is valid
            {
                codePass = true;
            }

            if (Program.ValidateInput(2, ModuleName))//Check if the ModuleName entered is valid
            {
                namePass = true;
            }
            if (namePass && codePass)//if all are true
            {
                ModuleAdderModel.ModuleCode = ModuleCode;
                ModuleAdderModel.ModuleName = ModuleName;
                ModuleAdderModel.NumberOfCredits = NumCredits;
                ModuleAdderModel.ClassHoursPerWeek = NumHours;
                //Check if the module is valid
                if (ModuleAdderModel.populateModule())
                {
                    return RedirectToAction("ViewModules");
                }
            }
            return RedirectToAction("AddModule");
        }

        public ActionResult SetSemester()
        {
            if (!UserModel.loggedIn)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SetSemester(FormCollection formData)
        {
            int numWeeks = Convert.ToInt32(formData["semRange"] == "" ? null : formData["semRange"]);
            String date = formData["dateValue"] == "" ? null : formData["dateValue"];
            if (String.IsNullOrEmpty(date))
            {
                return RedirectToAction("SetSemester");
            }
            String convertedDate = date.Substring(0, 4) + "/" + date.Substring(5, 2) + "/" + date.Substring(8, 2);

            DateTime endDay;

            StartModel.semesterStartDate = Convert.ToDateTime(convertedDate);
            endDay = StartModel.semesterStartDate;

            StartModel.semesterWeeks = Convert.ToInt32(numWeeks);

            //Calculate the semester end day
            DateTime answer = endDay.AddDays(numWeeks * 7);
            StartModel.semesterEndDate = answer;
            StartModel.semesterPeriodDone = true;//alert mainwindow that the user has completed the first view
            CalendarModel cm = new CalendarModel();
            Thread popDates = new Thread(new ThreadStart(cm.populateFromDatabase));
            popDates.Start();

            UserModel.SaveDetails();
            ModuleAdderModel mad = new ModuleAdderModel();
            mad.retrieveModules();
            UserModel.signedIn = false;

            return RedirectToAction("AddModule");

        }

        public ActionResult ViewStudy()
        {
            if (!UserModel.loggedIn)
            {
                return RedirectToAction("Login");
            }
            return View(CalendarModel.viewableFormat());
        }

        [HttpPost]
        public ActionResult ViewStudy(FormCollection formData)
        {
            String userDay = formData["dayValue"] == "" ? null : formData["dayValue"];
            if (String.IsNullOrEmpty(userDay))
            {
                return RedirectToAction("ViewStudy");
            }
            else
            {
                String convertedDate = userDay.Substring(0, 4) + "/" + userDay.Substring(5, 2) + "/" + userDay.Substring(8, 2);
                CalendarModel.SDate = Convert.ToDateTime(convertedDate);
                CalendarModel.SelectedDate = convertedDate;
                return RedirectToAction("SelectModules");
            }

        }

        public ActionResult SelectModules()
        {
            if (!UserModel.loggedIn)
            {
                return RedirectToAction("Login");
            }
            return View(CalendarModel.userData);
        }

        [HttpPost]
        public ActionResult SelectModules(FormCollection formData)
        {
            String modCode = formData["modCode"] == "" ? null : formData["modCode"];
            int NumHours = Convert.ToInt32(formData["numHours"] == "" ? null : formData["numHours"]);
            CalendarModel.ModuleCode = modCode;
            CalendarModel.ModuleHours = NumHours;
            CalendarModel.attemptToCancel = false;
            CalendarModel cm = new CalendarModel();
            cm.dateStorer();
            return RedirectToAction("ViewStudy");
        }
        public ActionResult Graph()
        {
            if (!UserModel.loggedIn)
            {
                return RedirectToAction("Login");
            }
            GraphViewData gvd = new GraphViewData();

            List<String> codes = new List<string>();
            List<String> mydates = new List<string>();
            List<codeDays> codeHours = new List<codeDays>();
            foreach (var date in CalendarModel.datesList)
            {

                foreach (var module in date.plannedList)
                {
                    if (!codes.Contains(module.codes))
                    {
                        codes.Add(module.codes);
                    }

                }
            }

            foreach (var code in codes)
            {
                List<int> forHour = new List<int>();
                codeHours.Add(new codeDays()
                {
                    name = code,
                    data = forHour,
                });
            }

            //repeat for amount of dates
            for (int i = 0; i < CalendarModel.datesList.Count; i++)
            {
                String currentWeek = CalendarModel.currentWeek(CalendarModel.datesList.ElementAt(i).storedDate);
                int myDatesIndex = 0;
                Boolean weekExists = false;
                //first store which week it is
                if (mydates.Contains(currentWeek)) //if mydates contains current week
                {
                    for (int b = 0; b < mydates.Count; b++)
                    {
                        if (mydates.ElementAt(b).Equals(currentWeek))
                        {
                            myDatesIndex = b;
                            weekExists = true;
                            break;
                        }
                    }
                }
                else
                {
                    mydates.Add(currentWeek);
                }

                //repeat for each planned module for the current day
                foreach (var module in CalendarModel.datesList.ElementAt(i).plannedList)
                {
                    for (int s = 0; s < codeHours.Count; s++)
                    {
                        if (codeHours.ElementAt(s).name.Equals(module.codes)) //if a module matches 
                        {
                            if (weekExists) //if the current day is part of the current week
                            {
                                List<int> oldHours = new List<int>(); //to store old hours

                                for (int j = 0; j < codeHours.ElementAt(s).data.Count; j++)
                                {
                                    if (j == myDatesIndex)
                                    {
                                        int total = codeHours.ElementAt(s).data.ElementAt(j) + module.hours; 
                                        oldHours.Add(total);//updating old hours where week is current week
                                    }
                                    else
                                    {
                                        oldHours.Add(codeHours.ElementAt(s).data.ElementAt(j));
                                    }
                                }
                                codeHours.ElementAt(s).data = oldHours;
                            }
                            else
                            {
                                codeHours.ElementAt(s).data.Add(module.hours);
                            }

                        }
                        else
                            if (!weekExists)
                        {
                            codeHours.ElementAt(s).data.Add(0);
                        }

                    }
                }

                if (!weekExists)
                {
                    //To remove all 0s found within each codeHours that are beyond the current date index
                    for (int s = 0; s < codeHours.Count; s++)
                    {
                        int toStore = 0;
                        int current = i;
                        Boolean someLeft = true;
                        while (someLeft)
                        {
                            try
                            {
                                if (codeHours.ElementAt(s).data.ElementAt(current) != 0)
                                {
                                    toStore = codeHours.ElementAt(s).data.ElementAt(current);
                                }
                                if (current > i)
                                {
                                    codeHours.ElementAt(s).data.RemoveAt(current);
                                }
                            }
                            catch (Exception)
                            {
                                someLeft = false;
                            }
                            current++;
                        }
                        if (toStore > 0)
                        {

                            codeHours.ElementAt(s).data.RemoveAt(i);
                            codeHours.ElementAt(s).data.Add(toStore);

                        }
                    }
                }

            }

            gvd.userName = StartModel.Users[0];
            gvd.data = codeHours;
            gvd.dates = mydates;

            return View(gvd);
        }
        public ActionResult ViewModules()
        {
            if (!UserModel.loggedIn)
            {
                return RedirectToAction("Login");
            }
            List<ViewModuleData> userData = new List<ViewModuleData>();
            List<ModuleData> moduleDatas = new List<ModuleData>();
            //get all modules and store in a list of moduleData type
            for (int i = 0; i < Program.stored; i++)
            {
                var currentModule = from m in Program.moduleList
                                    where m.moduleID == i
                                    select new { m.codes, m.credits, m.selfHours, m.hours };//Retrieving relevant data for ID

                foreach (var item in currentModule)//Displaying the data from ID
                    moduleDatas.Add(new ModuleData()
                    {
                        ModuleCode = item.codes,
                        NumberOfCredits = item.credits,
                        ClassHoursPerWeek = item.hours,
                        SelfStudyHours = item.selfHours
                    });
            }
            userData.Add(new ViewModuleData()
            {
                userName = StartModel.Users[0],
                modules = moduleDatas
            });

            CalendarModel.userData = userData;
            return View(userData);
        }
    }
}