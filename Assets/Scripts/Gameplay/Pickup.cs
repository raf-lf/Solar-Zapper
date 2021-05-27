using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 20);
    }

    public virtual void Execute(GameObject picker)
    {
        GetComponent<Animator>().Play("pickup");
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Execute(collision.gameObject);
        }
    }
}
