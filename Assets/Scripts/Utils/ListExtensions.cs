using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using SystemRandom = System.Random;

namespace Assets.Scripts.Utils
{
    public static class ListExtensions
    {
        private static SystemRandom random = new SystemRandom();

        public static T GetRandomElement<T>(this List<T> list)
        {
            int index = Random.Range(0, list.Count);
            return list[index];
        }

        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
