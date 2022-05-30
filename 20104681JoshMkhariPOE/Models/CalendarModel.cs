using _20104681JoshMkhariPOE.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _20104681JoshMkhariPOE.Models
{
    /*
     * Class summary
     * 
     * Used to store all neccessary data from the Calendar View
     * Performs all calendar related methods for the Calendar View
     */
    public class CalendarModel
    {
        public static string SelectedDate { get; set; }//Stores the current selected date from the calendar
        public static DateTime SDate { get; set; }//Stores the date in use for choosing study hours
        public static string ModuleCode { get; set; }//Stores the module code the user selects on the ViewModels View
        public static int ModuleHours { get; set; }//Stores the module hours the user selects on the ViewModels View

        public static Boolean inProcess { get; set; }//Used to alert the ViewModule View of the program on what actions are currently allowed

        public static Boolean attemptToCancel { get; set; }//Used to alert the MainWIndow  of the program on what actions are currently allowed

        public static Boolean addingToDatabase { get; set; }//

        public static IList<StoredDates> datesList = new List<StoredDates>();//stores all modules for a specific day and the date
        public static IList<StoredDates> usedDatesList = new List<StoredDates>();//stores all modules for a specific day and the date
        public static int planned { get; set; }

        public static List<ViewModuleData> userData = new List<ViewModuleData>();
        public string userName;
        public DateTime StoreDate;
        public int plan, hours;
        public string code;
        public static void Reset()
        {
            datesList.Clear();
            inProcess = false;
        }
        public void dateStorer()//Used to add a date and the modules for the date
        {

            ProgramDAL dal = new ProgramDAL();
            planned = dal.GetPlannedID() + 1;
            int moduleHours = 0;
            IList<PlannedModule> istOfModules = new List<PlannedModule>();//Used to keep a temporary copy of the module codes and hours already stored within the dates list for a specific day
            Boolean modFound = false;//used to determine if the current module being stored already exists within the curent dates list of modules
            Boolean datFound = false;//used to determine if the current date being stored already exists within the date list
            int datefound = 0;//Stores the location of the found date from the dates list, used to remove it later as it is replaced with an update for the specific day

            for (int i = 0; i < datesList.Count; i++)//repeat for the current length of the dates list
            {
                var currDate = datesList.ElementAt(i);//store the current element within the dateslist

                if (currDate.storedDate.Equals(SelectedDate))//we found a date that already exists
                {
                    datFound = true;
                    datefound = i;
                    int repeat = currDate.plannedList.Count;//stores the count of modules within the current date
                    List<string> foundCodes = new List<string>();//Stores a list of module codes that already exist for a specific day
                    for (int s = 0; s < repeat; s++)//now we are going to look at each module in found date
                    {
                        var curplan = currDate.plannedList.ElementAt(s);
                        if (curplan.codes.Equals(ModuleCode))//if a module is the same as our current module to add
                        {

                            istOfModules.Add(new PlannedModule()//add that module code and then add its old hours with our new hours
                            {
                                codes = curplan.codes,
                                hours = curplan.hours + ModuleHours
                            });
                            foundCodes.Add(ModuleCode);
                            modFound = true;
                            moduleHours = curplan.hours + ModuleHours;
                        }
                        else
                        {
                            if (!foundCodes.Contains(curplan.codes))// check if our found codes does not contain the current code from the current date 
                            {
                                istOfModules.Add(new PlannedModule()//add that module and the add its code
                                {
                                    codes = curplan.codes,
                                    hours = curplan.hours
                                });
                                foundCodes.Add(curplan.codes);
                                moduleHours = curplan.hours;
                            }

                        }
                    }
                    if (!modFound)//If the day did not have the module we are trying to add
                    {
                        //we want every module stored on said day
                        //istOfModules = currDate.plannedList;// make a copy of the current fays planned modules
                        istOfModules.Add(new PlannedModule()//add our new module and its code
                        {
                            codes = ModuleCode,
                            hours = ModuleHours
                        });
                        datesList.RemoveAt(i);
                        datesList.Add(new StoredDates()
                        {
                            storedDate = SelectedDate,
                            plannedList = istOfModules
                        });
                        break;
                    }
                    break;
                }
            }
            if (!datFound)//If we did not find the current day within our dates list
            {
                Boolean InUsedList = false;
                //check if it is within the usedDatesList
                foreach (var item in usedDatesList)
                {
                    if (item.storedDate.Equals(SelectedDate))
                    {
                        istOfModules.Add(new PlannedModule()
                        {
                            codes = ModuleCode,
                            hours = ModuleHours
                        });
                        //now add the current day and the updated list of modules which includes the old modules and our new addition
                        datesList.Add(new StoredDates()
                        {
                            storedDate = SelectedDate,
                            plannedList = istOfModules
                        });
                        //planned = dal.GetCurrentPlanned(SDate, StartModel.Users[0]);
                        dal.AddPlanned(planned);
                        dal.AddDateToList(SDate, StartModel.Users[0], planned);
                        dal.AddPlannedList(planned, ModuleCode, ModuleHours);//this is fine
                        addingToDatabase = false;
                        InUsedList = true;
                        break;
                    }
                }
                if (!InUsedList)
                {
                    istOfModules.Add(new PlannedModule()
                    {
                        codes = ModuleCode,
                        hours = ModuleHours
                    });

                    //add the current day to the dates list
                    datesList.Add(new StoredDates()
                    {
                        storedDate = SelectedDate,
                        plannedList = istOfModules
                    });
                    if (addingToDatabase)
                    {
                        dal.AddDate(SDate);
                        dal.AddPlanned(planned);
                        dal.AddDateToList(SDate, StartModel.Users[0], planned);
                        dal.AddPlannedList(planned, ModuleCode, ModuleHours);//this is fine
                    }
                }

            }
            if (datFound)
            {
                if (modFound)
                {
                    //first remove the previous version of the current day
                    datesList.RemoveAt(datefound);

                    //now add the current day and the updated list of modules which includes the old modules and our new addition
                    datesList.Add(new StoredDates()
                    {
                        storedDate = SelectedDate,
                        plannedList = istOfModules
                    });
                    if (addingToDatabase)
                    {
                        planned = dal.GetCurrentPlanned(SDate, StartModel.Users[0]);
                        dal.UpdatePlannedList(planned, ModuleCode, moduleHours);
                    }
                }
                else
                {
                    datesList.RemoveAt(datefound);

                    //now add the current day and the updated list of modules which includes the old modules and our new addition
                    datesList.Add(new StoredDates()
                    {
                        storedDate = SelectedDate,
                        plannedList = istOfModules
                    });
                    if (addingToDatabase)
                    {
                        planned = dal.GetCurrentPlanned(SDate, StartModel.Users[0]);
                        dal.AddPlannedList(planned, ModuleCode, moduleHours);
                        addingToDatabase = false;
                    }
                }
            }
            addingToDatabase = true;
        }

        public void usedDates()
        {
            ProgramDAL dal = new ProgramDAL();
            List<DateTime> dat = (List<DateTime>)dal.GetAllStoredDates();
            for (int i = 0; i < dat.Count; i++)
            {
                IList<PlannedModule> istOfModules = new List<PlannedModule>();
                usedDatesList.Add(new StoredDates()
                {
                    storedDate = dat.ElementAt(i).ToString().Substring(0, 10),
                    plannedList = istOfModules
                });
            }
        }
        public void populateFromDatabase()
        {

            ProgramDAL dal = new ProgramDAL();
            List<CalendarModel> datList = (List<CalendarModel>)dal.GetAllStoredDates(StartModel.Users[0]);
            for (int i = 0; i < datList.Count; i++)
            {
                IList<PlannedModule> istOfModules = new List<PlannedModule>();//Used to keep a temporary copy of the module codes and hours already stored within the dates list for a specific day
                List<CalendarModel> data = (List<CalendarModel>)dal.GetModuleHours(datList.ElementAt(i).plan);
                for (int s = 0; s < data.Count; s++)
                    istOfModules.Add(new PlannedModule()//add that module and the add its code
                    {
                        codes = data.ElementAt(s).code,
                        hours = data.ElementAt(s).hours
                    });
                datesList.Add(new StoredDates()
                {
                    storedDate = datList.ElementAt(i).StoreDate.ToString().Substring(0, 10),
                    plannedList = istOfModules
                });
            }

            usedDates();

            addingToDatabase = true;
        }

        public static List<DoubleList> calculateRemainingHoursForWeek(DateTime day)
        {
            List<string> currentWeekCode = new List<string>();
            List<int> currentWeekHours = new List<int>();
            int currentModuleSelfHours = 0;
            int currentDayInt = Convert.ToInt32(day.DayOfWeek); //5

            for (int i = 0; i < 7; i++)// Repeats for each day of the week
            {
                String currentDay = day.AddDays(-currentDayInt + i).ToString().Substring(0, 10);
                for (int s = 0; s < datesList.Count(); s++) //Repeats for each item in dateslist
                {
                    var currentDate = datesList.ElementAt(s);
                    if (currentDate.storedDate.Equals(currentDay)) //check if the currentdate is our current day
                    {
                        for (int t = 0; t < currentDate.plannedList.Count(); t++) //now we are going to extract every planned module object for the day
                        {
                            var currentList = currentDate.plannedList.ElementAt(t);
                            if (currentWeekCode.Contains(currentList.codes)) //check if the list we currently has this module
                            {
                                for (int b = 0; b < currentWeekCode.Count(); b++) //repeat for the length of added modules in our list
                                {
                                    if (currentWeekCode.ElementAt(b).Equals(currentList.codes))//if we find the right module code
                                    {
                                        int currTotal = currentWeekHours.ElementAt(b);//8
                                        //currentWeekHours is alwayys remaining hours
                                        // now we should subtract this total from self hours

                                        for (int v = 0; v < Program.moduleList.Count(); v++)//let us retrieve the self hours for this module
                                        {
                                            var currentProgram = Program.moduleList.ElementAt(v);
                                            if (currentProgram.codes.Equals(currentList.codes))
                                            {
                                                currentModuleSelfHours = currentProgram.selfHours;//9
                                                break;//Stop searching as we found what we needed
                                            }
                                        }
                                        currTotal = currentModuleSelfHours - currTotal;//9-8 = 1
                                        int newTotal = currTotal + currentList.hours; //retrieving the current total for week hours
                                        int remainingHours = currentModuleSelfHours - newTotal; //this is the remaining hours
                                        if (remainingHours < 1)
                                        {
                                            remainingHours = 0;
                                        }
                                        currentWeekCode.RemoveAt(b);
                                        currentWeekHours.RemoveAt(b);

                                        currentWeekCode.Add(currentList.codes);
                                        currentWeekHours.Add(remainingHours);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                currentWeekCode.Add(currentList.codes);
                                for (int v = 0; v < Program.moduleList.Count(); v++)//let us retrieve the self hours for this module
                                {
                                    var currentProgram = Program.moduleList.ElementAt(v);
                                    if (currentProgram.codes.Equals(currentList.codes))
                                    {
                                        currentModuleSelfHours = currentProgram.selfHours;
                                        break;
                                    }
                                }
                                int remainingHours = currentModuleSelfHours - currentList.hours;
                                if (remainingHours < 1)
                                {
                                    remainingHours = 0;
                                }
                                currentWeekHours.Add(remainingHours);
                            }
                        }
                    }
                }
            }
            //run through module list, check if elemet at 1 is in current week, if not add it to current week with its self study hours xD
            for (int i = 0; i < Program.moduleList.Count; i++)
            {
                var currentModule = Program.moduleList.ElementAt(i);
                if (!currentWeekCode.Contains(currentModule.codes))// if our list of current week modules has the 
                {
                    currentWeekCode.Add(currentModule.codes);
                    currentWeekHours.Add(currentModule.selfHours);
                }
            }
            List<DoubleList> remaininghHours = new List<DoubleList>();
            if (currentWeekCode.Count != 0)
            {
                remaininghHours.Add(new DoubleList()
                {
                    currentCode = currentWeekCode,
                    currentHours = currentWeekHours
                });
            }
            return remaininghHours;
        }

        public static List<ViewCalendarData> viewableFormat()
        {
            List<ViewCalendarData> userData = new List<ViewCalendarData>();
            IList<StoredDates> reworkedDate = new List<StoredDates>();
            int count = 0;

            foreach (var date in datesList)
            {
                String Year = date.storedDate.Substring(0, 4);
                String month = date.storedDate.Substring(5, 2);
                String myDate = date.storedDate.Substring(8, 2);

                List<DoubleList> remaininghHours = calculateRemainingHoursForWeek(Convert.ToDateTime(date.storedDate));

                switch (month)
                {
                    case "01":
                        myDate = myDate + " January";
                        break;
                    case "02":
                        myDate = myDate + " February";
                        break;
                    case "03":
                        myDate = myDate + " March";
                        break;
                    case "04":
                        myDate = myDate + " April";
                        break;
                    case "05":
                        myDate = myDate + " May";
                        break;
                    case "06":
                        myDate = myDate + " June";
                        break;
                    case "07":
                        myDate = myDate + " July";
                        break;
                    case "08":
                        myDate = myDate + " August";
                        break;
                    case "09":
                        myDate = myDate + " September";
                        break;
                    case "10":
                        myDate = myDate + " October";
                        break;
                    case "11":
                        myDate = myDate + " November";
                        break;
                    case "12":
                        myDate = myDate + " December";
                        break;
                }

                myDate = myDate + " " + Year;
                for (int i = 0; i < remaininghHours.ElementAt(0).currentCode.Count; i++)
                {
                    for (int s = 0; s < date.plannedList.Count; s++)
                    {
                        if (date.plannedList.ElementAt(s).codes.Equals(remaininghHours.ElementAt(0).currentCode.ElementAt(i)))
                        {
                            date.plannedList.ElementAt(s).remainingHours = remaininghHours.ElementAt(0).currentHours.ElementAt(i);
                            break;
                        }
                    }

                }

                reworkedDate.Add(new StoredDates()
                {
                    storedDate = myDate,
                    elementId = count,
                    plannedList = date.plannedList,
                });
                count++;
            }
            userData.Add(new ViewCalendarData()
            {
                userName = StartModel.Users[0],
                calendar = reworkedDate
            });
            return userData;
        }

        public static String currentWeek(string date)
        {
            DateTime day = Convert.ToDateTime(date);

            int currentDay = Convert.ToInt32(day.DayOfWeek); //5
            String edit = day.AddDays(-currentDay).ToString("D");// Monday, 15 June 2009
            Boolean spaceMissing = true;
            int index = 0;

            //Finding the first empty string character
            do
            {

                if (edit.Substring(index, 1).Equals(" "))
                {
                    spaceMissing = false;
                }
                index++;
            }
            while (spaceMissing);

            /*
             * Eg: String is "Monday, 15 June 2009"
             * 
             * THe code below will extract only the "15 June " part
             */
            int start = index;
            int spacesCount = 0;
            edit = edit.Substring(start);
            index = 0;
            do
            {
                if (edit.Substring(index, 1).Equals(" "))
                {
                    spacesCount++;
                }
                index++;
            }
            while (spacesCount != 2);

            string edited = edit.Substring(0, index);

            return "Week of " + edited;
        }
    }


}
