using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    [Header("Movement")]
    public float moveSpeed;
    protected float currentMoveSpeed;

    public float turnSpeed = .05f;
    protected float currentTurnSpeed;
    [HideInInspector]
    public Vector2 moveDirection;

    [Header("Detection")]
    public float detectionRadius;
    public LayerMask detectionMask;
    public GameObject target;

    [Header("Behavior")]
    public int state;
    public float[] stateDuration = new float[3];
    [HideInInspector]
    public float stateDurationTimer;


    protected virtual void Start()
    {
        currentMoveSpeed = moveSpeed;
        currentTurnSpeed = turnSpeed;

    }

    public void FaceDirection(Vector3 point)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Calculations.GetRotationZToTarget(transform.position, point)), currentTurnSpeed);
    }

    public void Move(bool moving)
    {
        if (moving) rb.velocity = moveDirection * currentMoveSpeed;
        else rb.velocity = Vector2.zero;
    }

    public bool Detecting()
    {
        if (Physics2D.CircleCast(transform.position, detectionRadius, Vector2.zero,0, detectionMask))
        {
            target = Physics2D.CircleCast(transform.position, detectionRadius, Vector2.zero, 0, detectionMask).collider.gameObject;
        //    Debug.Log(gameObject.name + "'s target is " + target);
            return true;
        }
        else return false;
    }

}
