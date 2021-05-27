using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : TableRoll
{
    [Header("Table")]
    public GameObject[] dropTable = new GameObject[6];

    [Header("Drop Conditions")]
    public int individualDrops = 1;
    [Range(0,100)]
    public float dropChance;

    public void AttemptDrop()
    {
        for (int i = individualDrops; i > 0; i--)
        {
            float dropAttempt = Random.Range(0, 100);
            if (dropAttempt <= dropChance) DropItem();

        }

    }

    public void DropItem()
    {
        GameObject item = Instantiate(dropTable[Roll()]);
        item.transform.position = transform.position + new Vector3(Random.Range(-1,1), Random.Range(-1, 1));

    }



}
