using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MovingObject
{
    public GameObject explosion;

    protected override void Death()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        base.Death();
    }
}
