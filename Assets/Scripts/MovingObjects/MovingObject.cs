using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    //Collision detection
    public BoxCollider2D boxCollider;
    public Rigidbody2D rb2D;
    public LayerMask blockingLayer;

    //Animations
    public Animator anim;

    //For highlighting
    public Color ogColor;

    //Health
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;
    public DamageNumber damageNumber;

    //Misc tuning
    public int moveSpeed;
    public int launchSpeed;


    // Start is called before the first frame update
    protected virtual void Start()
    {

        currentHealth = maxHealth;

        //UI setup.
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
    }

    //Cast a line to check for specific layermasks.
    public void CastLineMaskDetect(Vector2 start, Vector2 end, LayerMask layerMask, out RaycastHit2D[] hit)
    {
        //Cast a line from start point to end point checking collision on a LayerMask.
        hit = Physics2D.LinecastAll(start, end, layerMask);
    }

    protected IEnumerator SmoothMovement(Vector3 end, float speedMultiplier)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, speedMultiplier * moveSpeed * Time.fixedDeltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    public void Launch(Vector2 direction, int displacement)
    {
        StartCoroutine(LaunchCoroutine(direction, displacement - 1)); //-1 accounting for start vector being in front of object.
    }

    public IEnumerator LaunchCoroutine(Vector2 direction, int displacement)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = (Vector2)transform.position + direction;

        // Calculate end position based on the direction and displacement distance.
        Vector2 end = start + (direction * displacement);

        RaycastHit2D[] hit;

        
        CastLineMaskDetect(start, end, blockingLayer, out hit);

        //Check if anything was hit
        if (hit.Length == 0)
        {
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            yield return StartCoroutine(SmoothMovement(end, launchSpeed));
        }

        else
        {
            //If something is hit, collide with obstacle.
            Vector3 offset = direction;
            yield return StartCoroutine(SmoothMovement(hit[0].transform.position - offset, launchSpeed));
            TakeDamage(3, Element.NONE);
        }
    }

    public virtual void TakeDamage(int damage, Element e)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        Instantiate(damageNumber, transform.position, Quaternion.identity).SetDamageVisual(damage, e, false);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Highlight(Color highlightColor)
    {
        GetComponent<SpriteRenderer>().color = highlightColor;
    }

    public void Unhighlight()
    {
        GetComponent<SpriteRenderer>().color = ogColor;
    }

    protected virtual void Death()
    {
        Destroy(this);
    }
}
