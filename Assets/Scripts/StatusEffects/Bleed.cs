using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bleed")]
public class Bleed : StatusEffect
{
    public override void OnApply(Unit target)
    {
        this.target = target;

        StatusEffect se = target.statuses.Find(x => x.statusName == statusName);

        //Make sure there is only one stack of stun on a unit at a time.
        if (se != null)
        {
            se.stacks++;
            se.timer += duration;
            Destroy(this.gameObject);
        }
        else
        {
            timer = duration;
            stacks = 1;
            target.statuses.Add(this);
            transform.parent = target.statusEffectContainer.transform;
        }
    }

    public override void Effect()
    {
        target.TakeDamage(stacks * damage, Element.NONE);
        timer--;
    }
}
