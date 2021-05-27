using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed;
    public Vector3 offset;
    public GameObject target;

    private void Start()
    {
        if (GameManager.PlayerCharacter != null) target = GameManager.PlayerCharacter;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, speed);
    }
}
