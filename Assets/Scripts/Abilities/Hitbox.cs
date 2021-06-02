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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            foreach (StatusEffect effect in effects)
            {
                collision.gameObject.GetComponent<Unit>().ApplyStatus(Object.Instantiate(effect));
            }
            collision.gameObject.GetComponent<Unit>().TakeDamage(damage, element);
        }
    }
}
