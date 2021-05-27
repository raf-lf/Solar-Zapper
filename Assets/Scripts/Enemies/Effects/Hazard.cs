using System.Collections.Generic;
using UnityEngine;

public class Hazard : DamageArea
{
    public float duration;
    private float durationTimer;

    private bool off;

    private void Awake()
    {
        durationTimer = Time.time + duration;
    }

    private void Update()
    {
        if (Time.time > durationTimer && off == false)
        {
            off = true;
            GetComponent<Animator>().Play("inactive");
            Destroy(gameObject, 3);
        }
    }

}
