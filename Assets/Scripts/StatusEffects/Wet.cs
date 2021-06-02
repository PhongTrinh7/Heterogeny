using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wet")]
public class Wet : StatusEffect
{
    public Stun stun;

    public override void OnApply(Unit target)
    {
        this.target = target;

        StatusEffect se = target.statuses.Find(x => x.statusName == "Shock");

        //Make sure there is only one stack of wet on a unit at a time.
        if (se != null)
        {
            se.ClearStatus();
            //target.statuses.Remove(se);
            //Destroy(se.gameObject);
            target.ApplyStatus(Object.Instantiate(stun));
            Destroy(gameObject);
        }
        else
        {
            base.OnApply(target);
        }
    }

    public override void Effect()
    {
        target.moveCost = 2;
        timer--;
    }

    public override void ClearStatus()
    {
        target.moveCost = 1;
        target.statuses.Remove(this);
        Destroy(this.gameObject);
    }
}
