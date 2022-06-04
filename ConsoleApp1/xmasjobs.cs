using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class xmasjobs
    {
        List<Data> unChildren = new List<Data>();
        List<Data> unGifts = new List<Data>();
        List<Data> assignedData = new List<Data>();
        
        public void Start () 
        {
            int Year = DateTime.Now.Year;
            Title = ($"Christmas {Year} is comming!");
            string giftsFilePath = "GiftsList.txt";
            string childrenFilePath = "ChildrenList.txt";
            SetWindowSize(120, 50);
            Data myData = new Data();
            if ((File.Exists(giftsFilePath)) && (File.Exists(childrenFilePath)))
            {
                unChildren = myData.GetChildrens(childrenFilePath);
                unGifts = myData.GetGifts(giftsFilePath);
            }
            RunMainMenu();             
        }
        
        private void RunMainMenu()
        {
            
            string prompt = @"Welcome Santa! What would you like to do?         
(Use the arrow keys to navigate and press Enter to select an option)";
            string[] options = { "Registry Information", "Add new children or gifts", "Assign gifts", "Exit" };

            Menu mainMenu = new Menu(prompt, options);
            int selectedOption = mainMenu.Run();
            switch (selectedOption)
            {
                case 0:
                    RegInfo();
                    break;
                case 1:
                    Submenu1();
                    break;
                case 2:
                    Submenu2();
                    break;
                case 3:
                    ExitApp();
                    break;
            }
        }

        private void Submenu1()
        {
            string prompt = @"Let's add new Children or Gifts!
(Use the arrow keys to navigate and press Enter to select an option)";
            string[] options = { "Add new children", "Add new gifts", "Back to main menu" };

            Menu subMenu1 = new Menu(prompt, options);
            int selectedOption = subMenu1.Run();
            switch (selectedOption)
            {
                case 0:
                    AddNewChildren();
                    break;
                case 1:
                    AddNewGifts();
                    break;
                case 2:
                    BackToMain();
                    break;
            }
        }
        private void Submenu2()
        {
            string prompt = @"Let's assign some gifts!
(Use the arrow keys to navigate and press Enter to select an option)";
            string[] options = { "Automatically assign all gifts", "Automatically assign one gift", "Manually assign one gift", "Back to main menu" };

            Menu subMenu2 = new Menu(prompt, options);
            int selectedOption = subMenu2.Run();
            switch (selectedOption)
            {
                case 0:
                    AutoAssignAll();
                    break;
                case 1:
                    AutoAssignRandomGift();
                    break;
                case 2:
                    ManuallyAssign();
                    break;
                case 3:
                    BackToMain();
                    break;
            }
        }
        private void RegInfo()
        {
            Clear();
            DisplayUnassigned();
            DisplayGifts();
            DisplayAssigned();
            DisplayInfo("full");
            WriteLine("\nPress any key to return to menu...");
            ReadKey(true);
            RunMainMenu();
        }
        private void DisplayUnassigned()
        {
            WriteLine("All children without assigned gifts");
            WriteLine(new string('-', 45));
            WriteLine("{0,-5} {1,-20} {2,-20}", "ID", "First Name", "Last Name");
            WriteLine(new string('-', 45));
            for (int i = 0; i < unChildren.Count; i++)
            {
                Write("{0,-6}", $"{i + 1}");
                unChildren[i].DisplayChildren();
            }
        }
        private void DisplayGifts()
        {
            WriteLine("\nAvailable gifts");
            WriteLine(new string('-', 35));
            WriteLine("{0,-5} {1,-30}", "ID", "Gift");
            WriteLine(new string('-', 35));
            for (int i = 0; i < unGifts.Count; i++)
            {
                    Write("{0,-6}", $"{i + 1}");
                    unGifts[i].DisplayGifts();
            }
        }
        private void DisplayAssigned()
        {
            WriteLine("\nAll children with assigned gifts");
            WriteLine(new string('-', 75));
            WriteLine("{0,-5} {1,-20} {2,-20} {3,-30}", "ID", "Name", "Last Name", "Gift");
            WriteLine(new string('-', 75));
            for (int i = 0; i < assignedData.Count; i++)
            {
                Write("{0,-6}", $"{i + 1}");
                assignedData[i].DisplayAll();
            }
        }
        private void DisplayInfo(string details)
        {
            int pairs = assignedData.Count;
            int uGifts = unGifts.Count;
            int uKids = unChildren.Count;
            if (details == "full")
            {
                WriteLine($"\nAssigned pairs: {pairs}");
                WriteLine($"Unassigned children: {uKids}");
                WriteLine($"Available gifts: {uGifts}");
                if (uKids == 0)
                {
                    WriteLine("\nAll Children have gifts assigned. Good Job Santa!");
                }
            }
            if (uGifts < uKids)
            {
                ForegroundColor = ConsoleColor.DarkRed;
                BackgroundColor = ConsoleColor.White;
                WriteLine($"\nWARNING! You have more unassigned children then available gifts. Add at least {uKids - uGifts} gift(s)");
                ResetColor();
            }
        }
        private void AutoAssignRandomGift()
        {
            Clear();
            if (unChildren.Count >= 1 && unGifts.Count >= 1)
            {
                DisplayUnassigned();
                DisplayGifts();
                DisplayInfo("part");
                WriteLine();
                UInt16 childID;
                do
                {
                    WriteLine("Enter child ID");
                    try 
                    {
                        childID = Convert.ToUInt16(ReadLine());
                    }
                    catch
                    {
                        WriteLine("Please enter a valid positive number");
                        childID = 0;
                    }
                } while (!(childID >= 1 && childID <= unChildren.Count));

                Random rnd = new Random();
                int giftID = rnd.Next(unGifts.Count);
                Data tmpData = new Data(unChildren[childID - 1].FirstName, unChildren[childID - 1].LastName, unGifts[giftID].Gift);
                assignedData.Add(tmpData);
                WriteLine(unChildren[childID - 1].FirstName + " " + unChildren[childID - 1].LastName + " got " + unGifts[giftID].Gift + " assigned");
                unChildren.RemoveAt(childID - 1);
                unGifts.RemoveAt(giftID);
            }
            else
            {
                WriteLine("There must be at least 1 unassigned child and 1 gift available. Add more children or gifts");
            }
            WriteLine("\nPress any key to return to menu...");
            ReadKey(true);
            Submenu2();
        }
        private void ManuallyAssign()
        {
            Clear();
            if (unChildren.Count >= 1 && unGifts.Count >= 1)
            {
                DisplayUnassigned();
                DisplayGifts();
                DisplayInfo("part");
                WriteLine();
                UInt16 giftID, childID;
                do
                {
                    WriteLine("Enter gift ID");
                    try
                    {
                        giftID = Convert.ToUInt16(ReadLine());
                    }
                    catch
                    {
                        WriteLine("Please enter a valid positive number");
                        giftID = 0;
                    }
                } while (!(giftID >= 1 && giftID <= unGifts.Count));

                do
                {
                    WriteLine("Enter child ID");
                    try
                    {
                        childID = Convert.ToUInt16(ReadLine());
                    }
                    catch
                    {
                        WriteLine("Please enter a valid positive number");
                        childID = 0;
                    }
                } while (!(childID >= 1 && childID <= unChildren.Count));

                Data tmpData = new Data(unChildren[childID - 1].FirstName, unChildren[childID - 1].LastName, unGifts[giftID - 1].Gift);
                assignedData.Add(tmpData);
                WriteLine(unChildren[childID - 1].FirstName + "  " + unChildren[childID - 1].LastName + " got " + unGifts[giftID - 1].Gift + " assigned");
                unChildren.RemoveAt(childID - 1);
                unGifts.RemoveAt(giftID - 1);
            }
            else
            {
                WriteLine("There must be at least 1 unassigned child and 1 gift available. Add more children or gifts.");
            }
            WriteLine("\nPress any key to return to menu...");
            ReadKey(true);
            Submenu2();
        }
        private void AutoAssignAll()
        {
            Clear();
            if (unChildren.Count >= 1 && unGifts.Count >= 1)
            {
                if (unGifts.Count >= unChildren.Count)
                {
                    while (unChildren.Count >= 1)
                    {
                        Random rndGiftID = new Random();
                        Random rndChildID = new Random();
                        int giftID = rndGiftID.Next(unGifts.Count);
                        int childID = rndChildID.Next(unChildren.Count);
                        Data tmpData = new Data(unChildren[childID].FirstName, unChildren[childID].LastName, unGifts[giftID].Gift);
                        assignedData.Add(tmpData);
                        WriteLine(unChildren[childID].FirstName + " " + unChildren[childID].LastName + " got " + unGifts[giftID].Gift + " assigned");
                        unChildren.RemoveAt(childID);
                        unGifts.RemoveAt(giftID);
                        System.Threading.Thread.Sleep(1000);

                    }
                }
                else
                {
                    WriteLine("There must be more or the same amount of available gifts as there are unassigned children. Add more gifts.");
                }
            }
            else
            {
                WriteLine("There must be at least 1 unassigned child and 1 gift available. Add more children or gifts.");
            }

            WriteLine("\nPress any key to return to menu...");
            ReadKey(true);
            Submenu2();
        }
        private void AddNewChildren()
        {
            Clear();
            WriteLine("Be carefull, Santa!" +
                " A child's first and last name must begin with a capital letter, must only contain letters (en-US) and be 2-20 symbols lenght\n");
            int b,a = 0;
            string go;
            do
            {
                string fname;
                string lname;
                string pattern = @"^([A-Z][a-z]{1,19})$";
                do
                {
                    WriteLine("Enter first name");
                    fname = ReadLine();
                } while (!IsNameCorrect(fname, pattern));
                do
                {
                    WriteLine("Enter last name");
                    lname = ReadLine();
                } while (!IsNameCorrect(lname, pattern));
                               
                Data tmpData = new Data(fname, lname);
                bool existInUnassign = unChildren.Exists(x => x.FirstName == tmpData.FirstName && x.LastName == tmpData.LastName);
                bool existInAssign = assignedData.Exists(x => x.FirstName == tmpData.FirstName && x.LastName == tmpData.LastName);

                if (existInUnassign || existInAssign )
                { 
                   WriteLine($"{tmpData.FirstName} {tmpData.LastName} already exist");
                }
                else
                {
                    unChildren.Add(tmpData);
                    WriteLine($"{tmpData.FirstName} {tmpData.LastName} was added");
                }

                do
                {
                    WriteLine("Do you want to add more children? Y / N");
                    go = ReadLine();
                    if (go.ToLower().Equals("y"))
                    {
                        b = 1;
                    }
                    else if (go.ToLower().Equals("n"))
                    {
                        a = 1;
                        b = 1;
                    }
                    else
                    {
                        WriteLine("Please enter Y or N");
                        b = 0;
                    }
                } while (b == 0);
            } while (a == 0);
            
            WriteLine("\nPress any key to return to menu...");
            ReadKey(true);
            Submenu1();
        }

        private void AddNewGifts()
        {
            Clear();
            WriteLine("Be carefull, Santa!" +
                " A gift's name must start with a capital letter.\n A gift's name can consist of two words (with a space in between) 15 symbols long each." +
                "\n A second word of a gift can start with either a small or a capital letter." +
                "\n Only (en-US) letters are accepted\n");
            int b, a = 0;
            string pattern = @"^([A-Z][a-z]{1,14}\s*[a-zA-Z]{0,15})$";
            string go;
            do
            {
                string gift;
                do
                {
                    WriteLine("Enter a gift");
                    gift = ReadLine();
                } while (!IsNameCorrect(gift, pattern));

                Data tmpData = new Data(gift);
                unGifts.Add(tmpData);
                WriteLine($"{tmpData.Gift} was added");

                do
                {
                    WriteLine("Do you want to add more gifts? Y / N");
                    go = ReadLine();
                    if (go.ToLower().Equals("y"))
                    {
                        b = 1;
                    }
                    else if (go.ToLower().Equals("n"))
                    {
                        a = 1;
                        b = 1;
                    }
                    else
                    {
                        WriteLine("Please enter Y or N");
                        b = 0;
                    }
                } while (b == 0);
            } while (a == 0);

            while (unGifts.Count < unChildren.Count)
            {
                DisplayInfo("part");
                string gift;
                do
                {
                    WriteLine("Enter a gift");
                    gift = ReadLine();
                } while (!IsNameCorrect(gift, pattern));
                Data tmpData = new Data(gift);
                unGifts.Add(tmpData);
                WriteLine($"{tmpData.Gift} was added");
            }

            WriteLine("\nPress any key to return to menu...");
            ReadKey(true);
            Submenu1();
        }
        private void BackToMain()
        {
            RunMainMenu();
        }
        private void ExitApp()
        {
            WriteLine("\nPress any key to continue...");
            ReadKey(true);
            Environment.Exit(0);
        }
        private bool IsNameCorrect(string name, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(name);
        }
    }
}
