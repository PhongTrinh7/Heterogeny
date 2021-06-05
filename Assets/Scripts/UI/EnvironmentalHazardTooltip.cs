using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentalHazardTooltip : MonoBehaviour
{
    private RectTransform background;
    private TextMeshProUGUI tooltipText;

    void Awake()
    {
        Debug.Log("Awaken my masters!");
        background = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void ShowTooltip(EnvironmentalHazard hazard)
    {
        gameObject.SetActive(true);

        tooltipText.text = hazard.localName + "\n" + hazard.description + "\nDuration: " + hazard.duration;
        float padding = 0f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + padding, tooltipText.preferredHeight + padding);
        background.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
