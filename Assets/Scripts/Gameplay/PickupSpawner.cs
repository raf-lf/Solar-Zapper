using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : TableRoll
{
    [Header ("Table")]
    public GameObject[] pickupTable = new GameObject[6];

    [Header("Spawn Conditions")]
    public int maxPickups;
    public float spawnInterval;
    private float spawnIntervalTimer;
    public CircleCollider2D spawnArea;
    public GameObject spawnVfx;

    public void AttemptSpawn()
    {
        if (GameManager.BossHud.GetInteger("state") != 2)
        {
            if (Time.time > spawnIntervalTimer)
            {
                spawnIntervalTimer = Time.time + spawnInterval;

                int enemyCount = 0;

                Damageable[] damageablesActive = FindObjectsOfType<Damageable>();
                foreach (Damageable damageable in damageablesActive)
                {
                    if (damageable.gameObject.CompareTag("Enemy")) enemyCount++;
                }

                Debug.Log("Counted " + enemyCount + " enemies. Can spawn " + (maxPickups - enemyCount) + " more enemies.");

                if (enemyCount < maxPickups) Spawn();
            }
        }

    }

    public void Spawn()
    {
        GameObject spawnedObject = Instantiate(pickupTable[Roll()]);

        spawnArea.radius = FindObjectOfType<Arena>().currentArenaRadius;
        spawnedObject.transform.position = DefineSpawnPoint();

        GameObject effect = Instantiate(spawnVfx);
        effect.transform.position = spawnedObject.transform.position;

    }

    private Vector2 DefineSpawnPoint()
    {
        Vector2 spawnPoint = new Vector2(Random.Range(-spawnArea.radius, spawnArea.radius), Random.Range(-spawnArea.radius, spawnArea.radius));

        if (spawnArea.bounds.Contains(spawnPoint)) return spawnPoint;
        else return DefineSpawnPoint();

    }

    private void Update()
    {
        AttemptSpawn();
    }

}
