using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class ReligionTrackerTest : ReligionTracker
    {
        [TestMethod]
        public void TestMakeReligionUpdate()
        {
            ReligionTracker tracker = new ReligionTracker();
            Dictionary<string, string> pairs = new Dictionary<string, string>()
            {
                { "1", "Ancestor Worship" },
                { "1-type", "Pantheon" }
            };
            ParseDatabaseEntries(pairs);
            foreach (Belief belief in Beliefs)
            {
                Assert.AreEqual("Ancestor Worship", belief.Name);
                Assert.AreEqual(BeliefTypes.Pantheon, belief.Type);
            }
        }
    }
}
