using System;

namespace Project.Scripts.Core.ValueObjects
{
    [Serializable]
    public class CityInfo
    {
        public int cash;
        public int day;
        public float food;
        public FlowControl population;
        public FlowControl jobs;


        public bool SpendCash(int amount)
        {
            if (cash < amount) return false;
            cash -= amount;
            return true;
        }
    }
}