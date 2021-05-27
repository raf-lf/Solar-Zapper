using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [Header("Arena Damage")]
    public int outsideDamage;
    public float damageInterval;
    public float arenaRadius;
    public float currentArenaRadius = 50;
    public float arenaRadiusSpeed = .1f;
    public ParticleSystem[] particles;

    [Header("Boss Battle")]
    public GameObject boss;
    public GameObject summonedBoss;
    public Transform bossSpawn;
    public AudioClip bossMusic;



    private void Start()
    {
        GameManager.arena = GetComponent <Arena>();
    }

    public void SummonBoss()
    {
        summonedBoss = Instantiate(boss);
        summonedBoss.transform.position = bossSpawn.position;

        GameManager.BossHud.SetInteger("state", 2);
        GameManager.scriptCanvas.bossScript = summonedBoss.GetComponent<Damageable>();
        GameManager.scriptCanvas.bossName.text = boss.name;
        GameManager.musicSource.clip = bossMusic;
        GameManager.musicSource.Play();

    }

    public void BossDefeatCheck()
    {
        if (summonedBoss != null)
        {
            if(summonedBoss.GetComponent<Damageable>().dying && GameManager.BossHud.GetInteger("state")== 2)
            {
                arenaRadius = 100;
                GameManager.musicSource.volume = 0;
                GameManager.BossHud.SetInteger("state", 3);
                FindObjectOfType<CameraFollow>().target = summonedBoss;
                GameManager.PlayerControls = false;
                GameManager.scriptPlayer.currentIFrames = 1000;
                GameManager.timerPaused = true;
                Invoke(nameof(FadeOut), 5);
                Invoke(nameof(LoadEndScene),7);
            }
        }

    }

    private void FadeOut()
    {
        GameManager.Overlay.SetInteger("state", 1);

    }
    private void LoadEndScene()
    {
        SceneManager.LoadScene("End");

    }


    private void UpdateArenaRadius()
    {
        if(currentArenaRadius != arenaRadius)
        {
            if (currentArenaRadius > arenaRadius) currentArenaRadius = Mathf.Clamp(currentArenaRadius - arenaRadiusSpeed, arenaRadius, currentArenaRadius);
            else currentArenaRadius = Mathf.Clamp(currentArenaRadius + arenaRadiusSpeed, currentArenaRadius, arenaRadius);

        }

    }

    private void UpdateParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            ParticleSystem.ShapeModule shapeModule = particle.shape;
            shapeModule.radius = currentArenaRadius+1;
        }

    }

    void Update()
    {
        UpdateArenaRadius();
        UpdateParticles();
        BossDefeatCheck();
    }
}
