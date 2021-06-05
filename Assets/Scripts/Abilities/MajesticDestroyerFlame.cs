using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MajesticDestroyerFlame")]
public class MajesticDestroyerFlame : Ability
{
    public GameObject fireball;
    public int projectileSpeed;

    public override void Effect(int i)
    {
        switch (i)
        {
            case 0:
                Instantiate(fireball, caster.transform.position + (Vector3) caster.facingDirection, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = caster.facingDirection * projectileSpeed;
                Debug.Log(i);
                break;
            case 1:
                Instantiate(fireball, caster.transform.position + (Vector3)caster.facingDirection, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = caster.facingDirection * projectileSpeed;
                Debug.Log(i);
                break;
            case 2:
                Instantiate(fireball, caster.transform.position + (Vector3)caster.facingDirection, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = caster.facingDirection * projectileSpeed;
                Debug.Log(i);
                break;
        }
    }
}
