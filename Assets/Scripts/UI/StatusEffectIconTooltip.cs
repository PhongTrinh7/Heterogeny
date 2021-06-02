using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIconTooltip : MonoBehaviour
{
    private RectTransform background;
    private TextMeshProUGUI tooltipText;
    //public StatusEffect statusEffect;

    void Awake()
    {
        background = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void ShowTooltip(StatusEffect statusEffect)
    {
        gameObject.SetActive(true);

        tooltipText.text = statusEffect.Description();
        float padding = 0f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + padding, tooltipText.preferredHeight + padding);
        background.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
