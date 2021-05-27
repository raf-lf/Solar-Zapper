using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed;
    public float boostedMoveSpeed;
    [HideInInspector]
    public float currentMoveSpeed;
    public float deacceleration;

    public Vector2 velocity;

    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public float rotationZ;

    private void Start()
    {
        GameManager.scriptMovement = GetComponent<PlayerMovement>();
    }

    public void Move(bool onMove)
    {
        if (onMove)
        {
            if (Vector2.Distance(transform.position, GameManager.scriptCursor.gameObject.transform.position) > .4f)
            {
                velocity = direction * currentMoveSpeed;
            }
        }
        else
        {
            velocity.x *= deacceleration;
            velocity.y *= deacceleration;
        }
    }
    public void HaltMove()
    {
        velocity = Vector2.zero;
        GameManager.scriptPlayer.rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (GameManager.PlayerControls)
        {
            UpdateSpeed();
            UpdateRotation();

            if (Input.GetMouseButton(0)) Move(true);
            else Move(false);
        }
        else Move(false);
        
    }

    private void UpdateRotation()
    {
        Vector3 cursorTarget = GameManager.scriptCursor.gameObject.transform.position;

        rotationZ = Calculations.GetRotationZToTarget(transform.position, cursorTarget);
        direction = Calculations.GetDirectionToTarget(transform.position, cursorTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationZ), GameManager.scriptCursor.currentCursorSpeed);


    }
    private void UpdateSpeed()
    {
        if (GameManager.scriptPlayer.buffActive[0])
        {
            currentMoveSpeed = boostedMoveSpeed;
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
        }


        GameManager.scriptPlayer.rb.velocity = velocity;

    }
}
