using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

public class BattleManager : Manager<BattleManager>
{
    //Board references.
    public Board[] boards;
    public Board board;

    //Handles turns.
    public float turnDelay;
    public Unit activeUnit;
    public List<Unit> currentUnits;
    public int turnCounter;

    //Pathfinding.
    public List<MovingObject> movingObjects;
    public List<Vector3> currentUnwalkables;

    //Camera.
    public CameraController cameraController;

    //Lock.
    public bool controlLocked;

    //End.
    private int playerCount;
    private int enemyCount;

    private void Start()
    {

        
    }
    private void OnEnable()
    {

    }

    public void StartBattle(Board[] boards, Player[] players, Enemy[] enemies)
    {
        //Set up the playing field.
        board = Instantiate(boards[Random.Range(0, boards.Length)]);
        board.SetUp(players, enemies);

        cameraController.BattleCamera(board);

        foreach (Unit unit in board.units)
        {
            currentUnits.Add(unit);

            if (unit.CompareTag("Player"))
            {
                playerCount++;
            }
            else if (unit.CompareTag("Enemy"))
            {
                Enemy enemy = (Enemy)unit;
                enemy.SetUpBoardKnowledge(board.rows, board.columns);
                enemyCount++;
            }
        }

        //Set up pathfinding.
        currentUnwalkables = new List<Vector3>();
        UpdateUnwalkables();

        turnCounter = -1;

        AdvanceTurn();

        //activeUnit = currentUnits[0];
        //Debug.Log(activeUnit.name);
        //activeUnit.StartTurn();
    }

    public void AdvanceTurn()
    {
        StartCoroutine(AdvanceTurn(false));
    }

    IEnumerator AdvanceTurn(bool stay)
    {
        //Lock players controls
        controlLocked = true;
        UIManager.Instance.ListenForBattleUI(false);

        //Next up in line goes.
        turnCounter++;
        
        activeUnit = currentUnits[turnCounter%currentUnits.Count];
        Debug.Log(activeUnit);

        if (activeUnit.bDead)
        {
            AdvanceTurn();
            yield break;
        }

        //Center camera on active unit
        cameraController.CameraLookAt(activeUnit);

        //Let effects run their course before freeing movement
        yield return new WaitForSeconds(turnDelay);

        //Updates pathfinding based on current unit positions.
        UpdateUnwalkables();


        activeUnit.StartTurn();

        if (activeUnit is Enemy)
        {
            Enemy enemy = (Enemy) activeUnit;
            StartCoroutine(enemy.TurnSequence(currentUnwalkables));
        }
        else if (activeUnit is Player && controlLocked)
        {
            //Give players control again.
            controlLocked = false;
        }
    }

    public void UpdateUnwalkables()
    {
        currentUnwalkables.Clear();
        currentUnwalkables.AddRange(board.unwalkables);

        foreach (MovingObject mo in movingObjects)
        {
            currentUnwalkables.Add(mo.transform.position);
        }
    }

    private void Update()
    {
        //While BattleManager is active, make sure things are listening for battle specific inputs
        if (activeUnit is Player && !controlLocked)
        {
            Player activePlayerChar = (Player) activeUnit;
            activePlayerChar.ListenForInput();
        }
    }

    public void UnitDeath(Unit unit)
    {
        unit.gameObject.SetActive(false); //Temporary, will change when death animations are added.

        if (unit is Player)
        {
            playerCount--;
            if (playerCount <= 0)
            {
                EndBattle(true);
                return;
            }
        }
        else if (unit is Enemy)
        {
            enemyCount--;
            if (enemyCount <= 0)
            {
                EndBattle(false);
                return;
            }
        }

        if (unit.Equals(activeUnit))
        {
            AdvanceTurn();
        }
    }

    public void EndBattle(bool win)
    {
        UIManager.Instance.ShowBattleUI(false);
        if (win)
        {
            Debug.Log("Victory");
        }
        else
        {
            Debug.Log("Defeat");
        }
    }

    private void OnDisable()
    {
        cameraController.ResetCamera();
        Destroy(board);
    }
}
