using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    [Header("Projectile")]
    public GameObject projectile;
    public Transform projectileOrigin;

    [Header("Hazard")]
    public GameObject hazard;
    public float hazardCooldown;
    private float hazardCooldownTimer;

    [Header("Trajectory")]
    public Vector2[] patrolPoints;
    public Vector2 anchorPosition;
    public int currentTargetPoint = 0;
    public float patrolPointDetectionRange;

    protected override void Start()
    {
        base.Start();

        anchorPosition = transform.position;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] += anchorPosition;

        }
    }

    private void HazardSpawn()
    {
        if (Time.time > hazardCooldownTimer)
        {
            hazardCooldownTimer = Time.time + hazardCooldown;

            GameObject hazardCreated = Instantiate(hazard);
            hazardCreated.transform.position = projectileOrigin.transform.position + new Vector3(Random.Range(-1,1), Random.Range(-1, 1));
            hazardCreated.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360));
        }
    }

    private void DefensiveShot()
    {
            GameObject shot = Instantiate(projectile);
            shot.transform.position = projectileOrigin.transform.position;

            Vector3 randomTarget = transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));

            shot.GetComponent<Projectile>().rotationZ = Calculations.GetRotationZToTarget(projectileOrigin.transform.position, randomTarget);
            shot.GetComponent<Projectile>().direction = Calculations.GetDirectionToTarget(projectileOrigin.transform.position, randomTarget);


    }

    public override void Damage(int damageTaken)
    {
        if (currentIFrames <= 0 && dying == false)
        {
            DefensiveShot();

        }

        base.Damage(damageTaken);
        
    }


    private void Patrol()
    {
        FaceDirection(patrolPoints[currentTargetPoint]);
        rb.transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentTargetPoint], currentMoveSpeed);

    }

    private void CheckArrival()
    {
        if(Vector2.Distance(transform.position, patrolPoints[currentTargetPoint]) <= patrolPointDetectionRange)
        {
           // Debug.Log(gameObject.name + " arrived at patrol point "+ currentTargetPoint);

            if (currentTargetPoint +1 >= patrolPoints.Length) currentTargetPoint = 0;
            else currentTargetPoint++;

        }

    }

    protected override void Update()
    {
        base.Update();

        if (dying == false)
        {
            HazardSpawn();
            CheckArrival();
            Patrol();

        }
        else Move(false);

    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 point in patrolPoints)
        {
            Gizmos.DrawSphere(point + transform.position, patrolPointDetectionRange);
        }
    }
}
