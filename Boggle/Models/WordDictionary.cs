using System;
using System.Collections.Generic;
using System.IO;

namespace Boggle.Models
{
    public class WordDictionary
    {
        private static WordDictionary inst = new WordDictionary();

        public List<string> DictionaryWords { get; set; }

        public WordDictionary()
        {
            string path = ResolveDictionaryPath();
            this.DictionaryWords = new List<string>(File.ReadAllLines(path));
        }

        /// <summary>
        /// Locates english3.txt without depending on the process working directory.
        /// The app's base directory is the reliable anchor: it is correct under
        /// "dotnet run", "dotnet test", and a published container image alike,
        /// whereas Environment.CurrentDirectory varies by host.
        /// </summary>
        private static string ResolveDictionaryPath()
        {
            var candidates = new List<string>
            {
                Path.Combine(AppContext.BaseDirectory, "AppData", "english3.txt"),
                Path.Combine(AppContext.BaseDirectory, "wwwroot", "data", "english3.txt"),
                Path.Combine(Environment.CurrentDirectory, "AppData", "english3.txt"),
                Path.Combine(Environment.CurrentDirectory, "wwwroot", "data", "english3.txt")
            };

            foreach (string candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            throw new FileNotFoundException(
                "Could not locate the word dictionary (english3.txt). Searched: " +
                string.Join("; ", candidates));
        }

        /// <summary>
        /// The dictionary file is sorted, so a binary search is valid here.
        /// Callers are expected to pass a lower-cased word.
        /// </summary>
        public bool IsWord(string word)
        {
            return this.DictionaryWords.BinarySearch(word) >= 0;
        }

        /// <summary>
        /// True when at least one dictionary word starts with the given text.
        ///
        /// This is what makes solving a board tractable: a depth-first walk can
        /// abandon a path the moment no word could still be reached from it,
        /// instead of exploring every route to full length. Because the list is
        /// sorted, the first entry at or after the prefix is the only candidate
        /// that needs checking.
        /// </summary>
        public bool HasPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) return true;

            int idx = this.DictionaryWords.BinarySearch(prefix);
            if (idx >= 0) return true;

            idx = ~idx;
            return idx < this.DictionaryWords.Count
                && this.DictionaryWords[idx].StartsWith(prefix, StringComparison.Ordinal);
        }

        public static WordDictionary getInstance()
        {
            return inst;
        }
    }
}
