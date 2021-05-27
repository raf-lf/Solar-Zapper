using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : Pickup
{
    public int buffId;

    public override void Execute(GameObject picker)
    {
        base.Execute(picker);
        picker.GetComponent<Player>().ActivateBuff(buffId);
    }

}
