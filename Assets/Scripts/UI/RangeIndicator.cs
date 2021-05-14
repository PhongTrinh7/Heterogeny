using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public BoxCollider2D selfCollider;
    public Color highlightColor;

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("collided");
            collision.gameObject.GetComponent<MovingObject>().Highlight(highlightColor);
        }
    }
*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<MovingObject>().Unhighlight();
        }
    }
}
