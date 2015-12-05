using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class DemographicsTrackerTest : DemographicsTracker
    {
        [TestMethod]
        public void TestParseDemographicsDatabase()
        { 
            Dictionary<string, string> pairs = new Dictionary<string, string>()
            {
                { "turn", "20" },
                { "food-rank", "1" },
                { "food-value", "12" },
                { "food-average", "10.5" }
            };
            ParseDatabaseEntries(pairs);
            foreach (var demo in Demographics)
            {
                Assert.AreEqual(20, demo.Turn);
                Assert.AreEqual(Categories.Food, demo.Category);
                Assert.AreEqual(1, demo.Rank);
                Assert.AreEqual(12, demo.Value);
                Assert.AreEqual(10.5, demo.Average);
            }
        }

        [TestMethod]
        public void TestSerializeDemographic()
        {
            Demographic demographic = new Demographic(20, Categories.Food, 10, 8, 1);
            Serializer serializer = new Serializer();
            string json = serializer.Serialize(demographic);
            Assert.IsTrue(json.Contains("\"rank\":1"));
        }
    }
}
