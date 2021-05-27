using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Phases")]
    public int phase;
    public int[] restDuration = { 5, 3, 1 };
    private Arena arena;
    public float[] arenaRadius = { 20, 15, 10 };

    [Header("Minions")]
    public int[] minionSpawned = { 5, 3, 1 };
    public int[] minionsMax = { 3, 3, 3 };
    public int[] minionSpawnInterval = { 10, 10, 5 };
    private float minionSpawnIntervalTimer;

    [Header("Attacks")]
    public bool firing;
    public float[] laserChargeTime = { 3, 2, 1 };
    public float[] laserFireTime = { 3, 3, 3 };
    public float[] laserTurnSpeed = { .05f, .04f, .03f };
    public float[] novaChargeTime = { 2, 1, 0.5f };
    public float[] novaFireTime = { 4, 4, 4 };
    public float[] spawnTime = { 4, 4, 4 };
    public float[] spawnInterval = { 2, .1f, .5f };
    private float spawnIntervalTimer;
    public GameObject spawnedObject;
    public Transform spawnOrigin;

    private void Awake()
    {
        BeginRest();
        arena = FindObjectOfType<Arena>();
        minionSpawnIntervalTimer = Time.time + minionSpawnInterval[phase];
        target = GameManager.PlayerCharacter;
    }

    private void RollAttack()
    {
        float roll = Random.Range(1, 100);

        switch (phase)
        {
            case 0: BeginLaser();
                if (roll < 40) BeginLaser();
                else if (roll < 60) BeginNova();
                else BeginSpawn();
                break;
            case 1:
                {
                    if (roll < 30) BeginLaser();
                    else if (roll < 60) BeginNova();
                    else BeginSpawn();
                    break;
                }
            case 2:
                {
                    if (roll < 40) BeginLaser();
                    else if (roll < 80) BeginNova();
                    else BeginSpawn();
                    break;
                }
            default: BeginRest();
                break;
        }

    }

    void BeginRest()
    {
        currentTurnSpeed = turnSpeed;
        firing = false;
        state = 0;
        stateDurationTimer = Time.time + restDuration[phase];
    }

    void BeginLaser()
    {
        state = 1;
        stateDurationTimer = Time.time + laserChargeTime[phase];

    }
    void FireLaser()
    {
        currentTurnSpeed = laserTurnSpeed[phase];
        firing = true;
        stateDurationTimer = Time.time + laserFireTime[phase];

    }

    void BeginNova()
    {
        state = 2;
        stateDurationTimer = Time.time + novaChargeTime[phase];

    }
    void FireNova()
    {
        firing = true;
        stateDurationTimer = Time.time + novaFireTime[phase];

    }

    void BeginSpawn()
    {
        state = 3;
        stateDurationTimer = Time.time + spawnTime[phase];
        spawnIntervalTimer = Time.time + spawnInterval[phase];

    }

    void SpawnLeeches()
    {
        if (Time.time > spawnIntervalTimer)
        {
            spawnIntervalTimer = Time.time + spawnInterval[phase];
            GameObject spawnedMinion = Instantiate(spawnedObject);

            spawnedMinion.transform.position = spawnOrigin.position;
            spawnedMinion.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + 180);

        }

    }

    public void BattleEnd()
    {
    }

    public override void Death()
    {
        dying = true;

        if (deathVfx != null)
        {
            GameObject effect = Instantiate(deathVfx);
            effect.transform.position = transform.position;
        }

        anim.Play("death");

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;

        }

        DropScorePickup();

        if (GetComponent<ItemDrop>()) GetComponent<ItemDrop>().AttemptDrop();

        Destroy(gameObject, 20);
    }

    protected override void Update()
    {
        base.Update();

        anim.SetInteger("state", state);
        anim.SetBool("firing", firing);

        UpdatePhase();
        PhaseChanges();

        if (dying == false)
        {
            switch (state)
            {
                //Rest
                case 0:
                    if (target != null) FaceDirection(target.transform.position);
                    if (Time.time > stateDurationTimer) RollAttack();
                    break;
                //Laser
                case 1:
                    if (target != null) FaceDirection(target.transform.position);
                    if (Time.time > stateDurationTimer)
                    {
                        if (firing == false) FireLaser();
                        else BeginRest();

                    }
                    break;
                //Nova
                case 2:
                    if (Time.time > stateDurationTimer)
                    {
                        if (firing == false) FireNova();
                        else BeginRest();
                    }
                    break;
                //Spawn
                case 3:
                    if (Time.time > stateDurationTimer) BeginRest();
                    else SpawnLeeches();
                    break;
            }
        }
    }

    private void UpdatePhase()
    {
        if (hp <= hpMax * .33f) phase = 2;
        else if (hp <= hpMax * .66f) phase = 1;
        else phase = 0;
    }

    private void PhaseChanges()
    {
        arena.arenaRadius = arenaRadius[phase];
    }
}
