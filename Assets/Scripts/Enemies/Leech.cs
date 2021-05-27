using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leech : Enemy
{
    [Header("Specific")]
    [Space(30)]
    public float contactDistance;

    private void Approach()
    {
        moveDirection = Calculations.GetDirectionToTarget(transform.position, target.transform.position);
        FaceDirection(target.transform.position);

        if(Vector2.Distance(transform.position, target.transform.position) > contactDistance) rb.velocity = moveDirection * currentMoveSpeed;

    }

    protected override void Update()
    {
        base.Update();

        if(dying == false) if (Detecting() && target != null) Approach();
        
    }
}
