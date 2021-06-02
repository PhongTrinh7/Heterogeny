using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Immobilize")]
public class Immobilize : StatusEffect
{

    public override string Description()
    {
        return statusName + "\n" + description + "\nDuration: " + (timer - 1);
    }

    public override void OnApply(Unit target)
    {
        this.target = target;

        StatusEffect se = target.statuses.Find(x => x.statusName == statusName);

        //Make sure there is only one stack of stun on a unit at a time.
        if (se != null)
        {
            se.timer += duration;
            Destroy(this.gameObject);
        }
        else
        {
            target.immobilized = true;
            timer = duration;
            stacks = 1;
            target.statuses.Add(this);
            transform.parent = target.statusEffectContainer.transform;
        }
    }
    public override void Effect()
    {
        target.immobilized = true;
        timer--;
    }

    public override void ClearStatus()
    {
        target.immobilized = false;
        target.statuses.Remove(this);
        Destroy(this.gameObject);
    }
}
