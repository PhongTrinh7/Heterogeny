using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestAbility")]
public class TestAbility : Ability
{
    public override void Effect(int i)
    {
        switch (i)
        {
            case 0:
                foreach (RaycastHit2D hit in GetRaycastHitsForward(0))
                {
                    if (hit.transform != null && hit.transform.CompareTag("Player") || hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponent<MovingObject>().TakeDamage(5, Element.WATER);
                    }
                }
                Debug.Log(i);
                break;
            case 1:
                Debug.Log(i);
                break;
            case 2:
                foreach (RaycastHit2D hit in GetRaycastHitsForward(0))
                {
                    if (hit.transform != null && hit.transform.CompareTag("Player") || hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponent<MovingObject>().TakeDamage(5, Element.NONE);
                        hit.transform.GetComponent<MovingObject>().Launch(caster.facingDirection, 3);
                    }
                }
                Debug.Log(i);
                break;
        }
    }
}
