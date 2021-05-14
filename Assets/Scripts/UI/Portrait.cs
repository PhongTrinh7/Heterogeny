using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public Unit character;
    public Image image;

    public void Highlight()
    {
        image.color = Color.blue;
    }

    public void Unhighlight()
    {
        image.color = character.ogColor;
    }

    private void OnDestroy()
    {
        character.Unhighlight();
    }
}
