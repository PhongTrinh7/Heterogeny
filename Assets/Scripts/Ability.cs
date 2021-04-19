using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    //Reference to the caster and target(s)
    protected Unit caster;
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
    public int range;
    public int cost;
    public int initialCooldown;
    public int cooldown;
    public float cooldownFill;
    public bool onCooldown;

    //Animation to play
    public string animationName;

    //Layermasks to check for
    public LayerMask[] layermask;

    //SFor showing the range
    public GameObject highlight;
    public Color32 highlightColor;

    public virtual void OnEnable()
    {
        highlightColor = new Color32(255, 0, 0, 120);
        cooldownFill = 1;
    }

    public virtual string Description()
    {
        string summary = abilityName + "\n" + description + "\nDamage: " + damage + "\nRange: " + range + "\nCost: " + cost + "\nCooldown: " + initialCooldown;
        return summary;
    }

    public virtual void ShowRange()
    {

    }

    public virtual void HideRange()
    {

    }

    public virtual bool Ready(Unit caster)
    {
        if (onCooldown)
        {
            return false;
        }

        this.caster = caster;
        ShowRange();
        return true;
    }

    public virtual void Cast()
    {
        HideRange();

        //caster.TriggerAnimation(animationName);

        PlaceOnCooldown();
    }

    public abstract void Effect();

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
}
