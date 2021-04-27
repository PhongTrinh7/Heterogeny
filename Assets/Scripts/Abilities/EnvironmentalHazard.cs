using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentalHazard : MonoBehaviour
{
    public string localName;
    public string description;
    public int damage;
    //public List<StatusEffect> effects;

    public LayerMask environmentalHazard;

    public int duration;
    protected int durationTimer;

    protected BoxCollider2D boxCollider;

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
