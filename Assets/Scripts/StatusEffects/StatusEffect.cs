using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusEffect : MonoBehaviour
{
    public string statusName;
    public string description;
    public Image icon;
    public int stacks = 0;
    public int damage;
    public int duration;
    public int timer;
    protected Unit target;

    public virtual string Description()
    {
        return statusName + "\n" + description + "\nDuration: " + timer;
    }

    public virtual void OnApply(Unit target)
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
            timer = duration;
            stacks = 1;
            target.statuses.Add(this);
            transform.parent = target.statusEffectContainer.transform;
        }
    }

    public abstract void Effect();

    public virtual bool CheckTimer()
    {
        if (timer == 0)
        {
            ClearStatus();
            return true;
        }
        return false;
    }

    public virtual void ClearStatus()
    {
        target.statuses.Remove(this);
        Destroy(this.gameObject);
    }
}
