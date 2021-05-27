using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable
{
    [Header("Score")]
    public int score;
    public int scoreMax = 100;

    [Header("Shields")]
    public int shieldsActive;
    public Animator[] shieldAnimators;
    public GameObject shieldDamageVfx;

    [Header("Buffs")]
    public bool[] buffActive = new bool[3];
    public float[] buffDuration = { 30, 20, 10 };
    public float[] buffDurationTimer = new float[3];


    private void Start()
    {
        GameManager.PlayerCharacter = gameObject;
        GameManager.scriptPlayer = GetComponent<Player>();
    }

    public override void Damage(int damageTaken)
    {
        if (dying == false && currentIFrames <= 0)
        {
            currentIFrames = iFrames;

            if (shieldsActive > 0)
            {
                if (damageTaken > 0)
                {
                    ShieldsChange(-1);
                    if (shieldDamageVfx != null)
                    {
                        GameObject effect = Instantiate(shieldDamageVfx);
                        effect.transform.position = transform.position;
                    }
                }
            }
            else
            {
                IFramesFeedback(true);

                HealthChange(-damageTaken);

                if (damageTaken > 0) DamageFeedback();

                if (hp <= 0) Death();
            }
        }
    }


    public override void Death()
    {
        GameManager.PlayerControls = false;
        GameManager.scriptMovement.HaltMove();

        if (GameManager.scriptActions.AttemptRevive()) GameManager.scriptActions.Revive();
        else
        {
            dying = true;

            if (deathVfx != null)
            {
                GameObject effect = Instantiate(deathVfx);
                effect.transform.position = transform.position;
            }

            GameManager.scriptMovement.HaltMove();

            anim.Play("death");

            Invoke(nameof(GameOverOverlay), 1);
            Invoke(nameof(GameOverReload), 4);
        }

    }
    
    public void Revive(int reviveIFrames)
    {
        GameManager.AnimationOverlay("respawn");
        anim.Play("respawn");

        GameManager.PlayerControls = true;
        GameManager.scriptMovement.HaltMove();

        ShieldsChange(shieldAnimators.Length);
        HealthChange(hpMax / 4);
        currentIFrames = reviveIFrames;

    }

    private void GameOverOverlay()
    {
        GameManager.ChangeOverlay(2);
    }
    private void GameOverReload()
    {
        GameManager.ReloadScene();
    }


    public void ScoreChange(int scoreChange)
    {
        if (scoreChange > 0) GameManager.scriptHud.scoreFill.GetComponent<Animator>().Play("valueUp");
        else if (scoreChange < 0) GameManager.scriptHud.scoreFill.GetComponent<Animator>().Play("valueDown");

        score = Mathf.Clamp(score + scoreChange, 0, scoreMax);

    }

    public override void HealthChange(int value)
    {
        hp = Mathf.Clamp(hp + value, 0, hpMax);

        if (value > 0) GameManager.scriptHud.healthFill.GetComponent<Animator>().Play("valueUp");
        else if (value < 0)
        {
            GameManager.scriptHud.healthFill.GetComponent<Animator>().Play("valueDown");
            DamageFeedback();
        }

        if (hp <= 0) Death();

    }

    public void ShieldsChange(int value)
    {
        shieldsActive = Mathf.Clamp(shieldsActive + value, 0, GameManager.scriptPlayer.shieldAnimators.Length);

    }

    public void ActivateBuff(int buffId)
    {
        buffActive[buffId] = true;
        buffDurationTimer[buffId] = Time.time + buffDuration[buffId];

    }

    protected override void Update()
    {
        base.Update();

        UpdateShields();
        UpdateBuffs();

    }

    private void UpdateShields()
    {
        for (int i = 0; i < shieldAnimators.Length; i++)
        {
            if (i < shieldsActive)
            {
                shieldAnimators[i].SetBool("active", true);
                GameManager.scriptHud.shieldChargeIcon[i].GetComponent<Animator>().SetBool("active", true);
            }
            else
            {
                shieldAnimators[i].SetBool("active", false);
                GameManager.scriptHud.shieldChargeIcon[i].GetComponent<Animator>().SetBool("active", false);
            }
        }
    }

    private void UpdateBuffs()
    {
        for (int i = 0; i < buffActive.Length; i++)
        {
            if(buffActive[i] && Time.time > buffDurationTimer[i]) buffActive[i] = false;

        }
    }
}
