using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Hitbox
{
    public float duration;
    public bool pierce;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            foreach (StatusEffect effect in effects)
            {
                collision.gameObject.GetComponent<Unit>().ApplyStatus(Object.Instantiate(effect));
            }
            collision.gameObject.GetComponent<Unit>().TakeDamage(damage, element);

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
