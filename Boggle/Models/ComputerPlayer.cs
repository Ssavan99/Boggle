using System;
using System.Collections.Generic;
using System.Linq;

namespace Boggle.Models
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    /// <summary>
    /// One word the computer intends to play, and how many seconds into the
    /// round it plays it.
    /// </summary>
    public class BotMove
    {
        public string Word { get; set; }
        public int AtSecond { get; set; }
    }

    /// <summary>
    /// Turns a solved board into a plausible run of play for the computer.
    ///
    /// The whole plan is decided up front and then released on a clock, so the
    /// opponent never needs a thread of its own: the game simply asks which
    /// moves are due whenever a client checks in. That keeps it deterministic
    /// and testable, and means a paused or abandoned game costs nothing.
    /// </summary>
    public static class ComputerPlayer
    {
        public const string BotName = "Computer";

        public static Difficulty ParseDifficulty(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return Difficulty.Medium;
            switch (value.Trim().ToLowerInvariant())
            {
                case "easy": return Difficulty.Easy;
                case "hard": return Difficulty.Hard;
                default: return Difficulty.Medium;
            }
        }

        /// <summary>
        /// Picks the words the computer will play and spaces them across the
        /// round. Harder settings play more words and reach for longer ones,
        /// which score far more under the length-based scoring rules.
        /// </summary>
        public static List<BotMove> BuildPlan(List<string> solvedWords, Difficulty difficulty,
                                              int roundSeconds, Random rnd)
        {
            var plan = new List<BotMove>();
            if (solvedWords == null || solvedWords.Count == 0) return plan;

            int target;
            int maxLength;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    target = 5;
                    maxLength = 4;
                    break;
                case Difficulty.Hard:
                    target = 24;
                    maxLength = int.MaxValue;
                    break;
                default:
                    target = 12;
                    maxLength = 6;
                    break;
            }

            // solvedWords arrives longest-first. Easy and Medium are held back to
            // shorter words so the computer stays beatable; Hard takes the best.
            var eligible = solvedWords.Where(w => w.Length <= maxLength).ToList();
            if (eligible.Count == 0) eligible = solvedWords;

            List<string> chosen;
            if (difficulty == Difficulty.Hard)
            {
                chosen = eligible.Take(target).ToList();
            }
            else
            {
                // Spread the picks through the eligible set rather than taking a
                // single block, so the computer does not always play the same
                // cluster of words on a given board.
                chosen = eligible.OrderBy(_ => rnd.Next()).Take(target).ToList();
            }

            if (chosen.Count == 0) return plan;

            // Leave a little quiet at each end of the round.
            int first = Math.Max(2, (int)(roundSeconds * 0.10));
            int last = Math.Max(first + 1, (int)(roundSeconds * 0.90));
            int span = last - first;

            for (int i = 0; i < chosen.Count; i++)
            {
                int at = chosen.Count == 1
                    ? first
                    : first + (int)Math.Round((double)span * i / (chosen.Count - 1));

                plan.Add(new BotMove { Word = chosen[i], AtSecond = at });
            }

            plan.Sort((a, b) => a.AtSecond - b.AtSecond);
            return plan;
        }
    }
}
