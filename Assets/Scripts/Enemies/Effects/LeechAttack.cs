using System.Collections.Generic;
using UnityEngine;

public class LeechAttack : DamageArea
{
    public int scoreDrain;

    protected override void Damage(Damageable target)
    {
        base.Damage(target);
        target.gameObject.GetComponent<Player>().ScoreChange(-scoreDrain);
    }


}
