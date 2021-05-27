using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Header("Combat")]
    public int hp;
    public int hpMax;
    public GameObject damageVfx;
    public GameObject deathVfx;
    public bool dying;
    public bool immuneToArenaDamage;

    public Collider2D damageReceiver;
    public int iFrames = 30;
    public int currentIFrames;

    [Header("Score Drop")]
    public int scoreDrop;
    public GameObject scorePickupObject;

    [Header("Components")]
    public Animator anim;
    public Rigidbody2D rb;


    public virtual void Damage(int damageTaken)
    {
        if (dying == false && currentIFrames <= 0)
        {
            currentIFrames = iFrames;
            IFramesFeedback(true);

            HealthChange(-damageTaken);

            if (damageTaken > 0) DamageFeedback();

            if (hp <= 0) Death();
        }
    }

    public virtual void HealthChange(int value)
    {
        hp = Mathf.Clamp(hp + value, 0, hpMax);

        if (GameManager.scriptCanvas.bossScript == GetComponent<Damageable>())
        {
            if (value > 0) GameManager.scriptCanvas.bossHpFill.GetComponent<Animator>().Play("valueUp");
            else if (value < 0) GameManager.scriptCanvas.bossHpFill.GetComponent<Animator>().Play("valueDown");
        }
        if (value < 0) DamageFeedback();

        if (hp <= 0) Death();

    }

    protected virtual void DamageFeedback()
    {
        if (damageVfx != null)
        {
            GameObject effect = Instantiate(damageVfx);
            effect.transform.position = transform.position;
        }

    }

    public virtual void Death()
    {
        dying = true;

        if (deathVfx != null)
        {
            GameObject effect = Instantiate(deathVfx);
            effect.transform.position = transform.position;
        }

        anim.Play("death");

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach( Collider2D collider in colliders)
        {
            collider.enabled = false;

        }

        DropScorePickup();

        if (GetComponent<ItemDrop>()) GetComponent<ItemDrop>().AttemptDrop();

        Destroy(gameObject, 5);
    }

    public virtual void DropScorePickup()
    {
        if (scoreDrop > 0)
        {
            GameObject droppedScoreItem= Instantiate(scorePickupObject);
            droppedScoreItem.GetComponent<PickupAtribute>().atributeChange = scoreDrop;
            droppedScoreItem.transform.position = transform.position;

        }

    }
    protected void IFramesFeedback(bool entering)
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            if (entering) sprite.color = new Color(255,0,0,100);
            else sprite.color = new Color(255, 255, 255, 255);
        }

    }

    private void OutsideArenaDamage()
    {
        if (Vector2.Distance(transform.position, GameManager.arena.gameObject.transform.position) > GameManager.arena.currentArenaRadius)
        {
            Damage(GameManager.arena.outsideDamage);
        }
    }

    protected virtual void Update()
    {
        if (currentIFrames > 0)
        {
            damageReceiver.enabled = false;
            currentIFrames--;
            if (currentIFrames == 0) IFramesFeedback(false);

        }
        else damageReceiver.enabled = true;

        if (immuneToArenaDamage == false) OutsideArenaDamage();

    }


}
