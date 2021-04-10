using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Boggle.Models
{
    public class Dictionary
    {
        public List<string> DictionaryWords { set; get; }

        public Dictionary CreateDictionary()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"AppData\", "english3.txt");
            var logFile = File.ReadAllLines(path);
            var logList = new List<string>();
            foreach(var s in logFile)
            {
                s.Replace("\n", "");
                logList.Add(s);
            }
            Dictionary dict = new Dictionary
            {
                DictionaryWords = logList
            };
            return dict;
        }
        
        public bool IsWord(string word, Dictionary dict)
        {
            int index = dict.DictionaryWords.BinarySearch(word);
            if(index >= 0)
            {
                return true;
            }
            return false;
        }
    }
}
