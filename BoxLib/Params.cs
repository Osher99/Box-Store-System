using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxLib
{
    public struct Params
    {
        public TimeSpan ExpirationDate { get; private set; }
        public TimeSpan TimerInterval { get; private set; }
        public int MaxCapacity { get; private set; }
        public int MinCapacity { get; private set; }

        public Params(TimeSpan expirationDate, TimeSpan timerInterval, int maxCapacity = 100, int minCapacity = 5)
        {
            ExpirationDate = expirationDate;
            TimerInterval = timerInterval;
            MaxCapacity = maxCapacity;
            MinCapacity = minCapacity;
        }
    }
}
