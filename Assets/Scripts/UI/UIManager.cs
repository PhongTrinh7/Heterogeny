using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    Canvas gameUI;

    //Battle UI
    public Canvas battleUI;

    //Ability buttons
    public Button[] abilityButtons;
    [SerializeField] public Button abilityButton1;
    [SerializeField] public Button abilityButton2;
    [SerializeField] public Button abilityButton3;
    [SerializeField] public Button abilityButton4;
    [SerializeField] public Button endTurnButton;

    //Turn order
    public GameObject turnOrderPanel;

    private void Start()
    {
        //GameManager.Instance.OnBattleStart.AddListener(ShowBattleUI);
    }

    public void ShowBattleUI(bool b)
    {
        battleUI.gameObject.SetActive(b);
        //battleUI.enabled = true;
    }

    //Make buttons interactable or not
    public void ListenForBattleUI(bool b)
    {
        foreach (Button button in abilityButtons)
        {
            button.interactable = b;
        }

        //Only check for usability if turning UI to interactable.
        if (b)
        {
            UpdateActiveUnitAbilities();
        }

        endTurnButton.interactable = b;
    }

    public void AbilityButtonClicked(int i)
    {
        BattleManager.Instance.activeUnit.ReadyAbility(i);
    }

    public void EndTurnButtonClicked()
    {
        Debug.Log("End Turn");
    }

    public void UpdateActiveUnitAbilities()
    {
        Player activePlayer = BattleManager.Instance.activeUnit as Player;

        for (int i = 0; i < activePlayer.abilities.Count; i++)
        {
            if (activePlayer.CurrentEnergy < activePlayer.abilities[i].cost)
            {
                abilityButtons[i].interactable = false;
            }
            else
            {
                abilityButtons[i].interactable = !activePlayer.abilities[i].onCooldown;
            }
            //abilityButtons[i].GetComponent<Image>().sprite = activePlayer.abilities[i].sprite;
            abilityButtons[i].GetComponent<Image>().fillAmount = activePlayer.abilities[i].cooldownFill;
        }
    }

    public void UpdateTurnOrder(List<Unit> units)
    {
        foreach (Transform child in turnOrderPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Unit unit in units)
        {
            Portrait portrait = Instantiate(unit.portrait, turnOrderPanel.transform);
            portrait.character = unit;
            unit.tempPortrait = portrait;
        }
    }

    public void HighlightUnit(Portrait portrait)
    {
        portrait.character.Highlight(Color.blue);
        portrait.Highlight();
    }

    public void UnhighlightUnit(Portrait portrait)
    {
        portrait.character.Unhighlight();
        portrait.Unhighlight();
    }

    public void HighlightPortrait(Unit unit)
    {
        unit.Highlight(Color.blue);
        unit.tempPortrait.Highlight();
    }

    public void UnhighlightPortrait(Unit unit)
    {
        unit.Unhighlight();
        unit.tempPortrait.Unhighlight();
    }

    public void ShowBattleEnd(bool win)
    {

    }
}
