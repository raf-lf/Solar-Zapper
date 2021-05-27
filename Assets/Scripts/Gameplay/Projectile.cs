using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;

    public GameObject impactEffect;

    public float autoDestroyTimer;
    private float autoDestroyTargetTime;

    [HideInInspector]
    public float rotationZ;
    [HideInInspector]
    public Vector2 direction;

    private AudioSource audioSource;


    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        GetComponent<Rigidbody2D>().velocity = direction * speed;

        if (autoDestroyTimer != 0) autoDestroyTargetTime = Time.time + autoDestroyTimer;


        if (GetComponent<AudioSource>())
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.transform.parent = null;
            Destroy(audioSource, 10);
        }
    }

    private void Impact()
    {
        GetComponent<Animator>().Play("impact");
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        Destroy(gameObject, 5);

        if (impactEffect != null)
        {
            GameObject impact =  Instantiate(impactEffect);
            impact.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Damageable>())
        {
            Impact();
            collision.gameObject.GetComponentInParent<Damageable>().Damage(damage);

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Impact();
    }

    private void FixedUpdate()
    {
        if (Time.time >= autoDestroyTargetTime && autoDestroyTimer != 0)
        {
            GetComponent<Animator>().Play("fade");
            Destroy(gameObject, 5);
        }
    }
}
