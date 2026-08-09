using System;
using System.Collections.Generic;

namespace Boggle.Models
{
    /// <summary>
    /// Finds every dictionary word that can legally be traced on a board.
    ///
    /// A depth-first walk from each tile, following the same adjacency rules the
    /// players get and never reusing a die within one word. The search is kept
    /// cheap by pruning on prefixes: as soon as the letters collected so far
    /// cannot begin any dictionary word, that whole branch is abandoned. Without
    /// that check the walk explores every path on the board regardless of whether
    /// it could ever spell anything.
    /// </summary>
    public static class BoardSolver
    {
        public const int MinWordLength = 3;

        public static List<string> Solve(Board board)
        {
            return Solve(board, WordDictionary.getInstance());
        }

        public static List<string> Solve(Board board, WordDictionary dictionary)
        {
            int size = board.boardSize();
            var found = new HashSet<string>(StringComparer.Ordinal);
            var visited = new bool[size, size];

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    walk(board, dictionary, r, c, "", visited, found);
                }
            }

            var words = new List<string>(found);
            // Longest first: callers pick by length, and this makes that trivial.
            words.Sort((a, b) => b.Length != a.Length
                ? b.Length - a.Length
                : string.CompareOrdinal(a, b));
            return words;
        }

        private static void walk(Board board, WordDictionary dictionary, int r, int c,
                                 string prefix, bool[,] visited, HashSet<string> found)
        {
            int size = board.boardSize();
            if (r < 0 || r >= size || c < 0 || c >= size) return;
            if (visited[r, c]) return;

            // A die face can be more than one character ("Qu"), so build from the
            // face rather than a single char, matching how a guess is assembled.
            string word = prefix + board.getDie(r, c).getUpLetter().ToLowerInvariant();

            if (!dictionary.HasPrefix(word)) return;

            visited[r, c] = true;

            if (word.Length >= MinWordLength && dictionary.IsWord(word))
            {
                found.Add(word);
            }

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    walk(board, dictionary, r + dr, c + dc, word, visited, found);
                }
            }

            visited[r, c] = false;
        }
    }
}
