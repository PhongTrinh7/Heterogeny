using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Hitbox
{
    public float duration;
    public bool pierce;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

        if (collider.gameObject.GetComponent<MovingObject>() != null)
        {
            if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Player"))
            {
                foreach (StatusEffect effect in effects)
                {
                    collider.gameObject.GetComponent<Unit>().ApplyStatus(Object.Instantiate(effect));
                }
            }
            collider.gameObject.GetComponent<MovingObject>().TakeDamage(damage, element);

            if (!pierce)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if(duration < 0)
        {
            Destroy(this.gameObject);
        }
    }


}
