using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Boggle.Models
{
    public class WordDictionaryModel
    {
        public List<string> DictionaryWords { set; get; }

        public static WordDictionaryModel CreateDictionary()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"AppData\", "english3.txt");
            var logFile = File.ReadAllLines(path);
            var logList = new List<string>();
            foreach (var s in logFile)
            {
                s.Replace("\n", "");
                logList.Add(s);
            }
            WordDictionaryModel dict = new WordDictionaryModel
            {
                DictionaryWords = logList
            };
            return dict;
        }

        public static bool IsWord(string word, WordDictionaryModel dict)
        {
            int index = dict.DictionaryWords.BinarySearch(word);
            if (index >= 0)
            {
                return true;
            }
            return false;
        }
    }
}


