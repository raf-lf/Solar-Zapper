using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAtribute : Pickup
{
    public enum atribute
    {
        score, hp, shield

    }
    public atribute atributeType;

    public int atributeChange;

    public override void Execute(GameObject picker)
    {
        base.Execute(picker);
        switch (atributeType)
        {
            case atribute.score:
                picker.GetComponent<Player>().ScoreChange(atributeChange);
                break;
            case atribute.hp:
                picker.GetComponent<Player>().HealthChange(atributeChange);
                break;
            case atribute.shield:
                picker.GetComponent<Player>().ShieldsChange(atributeChange);
                break;

        }
    }

}
