using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Boggle.Models
{
    public class WordDictionary
    {
        public List<string> DictionaryWords { get; set ; }

        public WordDictionary()
        {   
            //the first path is for running tests while the second is for running the program
            //string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"Boggle", @"AppData", "english3.txt");
            string path = Path.Combine(Environment.CurrentDirectory, @"AppData", "english3.txt");
            var logFile = File.ReadAllLines(path);
            var logList = new List<string>();
            foreach (var s in logFile)
            {
                s.Replace("\n", "");
                logList.Add(s);
            }
            this.DictionaryWords = logList;
        }

        public bool IsWord(string word)
        {
            int index = this.DictionaryWords.BinarySearch(word);
            if (index >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
