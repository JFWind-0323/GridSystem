using System.Collections.Generic;
using UnityEngine;
public static class LinearTablesTool
{
    public static void InitDictFromList<T>(List<T> list, out Dictionary<int, T> dict)
    {
        dict = new Dictionary<int, T>();
        for (int i = 0; i < list.Count; i++)
        {
            if (!dict.ContainsKey(i))
            {
                dict.Add(i, list[i]);
            }
        }
    }

    public static void PrintDict<Tkey, TValue>(this Dictionary<Tkey, TValue> dict)
    {
        foreach (var item in dict)
        {
            Debug.Log(item.Key + " : " + item.Value);
        }
    }
    public static T[] ToArray<T>(this T[,] array2D)
    {
        int row = array2D.GetLength(0);
        int col = array2D.GetLength(1);
        int length = row * col;
        T[] array = new T[length];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                array[i * col + j] = array2D[i, j];
            }
        }
        return array;
    }

   public static T[,] ToArray2D<T>(this T[] array, int width, int height)
    {
        T[,] array2D = new T[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                array2D[i, j] = array[i * height + j];
            }
        }
        return array2D;
    }
}
