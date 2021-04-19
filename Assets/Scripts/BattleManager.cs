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
    public Pathfinding pathfinding;
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
                enemyCount++;
            }
        }

        //Set up pathfinding.
        currentUnwalkables = new List<Vector3>();
        UpdateUnwalkables();
        pathfinding = new Pathfinding(board.columns, board.rows, currentUnwalkables);


        turnCounter = 0;

        activeUnit = currentUnits[0];
        activeUnit.StartTurn();
    }

    public void AdvanceTurn()
    {
        StartCoroutine(AdvanceTurn(false));
    }

    IEnumerator AdvanceTurn(bool stay)
    {
        //Lock players controls
        controlLocked = true;

        //Next up in line goes.
        turnCounter++;
        activeUnit = currentUnits[turnCounter%currentUnits.Count];
        Debug.Log(activeUnit);

        activeUnit.StartTurn();

        //Center camera on active unit
        cameraController.CameraLookAt(activeUnit);

        //Let effects run their course before freeing movement
        yield return new WaitForSeconds(turnDelay);

        //Updates pathfinding based on current unit positions.
        UpdateUnwalkables();

        //Give players control again.
        controlLocked = false;
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
        if (activeUnit is Player)
        {
            Player activePlayerChar = (Player) activeUnit;
            activePlayerChar.ListenForInput();
        }
    }

    private void OnDisable()
    {
        cameraController.ResetCamera();
        Destroy(board);
    }
}
