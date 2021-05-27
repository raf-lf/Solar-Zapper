using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : TableRoll
{
    [Header ("Table")]
    public GameObject[] enemyTable = new GameObject[3];

    [Header("Spawn Conditions")]
    public int maxEnemies;
    public float spawnInterval;
    private float spawnIntervalTimer;
    public CircleCollider2D spawnArea;
    public GameObject spawnVfx;

    public void AttemptSpawn()
    {
        if (GameManager.BossHud.GetInteger("state") == 1)
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

                Debug.Log("Counted " + enemyCount + " enemies. Can spawn " + (maxEnemies - enemyCount) + " more enemies.");

                if (enemyCount < maxEnemies) Spawn();
            }
        }

    }

    public void Spawn()
    {
        GameObject spawnedObject = Instantiate(enemyTable[Roll()]);

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
