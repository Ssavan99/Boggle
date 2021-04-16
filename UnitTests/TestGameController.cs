using System;
using System.Collections.Generic;
using System.Text;
using Boggle.Controllers;
using Boggle.Models;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestGameController
    {
        [TestMethod]
        public void attemptWordTest()
        {
            User u1 = new User("Chris");
            User u2 = new User("Michael");
            User u3 = new User("Jean");

            String input1 = "11 22 12";
            String input2 = "21 33 10 32";
            String input3 = "33 02 00 03 10";

            GameController controller1 = new GameController();


            Assert.IsTrue(controller1.attemptWord(u1, input1));
            Assert.IsTrue(controller1.attemptWord(u2, input2));
            Assert.IsTrue(controller1.attemptWord(u3,input3));
        }
    }
}
