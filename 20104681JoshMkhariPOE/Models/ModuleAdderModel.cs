using _20104681JoshMkhariPOE.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace _20104681JoshMkhariPOE.Models
{
    /*
     * Class Summary
     * Adds Modules the user wants to store
     */
    class ModuleAdderModel
    {
        //Stores the relevant Module Information
        public static string ModuleCode { get; set; }
        public static string ModuleName { get; set; }
        public static int NumberOfCredits { get; set; }
        public static int ClassHoursPerWeek { get; set; }
        public static int ModuleID = 0;

        public string modulleName { get; set; }
        public string userName { get; set; }

        static List<string> moduleNames = new List<string>();
        static List<string> storedModules = new List<string>();

        public static void clearData()
        {
            moduleNames.Clear();
            storedModules.Clear();
        }
        public static Boolean populateModule()//Used to add modules to the module list
        {
            Boolean moduleExists = false;
            for (int i = 0; i < Program.moduleList.Count; i++) //repeat for the aomount of modules within the list
            {
                var currentModuloe = Program.moduleList.ElementAt(i);

                if (currentModuloe.codes.Equals(ModuleCode))// if module code within module list is equal to the module we are trying to add
                {
                    moduleExists = true;
                }
            }
            if (moduleExists)//If the module we are trying to add exists
            {
                return false;//Keep current view on ModuleAdder
            }
            else
            {
                if (storedModules.Contains(ModuleCode))
                {
                    ProgramDAL pal = new ProgramDAL();
                    pal.GetModule(ModuleCode);
                    pal.AddModuleList(ModuleCode, StartModel.Users[0]);
                }
                else
                {
                    ProgramDAL pal = new ProgramDAL();
                    pal.AddModule(ModuleCode, ModuleName, NumberOfCredits, ClassHoursPerWeek, StartModel.Users[0]);
                }
                Program.AddModule(ModuleID, ModuleCode, ModuleName, NumberOfCredits, ClassHoursPerWeek);
                ModuleID++;
                return true;//Change current view to view modules
            }

        }

        public static void Reset()
        {
            storedModules.Clear();
            moduleNames.Clear();
        }
        public void retrieveModules()
        {
            //first retrieve entire modules list
            ProgramDAL pal = new ProgramDAL();
            List<ModuleAdderModel> modList = new List<ModuleAdderModel>();
            modList = pal.GetAllModuleList().ToList();

            for (int i = 0; i < modList.Count; i++)
            {
                var currentUser = modList.ElementAt(i);
                if (currentUser.userName.Equals(StartModel.Users[0]))
                {
                    moduleNames.Add(currentUser.modulleName);
                }
                if (!storedModules.Contains(currentUser.modulleName))
                {
                    storedModules.Add(currentUser.modulleName);
                }

            }

            for (int i = 0; i < moduleNames.Count; i++)
            {
                //get each modules info from the modules database and then add them
                pal.GetModule(moduleNames.ElementAt(i));
                Program.AddModule(ModuleID, ModuleCode, ModuleName, NumberOfCredits, ClassHoursPerWeek);
                ModuleID++;
            }
        }

    }
}
