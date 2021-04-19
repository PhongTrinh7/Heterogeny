using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MovingObject
{
    public enum UnitState{
        ACTIVE,
        BUSY,
        IDLE
    }

    public UnitState state;
    public UnitState priorState;

    public UnitState State
    {
        get { return state; }
        set { state = value; }
    }

    public string unitName;
    public Image portrait;

    //Unit stats.
    public int maxEnergy;
    public int currentEnergy;

    //Unit Abilities.
    public List<Ability> abilities;

    //Movement
    protected float inverseMoveTime;
    public Vector2 facingDirection;
    public int moveCost;

    //Status Afflictions
    bool bStun;
    bool bImmobile;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentEnergy = maxEnergy;
    }

    protected void RefreshAbilities()
    {
        //Refresh ability cooldowns each battle.
        foreach (Ability a in abilities)
        {
            abilities.Add(Object.Instantiate(a));
        }
    }

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    public IEnumerator Move(int xDir, int yDir)
    {
        if (bStun || bImmobile)
        {
            Debug.Log("Can't move");
            yield break;
        }

        priorState = state;
        state = UnitState.BUSY;

        Vector2 newFacingDirection = new Vector2(xDir, yDir);
        ChangeFacingDirection(newFacingDirection);

        if (currentEnergy >= moveCost)
        {
            //Store start position to move from, based on objects current transform position.
            Vector2 start = transform.position;

            // Calculate end position based on the direction parameters passed in when calling Move.
            Vector2 end = start + facingDirection;

            RaycastHit2D[] hit;

            CastLineMaskDetect(end, end, blockingLayer, out hit);

            //Check if anything was hit
            if (hit.Length == 0)
            {
                Debug.Log("move");
                currentEnergy -= moveCost;

                //Movement animation.
                anim.SetBool("Moving", true);
                yield return StartCoroutine(SmoothMovement(end, 1f));
            }
        }

        anim.SetBool("Moving", false);
        ReturnToPriorState();
    }

    public void ChangeFacingDirection(Vector2 direction)
    {
        if (bStun)
        {
            return;
        }

        //Sets facing direction.
        facingDirection = direction;
        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Vertical", direction.y);
    }

    public void ReturnToPriorState()
    {
        state = priorState;
    }

    public void StartTurn()
    {
        state = UnitState.ACTIVE;
    }

    public void EndTurn()
    {
        state = UnitState.IDLE;
    }

    public void Death()
    {

    }
}
