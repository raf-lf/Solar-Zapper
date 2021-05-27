using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [Header("Projectile")]
    public GameObject projectile;
    public Transform projectileOrigin;
    public float shotCooldown;
    private float shotCooldownTimer;

    [Header("Wander")]
    private Vector2 wanderDestination;


    private void RestStart()
    {
        Move(false);
        stateDurationTimer = Time.time + stateDuration[0];
        state = 0;
    }

    private void WanderStart()
    {
        stateDurationTimer = Time.time + stateDuration[1];
        state = 1;

        wanderDestination = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        moveDirection = Calculations.GetDirectionToTarget(transform.position, wanderDestination);
    }


    private void AttackStart()
    {
        Move(false);
        stateDurationTimer = Time.time + stateDuration[2];
        state = 2;
    }

    private void Attack()
    {
        FaceDirection(target.transform.position);
        
        if(Time.time > shotCooldownTimer)
        {
            shotCooldownTimer = Time.time + shotCooldown;
            Shot();
        }
    }

    private void Shot()
    {
        GameObject shot = Instantiate(projectile);
        shot.transform.position = projectileOrigin.transform.position;

        shot.GetComponent<Projectile>().rotationZ = Calculations.GetRotationZToTarget(projectileOrigin.transform.position, target.transform.position);
        shot.GetComponent<Projectile>().direction = Calculations.GetDirectionToTarget(projectileOrigin.transform.position, target.transform.position);

    }

    protected override void Update()
    {
        base.Update();

        if (Detecting() && target != null && dying == false)
        {
            if (state == 0)
            {
                if (Time.time > stateDurationTimer)
                {
                    WanderStart();
                }
                else
                {
                    //do nothing
                }

            }

            else if (state == 1)
            {
                if (Time.time > stateDurationTimer)
                {
                    AttackStart();
                }

                else
                {
                    Move(true);
                    FaceDirection(wanderDestination);
                }
            }

            else if (state == 2)
            {
                if (Time.time > stateDurationTimer)
                {
                    RestStart();

                }
                else Attack();

            }
        }
        else Move(false);
        
    }
}
