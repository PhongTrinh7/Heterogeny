using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedRoot : EnvironmentalHazard
{
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Plant"))
        {
            Destroy(this.gameObject);
        }
        else if (collider.gameObject.CompareTag("Fire"))
        {
            Destroy(this.gameObject);
        }
        else if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Player"))
        {
            foreach (StatusEffect effect in effects)
            {
                collider.gameObject.GetComponent<Unit>().ApplyStatus(Object.Instantiate(effect));
            }
            collider.gameObject.GetComponent<Unit>().TakeDamage(damage, Element.NONE);
        }
    }
}
