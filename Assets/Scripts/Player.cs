using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    // Start is called before the first frame update

    public void ListenForInput()
    {
        if (state.Equals(UnitState.ACTIVE))
        {

            if (Input.GetKeyDown("space"))
            {
                BattleManager.Instance.AdvanceTurn();
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
                if (Input.GetKeyDown("w"))
                {
                    StartCoroutine(Move(0, 1));
                }
                else if (Input.GetKeyDown("a"))
                {
                    StartCoroutine(Move(-1, 0));
                }
                else if (Input.GetKeyDown("s"))
                {
                    StartCoroutine(Move(0, -1));
                }
                else if (Input.GetKeyDown("d"))
                {
                    StartCoroutine(Move(1, 0));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
