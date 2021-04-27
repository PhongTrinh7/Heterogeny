using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NothingPersonnel")]
public class NothingPersonnel : Ability
{
    public int displacement;

    //Calculates range and shows it on the field
    public override void ShowRange()
    {
        aoe.Add((Vector2)caster.transform.position + caster.facingDirection);
        aoe.Add((Vector2)caster.transform.position + caster.facingDirection * 2);
        aoe.Add((Vector2)caster.transform.position + caster.facingDirection * 3);

        foreach (Vector2 xy in aoe)
        {
            spawnedRangeIndicators.Add(Instantiate(rangeIndicator, xy, Quaternion.identity, caster.transform));
        }
    }
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
                foreach (RaycastHit2D hit in GetRaycastHitsForward(2))
                {
                    if (hit.transform != null && hit.transform.CompareTag("Player") || hit.transform.CompareTag("Enemy"))
                    {
                        Vector3 hitPosition = hit.transform.position;
                        hit.transform.position = caster.transform.position;
                        caster.transform.position = hitPosition;
                        hit.transform.GetComponent<MovingObject>().TakeDamage(5, Element.NONE);
                        //hit.transform.GetComponent<MovingObject>().Launch(-caster.facingDirection, 3);
                    }
                }
                Debug.Log(i);
                break;
        }
    }
}
