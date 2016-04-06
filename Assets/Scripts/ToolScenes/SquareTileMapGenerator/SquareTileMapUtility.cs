using UnityEngine;
using System.Collections;

// TODO rename this class since a lot of its functions are not limited to the SquareTileMapGenerator.
public static class SquareTileMapUtility
{
    // Shuffles an array using the Fisher-Yates algorithm.
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length-1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }

}
