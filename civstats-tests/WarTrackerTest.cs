using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class WarTrackerTest : WarsTracker
    {
        [TestMethod]
        public void TestParseWarsDatabase() 
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>()
            {
                { "Poland-10-war", "1" },
                { "Poland-20-peace", "" }
            };
            ParseDatabaseEntries(pairs);
            foreach (var war in WarEvents)
            {
                if (war.IsPeaceTreaty)
                    Assert.AreEqual(20, war.Turn);
                else
                {
                    Assert.AreEqual(10, war.Turn);
                    Assert.AreEqual(true, war.Aggressor);
                }
                Assert.AreEqual("Poland", war.Civilization);
            }
        }
    }
}
