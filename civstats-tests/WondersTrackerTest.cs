using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class WondersTrackerTest : WondersTracker
    {
        [TestMethod]
        public void TestMakeWondersUpdate()
        {
            WondersTracker tracker = new WondersTracker();
            Dictionary<string, string> pairs = new Dictionary<string, string>()
            {
                { "1", "1" },
                { "1-name", "Alhambra" },
                { "1-turn", "80" },
                { "1-conquered", "0" },
                { "1-city", "Moscow" }
            };
            ParseDatabaseEntries(pairs);
            foreach (Wonder wonder in Wonders)
            {
                Assert.IsTrue(wonder.Retained);
                Assert.AreEqual("Alhambra", wonder.Name);
                Assert.AreEqual(80, wonder.Turn);
                Assert.AreEqual(false, wonder.Captured);
                Assert.AreEqual("Moscow", wonder.CityName);
            }
        }
    }
}
