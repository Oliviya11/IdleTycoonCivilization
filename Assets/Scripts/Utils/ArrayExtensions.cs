using System;

namespace Assets.Scripts.Utils
{
    public static class ArrayExtensions
    {
        private static Random random = new Random();

        public static void Shuffle(this Array array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                var value = array.GetValue(k);
                array.SetValue(array.GetValue(n), k);
                array.SetValue(value, n);
            }
        }
    }
}
