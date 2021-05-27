using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRoll : MonoBehaviour
{
    [Header("Table Roll")]
    public int[] indexWeight = new int[1];
    public int[] indexWeightMaxRange = new int[1];
    private int totalWeight;

    public int Roll()
    {
        ConfigureWeight();

        int returnedIndex = 0;
        int roll = Random.Range(0, totalWeight + 1);
        Debug.Log("Rolled " + roll);

        for (int i = 0; i < indexWeight.Length; i++)
        {
            if (i == 0)
            {
                if (roll <= indexWeight[0]) returnedIndex = 0;
            }
            else
            {
                if (roll > indexWeightMaxRange[i - 1] && roll <= indexWeightMaxRange[i]) returnedIndex = i;
            }
        }

        return returnedIndex;

    }

    public void ConfigureWeight()
    {
        totalWeight = 0;

        for (int i = 0; i < indexWeight.Length; i++)
        {
            totalWeight += indexWeight[i];
            indexWeightMaxRange[i] = totalWeight;
            if (i == 0) Debug.Log("Index " + i + " range is " + 0 + " - " + indexWeightMaxRange[i]);
            else Debug.Log("Index " + i + " range is " + indexWeightMaxRange[i-1]+1 + " - " + indexWeightMaxRange[i]);

        }

    }

}
