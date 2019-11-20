using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerial
{
    public class UniformRandomSelector
    {
        private int count = 0;
        private List<int> candidates = new List<int>();
        private Random rand = new Random();
        private int prevVal = 0;

        public int next(int requestedCount)
        {
            if (requestedCount < 2)
            {
                return 0;
            }

            if (requestedCount != count)
            {
                init(requestedCount);
            }

            if (candidates.Count == 0)
            {
                init(requestedCount);
            }

            int nextIndex = rand.Next(candidates.Count);
            int nextVal = candidates[nextIndex];

            if (nextVal == prevVal)
            {
                return next(requestedCount);
            }

            prevVal = nextVal;
            candidates.RemoveAt(nextIndex);

            return nextVal;
        }

        private void init(int requestedCount)
        {
            count = requestedCount;
            candidates = new List<int>(Enumerable.Range(0, requestedCount));
        }
    }
}
