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
            Assert.AreEqual("Poland", Info.Civilization);
            Assert.AreEqual(Speeds.Quick, Info.Speed);
            Assert.AreEqual(MapSizes.Small, Info.Size);
            Assert.AreEqual(Difficulties.Diety, Info.Difficulty);
            Assert.AreEqual("Continents", Info.Map);
        }
    }
}
