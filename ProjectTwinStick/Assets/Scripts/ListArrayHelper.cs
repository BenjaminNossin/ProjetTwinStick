using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using System;

namespace HelperPSR.ListArray
{
    public static class ListArrayHelper
    {
        public static void ShuffleArray<T>(this System.Random _rng, T[] _array)
        {
            int n = _array.Length;
            while (n > 1)
            {
                int k = _rng.Next(n--);
                (_array[n], _array[k]) = (_array[k], _array[n]);
            }
        }

        public static void ShuffleList<T>(this System.Random _rng, List<T> _list)
        {
            int n = _list.Count;
            while (n > 1)
            {
                int k = _rng.Next(n--);
                (_list[n], _list[k]) = (_list[k], _list[n]);
            }
        }

        public static T[] Clone<T>(this T[] array)
        {
            T[] returnArray = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                returnArray[i] = array[i];
            }

            return returnArray;
        }

        public static List<T> Clone<T>(this List<T> list)
        {
            List<T> returnArray = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                returnArray.Add(list[i]);
                ;
            }

            return returnArray;
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            int randIndex = Random.Range(0, list.Count);
            return list[randIndex];
        }

        public static void RemoveRandomElement<T>(this List<T> list)
        {
            int randIndex = Random.Range(0, list.Count);
            list.RemoveAt(randIndex);
        }

        public static void SuffleList<T>(this List<T> list)
        {
            int currentCount = list.Count;
            while (currentCount > 0)
            {
                int randIndex = Random.Range(0, currentCount);
                list.Add(list[randIndex]);
                list.RemoveAt(randIndex);
                currentCount--;
            }
        }

        public static T CloneClass<T>(this T currentClass) where T : class
        {
            var fieldValues = typeof(T)
                .GetFields()
                .Select(field => field.GetValue(currentClass))
                .ToList();
            T returnClass = null;
            var returnValues = typeof(T)
                .GetFields()
                .Select(field => field.GetValue(currentClass))
                .ToList();
            for (int i = 0; i < fieldValues.Count; i++)
            {
                returnValues[i] = fieldValues[i];
            }

            return returnClass;
        }

        public static void RemoveRandomItem<T>(this List<T> list)
        {
            try
            {
                int index = Random.Range(0, list.Count);
                list.RemoveAt(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void ShuffleList<T>(this List<T> list)
        {
            System.Random rng = new System.Random();

            int n = list.Count;

            while (n > 1)
            {
                n--;
                int index = rng.Next(n + 1);

                (list[index], list[n]) = (list[n], list[index]);
            }
        }

        public static T CopyClass<T>(this T original) where T : class, new()
        {
            Type type = typeof(T);
            T returnClass = new T();
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic || field.IsLiteral)
                    continue;
                field.SetValue(returnClass, field.GetValue(original));
            }

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                property.SetValue(returnClass, property.GetValue(original));
            }

            return returnClass;
        }
    }
}