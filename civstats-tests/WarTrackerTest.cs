using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class WarEventsTrackerTest : WarEventsTracker
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
            foreach (var warEvent in WarEvents)
            {
                if (warEvent.Peace)
                    Assert.AreEqual(20, warEvent.Turn);
                else
                {
                    Assert.AreEqual(10, warEvent.Turn);
                    Assert.AreEqual(true, warEvent.Aggressor);
                }
                Assert.AreEqual("Poland", warEvent.Civilization);
            }
        }
    }
}
