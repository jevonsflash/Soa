using System;
using System.Collections.Generic;
using System.Text;

namespace Soa.Helpers
{
    public class CommonHelper
    {
        public static T GetRandom<T>(IList<T> a)
        {
            Random rnd = new Random();
            int index = rnd.Next(a.Count);
            return a[index];
        }


        public static IList<T> GetRandoms<T>(IList<T> a, int maxNumber, bool isAmountRandom = true)
        {
            Random rnd = new Random();
            int number = isAmountRandom ? rnd.Next(Math.Min(a.Count, maxNumber)) : maxNumber;
            var tags = new List<T>();
            for (int i = 0; i < number; i++)
            {
                var current = GetRandom(a);
                if (!tags.Contains(current))
                {
                    tags.Add(current);
                }
            }
            return tags;
        }
    }
}
