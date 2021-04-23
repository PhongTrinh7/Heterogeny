using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public RectTransform background;
    public Text tooltipText;
    public float padding = 8f;

    public void ShowTooltip(int i)
    {
        //Activate the tooltip
        gameObject.SetActive(true);

        //Get ability for corresponding button from the Battle Manager
        //Ability ability = BattleManager.Instance.activeUnit.abilitiesReference[i];

        tooltipText.text = BattleManager.Instance.activeUnit.abilities[i].Description();
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + padding, tooltipText.preferredHeight + padding);
        background.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    /*public void Update()
    {
        if (isActiveAndEnabled)
        {
            tooltipText.text = ability.Description();
            Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + padding, tooltipText.preferredHeight + padding);
            background.sizeDelta = backgroundSize;
        }
    }*/
}
