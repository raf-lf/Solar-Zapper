using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("Primary")]
    public Animator zapAnimator;
    public bool usingPrimary;

    [Header("Secondary")]
    public GameObject projectile;
    public Transform projectileOrigin;
    public int shotCost;
    public float shotBaseCooldown;
    public float shotBoostCooldown;
    [HideInInspector]
    public float shotCooldownTimer;

    [Header("Shield Restore")]
    public int layersRestored;
    public int restoreCost;
    public float restoreCooldown;
    [HideInInspector]
    public float restoreCooldownTimer;
    public GameObject restoreVfx;

    [Header("Blink")]
    public int blinkCost;
    public float blinkCooldown;
    [HideInInspector]
    public float blinkCooldownTimer;
    public GameObject[] blinkVfx = new GameObject[2];

    [Header("Revive")]
    public int reviveIFrames;
    public int reviveCost;
    public float reviveCooldown;
    [HideInInspector]
    public float reviveCooldownTimer;
    public GameObject reviveVfx;

    void Start()
    {
        GameManager.scriptActions = GetComponent<PlayerActions>();
    }

    void AttemptShot()
    {
        if (Time.time > shotCooldownTimer)
        {
            if (GameManager.scriptPlayer.buffActive[2])
            {
                Shot();
                shotCooldownTimer = Time.time + shotBoostCooldown;
            }
            else if (GameManager.scriptPlayer.score >= shotCost)
            {
                GameManager.scriptPlayer.ScoreChange(-shotCost);
                Shot();
                shotCooldownTimer = Time.time + shotBaseCooldown;

            }

        }

    }
    void Shot()
    {
        GameObject shot = Instantiate(projectile);
        shot.transform.position = projectileOrigin.position;
        shot.GetComponent<Projectile>().rotationZ = Calculations.GetRotationZToTarget(projectileOrigin.transform.position, GameManager.scriptCursor.gameObject.transform.position);
        shot.GetComponent<Projectile>().direction = Calculations.GetDirectionToTarget(projectileOrigin.transform.position, GameManager.scriptCursor.gameObject.transform.position);


    }

    void RestoreShields()
    {
        if (Time.time > restoreCooldownTimer)
        {
            if (GameManager.scriptPlayer.score >= restoreCost)
            {
                GameManager.scriptPlayer.ScoreChange(-restoreCost);
                restoreCooldownTimer = Time.time + restoreCooldown;

                GameManager.scriptPlayer.ShieldsChange(layersRestored);

                if (restoreVfx != null)
                {
                    GameObject effect = Instantiate(restoreVfx, transform);
                }
            }
        }

    }

    void Blink()
    {
        if (Time.time > blinkCooldownTimer)
        {
            if (GameManager.scriptPlayer.score >= blinkCost)
            {
                GameManager.scriptPlayer.ScoreChange(-blinkCost);
                blinkCooldownTimer = Time.time + blinkCooldown;

                GameManager.scriptCursor.BlinkToMouse();

                GameObject effect1 = Instantiate(blinkVfx[0]);
                effect1.transform.position = transform.position;

                transform.position = GameManager.scriptCursor.gameObject.transform.position;

                GameObject effect2 = Instantiate(blinkVfx[1], transform);
            }
        }
    }

    public bool AttemptRevive()
    {
        if (Time.time > reviveCooldownTimer && GameManager.scriptPlayer.score >= reviveCost) return true;
        else return false;

    }

    public void Revive()
    {
        GameManager.scriptPlayer.ScoreChange(-reviveCost);
        reviveCooldownTimer = Time.time + reviveCooldown;


        if (reviveVfx != null)
        {
            GameObject effect = Instantiate(reviveVfx,transform);
        }

        GameManager.scriptPlayer.Revive(reviveIFrames);
    }

    void Update()
    {
        if (GameManager.PlayerControls)
        {
            usingPrimary = Input.GetMouseButton(1);

            if (Input.GetKey(KeyCode.Q)) AttemptShot();
            if (Input.GetKeyDown(KeyCode.E)) RestoreShields();
            if (Input.GetKeyDown(KeyCode.W)) Blink();
        }
        else
        {
            usingPrimary = false;

        }

        zapAnimator.SetBool("active", usingPrimary);
        zapAnimator.SetBool("buffed", GameManager.scriptPlayer.buffActive[1]);
    }
}
