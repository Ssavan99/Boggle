using System;
using System.Collections.Generic;
using System.Linq;
using Boggle.Controllers;
using Boggle.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestComputerOpponent
    {
        /// <summary>
        /// Forces a known board so solver results are predictable. Die faces are
        /// set by rolling until the wanted letter comes up, which is the only way
        /// to drive a Die from outside.
        /// </summary>
        private static Board boardFrom(string[] rows)
        {
            var dice = new Die[4, 4];
            for (int r = 0; r < 4; r++)
            {
                string[] letters = rows[r].Split(' ');
                for (int c = 0; c < 4; c++)
                {
                    dice[r, c] = new Die(new string[]
                    {
                        letters[c], letters[c], letters[c],
                        letters[c], letters[c], letters[c]
                    });
                    dice[r, c].roll();
                }
            }

            Board b = new Board();
            b.setDice(dice);
            return b;
        }

        [TestMethod]
        public void hasPrefixFindsRealPrefixes()
        {
            WordDictionary d = WordDictionary.getInstance();

            Assert.IsTrue(d.HasPrefix("ca"));
            Assert.IsTrue(d.HasPrefix("boggl"));
            Assert.IsFalse(d.HasPrefix("zzzq"));
        }

        [TestMethod]
        public void hasPrefixAcceptsCompleteWords()
        {
            WordDictionary d = WordDictionary.getInstance();
            Assert.IsTrue(d.HasPrefix("cat"));
        }

        [TestMethod]
        public void solverFindsWordsThatAreOnTheBoard()
        {
            Board b = boardFrom(new[]
            {
                "C A T S",
                "X X X X",
                "X X X X",
                "X X X X"
            });

            List<string> words = BoardSolver.Solve(b);

            Assert.IsTrue(words.Contains("cat"), "cat is traceable across the top row");
            Assert.IsTrue(words.Contains("cats"), "cats is traceable across the top row");
        }

        [TestMethod]
        public void solverRejectsWordsThatAreNotTraceable()
        {
            Board b = boardFrom(new[]
            {
                "C X X T",
                "X X X X",
                "X X X A",
                "X X X X"
            });

            List<string> words = BoardSolver.Solve(b);

            Assert.IsFalse(words.Contains("cat"),
                "the letters are not adjacent, so cat must not be found");
        }

        [TestMethod]
        public void solverNeverReusesADie()
        {
            // Only one 'o': "oo" words must not appear.
            Board b = boardFrom(new[]
            {
                "N O X X",
                "X X X X",
                "X X X X",
                "X X X X"
            });

            List<string> words = BoardSolver.Solve(b);

            Assert.IsFalse(words.Any(w => w.Contains("oo")),
                "a die cannot be used twice in one word");
        }

        [TestMethod]
        public void solverHonoursMinimumWordLength()
        {
            Board b = boardFrom(new[]
            {
                "C A T S",
                "X X X X",
                "X X X X",
                "X X X X"
            });

            Assert.IsTrue(BoardSolver.Solve(b).All(w => w.Length >= BoardSolver.MinWordLength));
        }

        [TestMethod]
        public void harderSettingsPlayMoreWords()
        {
            var solved = new List<string>();
            for (int i = 0; i < 60; i++) solved.Add(new string('a', 3 + (i % 5)));
            // distinct entries
            solved = solved.Select((w, i) => w + i).ToList();

            var rnd = new Random(1);
            int easy = ComputerPlayer.BuildPlan(solved, Difficulty.Easy, 180, rnd).Count;
            int medium = ComputerPlayer.BuildPlan(solved, Difficulty.Medium, 180, rnd).Count;
            int hard = ComputerPlayer.BuildPlan(solved, Difficulty.Hard, 180, rnd).Count;

            Assert.IsTrue(easy < medium, "medium should play more words than easy");
            Assert.IsTrue(medium < hard, "hard should play more words than medium");
        }

        [TestMethod]
        public void planIsSpreadAcrossTheRound()
        {
            var solved = Enumerable.Range(0, 40).Select(i => "word" + i).ToList();
            var plan = ComputerPlayer.BuildPlan(solved, Difficulty.Hard, 180, new Random(7));

            Assert.IsTrue(plan.Count > 1);
            Assert.IsTrue(plan.First().AtSecond >= 0);
            Assert.IsTrue(plan.Last().AtSecond <= 180, "no move may land after the round ends");

            for (int i = 1; i < plan.Count; i++)
            {
                Assert.IsTrue(plan[i].AtSecond >= plan[i - 1].AtSecond, "plan must be ordered");
            }
        }

        [TestMethod]
        public void emptyBoardPlanIsEmpty()
        {
            var plan = ComputerPlayer.BuildPlan(new List<string>(), Difficulty.Hard, 180, new Random(1));
            Assert.AreEqual(0, plan.Count);
        }

        [TestMethod]
        public void botJoinsAsAPlayerWhenRequested()
        {
            ServerController c = new ServerController();
            Game g = c.getServer().newGame();
            g.enableBot(Difficulty.Medium);

            Assert.IsTrue(g.isBotEnabled());
            Assert.IsNotNull(g.getUser(ComputerPlayer.BotName));
        }

        [TestMethod]
        public void botPlaysOnlyWhatIsDue()
        {
            Game g = new Game(1, DateTime.Now);
            g.enableBot(Difficulty.Hard);
            g.setState(Game.State.Playing);
            g.planBotMoves(new Random(3));

            User bot = g.getUser(ComputerPlayer.BotName);

            g.advanceBot(0);
            int atStart = bot.getWordsUsed().Count;

            g.advanceBot(180);
            int atEnd = bot.getWordsUsed().Count;

            Assert.IsTrue(atEnd >= atStart, "word count must not go backwards");
            Assert.IsTrue(atEnd > 0, "the computer should have played by the end of the round");
        }

        [TestMethod]
        public void finishBotReleasesEverything()
        {
            Game g = new Game(2, DateTime.Now);
            g.enableBot(Difficulty.Hard);
            g.setState(Game.State.Playing);
            g.planBotMoves(new Random(11));

            g.finishBot();

            User bot = g.getUser(ComputerPlayer.BotName);
            Assert.AreEqual(g.getBotPlanCount(), bot.getWordsUsed().Count);
        }

        [TestMethod]
        public void gamesWithoutABotAreUnaffected()
        {
            Game g = new Game(3, DateTime.Now);
            g.setState(Game.State.Playing);

            g.planBotMoves(new Random(1));
            g.advanceBot(180);
            g.finishBot();

            Assert.IsFalse(g.isBotEnabled());
            Assert.IsNull(g.getUser(ComputerPlayer.BotName));
            Assert.AreEqual(0, g.getPlayerCount());
        }

        [TestMethod]
        public void difficultyParsingFallsBackToMedium()
        {
            Assert.AreEqual(Difficulty.Easy, ComputerPlayer.ParseDifficulty("easy"));
            Assert.AreEqual(Difficulty.Hard, ComputerPlayer.ParseDifficulty("HARD"));
            Assert.AreEqual(Difficulty.Medium, ComputerPlayer.ParseDifficulty("nonsense"));
            Assert.AreEqual(Difficulty.Medium, ComputerPlayer.ParseDifficulty(null));
        }
    }
}
