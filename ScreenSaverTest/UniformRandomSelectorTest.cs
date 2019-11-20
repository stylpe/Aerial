using Aerial;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ScreenSaverTest
{
    [TestClass]
    public class UniformRandomSelectorTest
    {
        private UniformRandomSelector sut;

        [TestInitialize]
        public void TestInit()
        {
            sut = new UniformRandomSelector();
        }

        [TestMethod]
        public void SelectsUniformly()
        {
            int count = 20;
            List<int> counters = new List<int>(Enumerable.Repeat(0, count).ToList());
            for (int i = 0; i < count * 100; i++)
            {
                int next = sut.next(count);
                //Debug.WriteLine(next);
                counters[next]++;
            }

            Assert.IsTrue(counters.Max() - counters.Min() < 2);
        }

        [TestMethod, Timeout(10000)]
        public void DoesntHangWithFewElements()
        {
            int zero = sut.next(0);
            int one = sut.next(1);
            sut.next(2);

            Assert.AreEqual(0, zero);
            Assert.AreEqual(0, one);
        }

        [TestMethod]
        public void SelectsNonSequentially()
        {
            int count = 20;
            Op prevOp = Op.NONE;
            int sequenceCounter = 0;
            int prev = 0;
            for (int i = 0; i < count * 100; i++)
            {
                int next = sut.next(count);
                Op op = Op.NONE;

                if (prev < next)
                {
                    op = Op.GT;
                }
                else if (prev > next)
                {
                    op = Op.LT;
                }
                else
                {
                    op = Op.EQ;
                }

                if ((next == prev + 1 && prevOp == Op.GT) || 
                    (next == prev - 1 && prevOp == Op.LT)) {
                    sequenceCounter++;
                }
                else
                {
                    sequenceCounter = 0;
                }

                if (sequenceCounter > 4)
                {
                    Assert.Fail("Found more than four sequential numbers");
                }

                prevOp = op;
                prev = next;
                //Debug.WriteLine(next);
            }
        }

        [TestMethod]
        public void DoesNotYieldSameNumberTwiceInARow()
        {
            int count = 20;
            int prev = 0;
            for (int i = 0; i < count * 100; i++)
            {
                int next = sut.next(count);
                Assert.AreNotEqual(prev, next, "Same number twice in a row!");
                prev = next;
            }
        }

        enum Op {
            GT,
            LT,
            EQ,
            NONE
        }
    }
}
