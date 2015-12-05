using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class PolicyChoicesTrackerTest : PolicyChoicesTracker
    {
        [TestMethod]
        public void TestParsePolicyDatabase()
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>()
            {
                { "1", "1" },
                { "1-branch", "Tradition" },
                { "1-name", "Aristocracy" },
                { "1-turn", "13" },
                { "1-cost", "15.53" }
            };
            ParseDatabaseEntries(pairs);
            foreach (var choice in Choices)
            {
                Assert.AreEqual(13, choice.Turn);
                Assert.AreEqual(PolicyBranches.Tradition, choice.Branch);
                Assert.AreEqual("Aristocracy", choice.Name);
                Assert.AreEqual(15.53, choice.Cost, 0.1);
            }
        }
    }
}
