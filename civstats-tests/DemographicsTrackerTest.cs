using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class DemographicsTrackerTest : DemographicsTracker
    {
        [TestMethod]
        public void TestMakeDemographicsUpdate()
        { 
            DemographicsTracker tracker = new DemographicsTracker();
            Dictionary<string, string> pairs = new Dictionary<string, string>()
            {
                { "turn", "20" },
                { "food-rank", "1" },
                { "food-value", "12" },
                { "food-average", "10.5" }
            };
            ParseDatabaseEntries(pairs);
            Assert.AreEqual(20, Turn);
            foreach (var demo in Demographics)
            {
                Assert.AreEqual(Categories.Food, demo.Category);
                Assert.AreEqual(1, demo.Rank);
                Assert.AreEqual(12, demo.Value);
                Assert.AreEqual(10.5, demo.Average);
            }
        }
    }
}
