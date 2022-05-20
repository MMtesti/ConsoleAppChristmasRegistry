using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Console;

namespace ConsoleApp1
{
    class Data
    {
        public string FirstName;
        public string LastName;
        public string Gift;

        public Data (string firstName, string lastName, string gift)
        {
            FirstName = firstName;
            LastName = lastName;
            Gift = gift;
        }
        public Data (string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public Data (string gift)
        {
            Gift = gift;
        }
        public Data() { }
        
        public List<Data> GetChildrens(string filepath)
        {
            List<Data> myDataList= new List<Data>();
            string[] allLines = File.ReadAllLines(filepath);
            for (int i = 0; i < allLines.Length; i++)
            {
                string[] parts = allLines[i].Split(',');
                string fname = parts[0].Trim();
                string lname = parts[1].Trim();
                Data tmpData = new Data(fname, lname);
                myDataList.Add(tmpData);
            } 
            return myDataList;
        }
        public List<Data> GetGifts(string filepath)
        {
            List<Data> myDataList = new List<Data>();
            string[] allLines = File.ReadAllLines(filepath);
            for (int i = 0; i < allLines.Length; i++)
            {
                Data tmpData = new Data(allLines[i].Trim());
                myDataList.Add(tmpData);
            }
            return myDataList;
        }
        public void DisplayAll()
        {
            WriteLine("{0,-20} {1,-20} {2,-30}", FirstName, LastName, Gift);
        }
        public void DisplayChildren()
        {
            WriteLine("{0,-20} {1,-20}", FirstName, LastName);
        }
        public void DisplayGifts()
        {
            WriteLine("{0,-30}", Gift);
        }
    }
}
