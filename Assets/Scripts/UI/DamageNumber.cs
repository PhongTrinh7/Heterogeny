using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    //Visual tweaks
    public float risingSpeed;
    public float deviationRange;
    public TextMeshPro tmp;
    public Rigidbody2D rb2d;

    //Colors
    public Color32 water;
    public Color32 fire;
    public Color32 wind;
    public Color32 lightning;
    public Color32 earth;

    public void SetDamageVisual(int damage, Element element, bool heal)
    {
        if (heal)
        {
            tmp.SetText((-damage).ToString());
            tmp.color = new Color32(42, 255, 85, 255);
        }
        else
        {
            tmp.SetText(damage.ToString());
            switch (element)
            {
                case Element.WATER:
                    tmp.color = water;
                    break;
                case Element.FIRE:
                    tmp.color = fire;
                    break;
                case Element.WIND:
                    tmp.color = wind;
                    break;
                case Element.LIGHTNING:
                    tmp.color = lightning;
                    break;
                case Element.EARTH:
                    tmp.color = earth;
                    break;
                case Element.NONE:
                    tmp.color = new Color32(255, 255, 255, 255);
                    break;
            }
        }

        rb2d.velocity = new Vector3(Random.Range(deviationRange, -deviationRange), risingSpeed);

        Invoke("Disappear", 1f);
    }

    void Disappear()
    {
        Destroy(this.gameObject);
    }
}

public enum Element
{
    WATER,
    FIRE,
    WIND,
    LIGHTNING,
    EARTH,
    NONE
}

