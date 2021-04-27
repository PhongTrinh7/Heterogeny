using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ChangeFacingDirection(new Vector2(1, 0));
    }

    public void ListenForInput()
    {
        if (state == UnitState.ACTIVE)
        {

            if (Input.GetKeyDown("space"))
            {
                EndTurn();
            }
            if (Input.GetKey("left shift"))
            {
                //Keyboard Input
                if (Input.GetKeyDown("w"))
                {
                    ChangeFacingDirection(new Vector2(0, 1));
                }
                else if (Input.GetKeyDown("a"))
                {
                    ChangeFacingDirection(new Vector2(-1, 0));
                }
                else if (Input.GetKeyDown("s"))
                {
                    ChangeFacingDirection(new Vector2(0, -1));
                }
                else if (Input.GetKeyDown("d"))
                {
                    ChangeFacingDirection(new Vector2(1, 0));
                }
            }
            else
            {
                //Keyboard Input
                if (Input.GetKey("w"))
                {
                    StartCoroutine(Move(0, 1));
                }
                else if (Input.GetKey("a"))
                {
                    StartCoroutine(Move(-1, 0));
                }
                else if (Input.GetKey("s"))
                {
                    StartCoroutine(Move(0, -1));
                }
                else if (Input.GetKey("d"))
                {
                    StartCoroutine(Move(1, 0));
                }
                else if (Input.GetKeyDown("x"))
                {
                    TakeDamage(60, Element.LIGHTNING);
                }
            }
        }
        else if (state == UnitState.READYUP)
        {
            //Confirm and cast the ability.
            if (Input.GetMouseButtonDown(0))
            {
                CastAbility();
            }
            //Cancel the readied ability.
            else if (Input.GetMouseButtonDown(1))
            {
                CancelAbility();
            }
        }
    }

    public override void CastAbility()
    {
        base.CastAbility();
        UIManager.Instance.UpdateActiveUnitAbilities();
    }

    protected void CancelAbility()
    {
        abilities[loadedAbility].HideRange();
        loadedAbility = -1;
        UnitStateChange(UnitState.ACTIVE);
        Debug.Log("Ability canceled.");
    }

    public override void UnitStateChange(UnitState newState)
    {
        //priorState = state;
        state = newState;
        UIManager.Instance.ListenForBattleUI(state == UnitState.ACTIVE);
    }

    public override void StartTurn()
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy + energyRegen, 0, maxEnergy);
        HandleCooldowns();
        UnitStateChange(UnitState.ACTIVE);
    }
}
