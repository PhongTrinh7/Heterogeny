using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HazardTest")]
public class HazardTest : Ability
{
    public EnvironmentalHazard hazard;

    public override void ShowRange()
    {
        Vector2 dir = caster.facingDirection;

        aoe.Add((Vector2)TileInFront);

        aoe.Add(TileInFront + dir);

        aoe.Add(TileInFront + dir + Vector2.Perpendicular(dir));

        aoe.Add(TileInFront + dir - Vector2.Perpendicular(dir));

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
                    if (hit.transform != null && hit.transform.gameObject.GetComponent<MovingObject>() != null)
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
                List<List<Vector3>> waves = new List<List<Vector3>>();

                Vector2 start = caster.transform.position;

                Vector2 dir = caster.facingDirection;

                RaycastHit2D[] hitLayerMask;

                List<Vector3> wave1 = new List<Vector3>();

                wave1.Add((Vector2)TileInFront);

                List<Vector3> wave2 = new List<Vector3>();

                wave2.Add(TileInFront + dir);

                wave2.Add(TileInFront + dir + Vector2.Perpendicular(dir));

                wave2.Add(TileInFront + dir - Vector2.Perpendicular(dir));

                waves.Add(wave1);
                waves.Add(wave2);

                caster.PlaceHazardWave(hazard, waves, 0.2f);

                Debug.Log(i);
                break;
        }
    }
}
