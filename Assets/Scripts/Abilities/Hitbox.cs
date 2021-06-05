using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage;
    public Element element;
    public BoxCollider2D boxCollider;
    public StatusEffect[] effects;
    public Animator anim;


    public void ColliderSwitch()
    {
        boxCollider.enabled = !boxCollider.enabled;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
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
        }
    }
}
