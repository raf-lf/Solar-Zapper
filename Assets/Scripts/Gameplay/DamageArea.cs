using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public int damage;

    protected virtual void Damage(Damageable target)
    {
       target.Damage(damage);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Damageable>()) Damage(collision.gameObject.GetComponentInParent<Damageable>());

    }


}
