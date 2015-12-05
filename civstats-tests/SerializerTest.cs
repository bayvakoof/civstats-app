using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using civstats;
using civstats.Trackers;

namespace civstats_tests
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void TestDirtySerialization()
        {
            // TODO be able to detect what specific things in each tracker have changed
            // i.e. detect new wonder, detect that policy changed etc.
            // skip
            return;
        }
    }
}
