using System;
using Random = UnityEngine.Random;

namespace Components.Managers
{
    public class Helper
    {
        public static int[] RandomDistinct(
            int min,
            int max,
            int amountDistinct
        )
        {
            int rangeSize = max - min;
            if (rangeSize < amountDistinct)
                throw new ArgumentException($"Range must contain at least {amountDistinct} values.");

            int[] values = new int[rangeSize];
            for (int i = 0; i < rangeSize; i++)
                values[i] = min + i;

            // Fisher–Yates shuffle (only first 3 needed)
            for (int i = 0; i < amountDistinct; i++)
            {
                int j = Random.Range(i, rangeSize);
                (values[i], values[j]) = (values[j], values[i]);
            }

            int[] distinctValues = new int[amountDistinct];
            for (int i = 0; i < amountDistinct; i++)
            {
                distinctValues[i] = values[i];
            }
            return distinctValues;
        }
    }
}
