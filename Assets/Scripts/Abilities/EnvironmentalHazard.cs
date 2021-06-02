using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentalHazard : Hitbox
{
    public string localName;
    public string description;

    public LayerMask environmentalHazard;

    public int duration;
    protected int durationTimer;

    void Start()
    {
        durationTimer = duration;
    }

    public virtual void ModifyBase(int damage, int duration)
    {
        this.damage = damage;
        this.duration = duration;
    }

    public void DurationCountDown()
    {
        duration--;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
