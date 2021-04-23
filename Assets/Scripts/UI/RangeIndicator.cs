using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<MovingObject>().highlight(true, Color.red);
        }
    }
}
