using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public float baseCursorSpeed;
    public float boostedCursorSpeed;
    [HideInInspector]
    public float currentCursorSpeed;

    public LineRenderer targetLine;

    private void Start()
    {
        GameManager.scriptCursor = GetComponent<Cursor>();
    }

    public void BlinkToMouse()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        transform.position = mouse;

    }

    private void FixedUpdate()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        
        transform.position = Vector2.MoveTowards(transform.position, mouse, currentCursorSpeed);

    }
    private void Update()
    {

        targetLine.SetPosition(0, GameManager.PlayerCharacter.transform.position);
        targetLine.SetPosition(1, transform.position);

        if (GameManager.scriptPlayer.buffActive[0])
        {
            currentCursorSpeed = boostedCursorSpeed;
        }
        else
        {
            currentCursorSpeed = baseCursorSpeed;
        }
    }
}
