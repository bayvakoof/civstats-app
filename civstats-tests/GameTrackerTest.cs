using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class GameTrackerTest : GameTracker
    {
        [TestMethod]
        public void TestMakeGameUpdate()
        {
            GameTracker tracker = new GameTracker();
            Dictionary<string, string> pairs = new Dictionary<string, string>()
            {
                { "civilization", "Poland" },
                { "speed", "Quick" },
                { "size", "Small" },
                { "map", "Continents" },
                { "difficulty", "Diety" }
            };
            ParseDatabaseEntries(pairs);
            Assert.AreEqual("Poland", Civilization);
            Assert.AreEqual(Speeds.Quick, Speed);
            Assert.AreEqual(MapSizes.Small, Size);
            Assert.AreEqual(Difficulties.Diety, Difficulty);
            Assert.AreEqual("Continents", Map);
        }
    }
}
