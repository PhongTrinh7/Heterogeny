using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    //Reference to the caster and target(s)
    public Unit caster;
    protected List<MovingObject> targets = new List<MovingObject>();

    //Restrictions
    private string characterRestriction;
    public int abilitySlot;

    //UI Info
    public Sprite sprite;
    public string abilityName;
    public string description;
    public int initialDamage;
    public int damage;
    public int cost;
    public int initialCooldown;
    public int cooldown;
    public float cooldownFill;
    public bool onCooldown;

    //List of spawnable hitboxes and traps
    protected GameObject[] effects;

    //Animation to play
    public string animationName;

    //Layermasks to check for
    public LayerMask layermask;

    //SFor showing the range
    public List<Vector2> aoe;
    public List<RangeIndicator> spawnedRangeIndicators;
    public RangeIndicator rangeIndicator;
    public Color32 highlightColor;

    public virtual void OnEnable()
    {
        highlightColor = new Color32(255, 0, 0, 120);
        cooldownFill = 1;
    }

    public virtual string Description()
    {
        string summary = abilityName + "\n" + description + "\nDamage: " + damage + "\nCost: " + cost + "\nCooldown: " + initialCooldown;
        return summary;
    }

    //Calculates range and shows it on the field
    public virtual void ShowRange()
    {
        aoe.Add((Vector2)caster.transform.position + caster.facingDirection);

        foreach (Vector2 xy in aoe)
        {
            spawnedRangeIndicators.Add(Instantiate(rangeIndicator, xy, Quaternion.identity, caster.transform));
        }
    }

    public virtual void HideRange()
    {
        aoe.Clear();

        foreach (RangeIndicator r in spawnedRangeIndicators)
        {
            Destroy(r.gameObject);
        }

        spawnedRangeIndicators.Clear();
    }

    public virtual bool Ready()
    {
        if (onCooldown)
        {
            return false;
        }

        ShowRange();
        return true;
    }

    public virtual void Cast()
    {
        HideRange();

        caster.TriggerAnimation(animationName);

        PlaceOnCooldown();
    }

    public abstract void Effect(int i);

    public virtual void PlaceOnCooldown()
    {
        cooldown = initialCooldown;
        onCooldown = true;
        cooldownFill = 0;
    }

    public virtual void Cooldown()
    {
        if (onCooldown)
        {
            cooldown--;
            cooldownFill += (1f / initialCooldown);
            if (cooldown == 0)
            {
                onCooldown = false;
                cooldownFill = 1;
            }
        }
    }

    protected Vector2 GetForwardStartVector()
    {
        return (Vector2)caster.transform.position + caster.facingDirection;
    }

    protected RaycastHit2D[] GetRaycastHitsForward(int range)
    {
        Vector2 start = GetForwardStartVector();
        RaycastHit2D[] hits;
        caster.CastLineMaskDetect(start, start + caster.facingDirection * range, layermask, out hits);
        return hits;
    }
}
