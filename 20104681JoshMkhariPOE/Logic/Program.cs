using _20104681JoshMkhariPOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using timeManagement;

namespace _20104681JoshMkhariPOE.Logic
{
    public class PlannedModule
    {
        /*
         * Class summary
         * 
         * Used to store the modules planned for a specific day
         */
        public string codes;
        public int hours;
        public int remainingHours;
    }

    public class StoredDates
    {   /*
         * Class summary
         * 
         * Used to store the date along side the list of oolanned modules for the date
         */
        public int elementId;
        public string storedDate;
        public IList<PlannedModule> plannedList;//stores all modules
    }

    class Program
    {
        //paralel Lists

        //Custom Class Library
        public static IList<Module> moduleList = new List<Module>();//stores all modules

        public static List<int> chosenIDs = new List<int>();//stores the specific IDs of chosen modules

        public static int stored = 0;//Stored is used to set a moduleID, working as an Indexer

        public static void ResetModuleList()//USed to empty the ModuleList
        {
            moduleList.Clear();
        }

        public static void AddModule(int current, String moduleCode, String moduleName, int moduleCredits, int moduleHours) //To add modules
        {
            Calculations c1 = new Calculations();//Using CLassLibrary for Calculations
            c1.setSelfStudyHours(moduleCredits, Convert.ToInt32(StartModel.semesterWeeks), moduleHours);//Sets self study hours
            int modSelfHours;
            if (c1.getSelfStudyHours() < 1)//Ensuring that selfstudy hours are always displayed as a number that is greater than -1
            {
                modSelfHours = 0;
            }
            else
                modSelfHours = c1.getSelfStudyHours();

            moduleList.Add(new Module()//Populating moduleList
            {
                moduleID = current,
                codes = moduleCode,
                names = moduleName,
                credits = moduleCredits,
                hours = moduleHours,
                selfHours = modSelfHours
            });
            stored++;
        }


        public static Boolean ValidateInput(int check, String input) //Used to validate user input
        {
            Boolean passed = true;
            if (check == 1) //ModuleCode Check
            {
                if (input.Length != 8)
                {
                    return false;
                }
                else
                {
                    String Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    String nums = "0123456789";
                    char[] chars = input.ToCharArray();
                    for (int i = 0; i < 4; i++)
                    {
                        if (!Alphabet.Contains(chars[i]))
                        {
                            passed = false;
                        }
                    }
                    if (!passed)
                    {
                        return false;
                    }
                    else
                    {
                        passed = true;
                        for (int i = 4; i < 8; i++)
                        {
                            if (!nums.Contains(chars[i]))
                            {
                                passed = false;
                            }
                        }
                        if (!passed)
                        {
                            return false;
                        }
                        else
                            return true;
                    }
                }
            }
            else
            {
                if (check == 2)//Module Name check
                {
                    if (input.Length <= 3)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                    return false;
            }
        }
    }
}
