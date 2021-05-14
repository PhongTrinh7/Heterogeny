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
        UIManager.Instance.ShowBattleUI(true);
        currentUnits = new List<Unit>();
        currentUnwalkables = new List<Vector3>();
    }

    public void StartBattle(Board[] boards, Enemy[] enemies)
    {
        //Set up the playing field.
        board = Instantiate(boards[Random.Range(0, boards.Length)]);
        board.SetUp(enemies);

        cameraController.BattleCamera(board);

        //Set up turn order.
        SetUpTurnOrder(board.units);

        //Set up pathfinding.
        UpdateUnwalkables();

        turnCounter = 1;

        AdvanceTurn();
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

        activeUnit = currentUnits[0];
        Debug.Log(activeUnit);

        //Update the turn order UI panel.
        UIManager.Instance.UpdateTurnOrder(currentUnits);

        //Center camera on active unit
        cameraController.CameraLookAt(activeUnit);

        //Updates pathfinding based on current unit positions.
        UpdateUnwalkables();

        //Let effects run their course before freeing movement
        yield return new WaitForSeconds(turnDelay);

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

    public void SetUpTurnOrder(List<Unit> units)
    {
        units.Sort(SortBySpeed);
        currentUnits.Clear();

        foreach (Unit unit in units)
        {
            currentUnits.Add(unit);

            if (unit.CompareTag("Player"))
            {
                playerCount++;
            }
            else if (unit.CompareTag("Enemy"))
            {
                Enemy enemy = (Enemy)unit;
                enemy.SetUpBoardKnowledge(board.rows, board.columns, board.unwalkables);
                enemyCount++;
            }
        }
    }

    public void UpdateUnwalkables()
    {
        currentUnwalkables.Clear();
        currentUnwalkables.AddRange(board.unwalkables);

        foreach (MovingObject mo in currentUnits)
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
        currentUnits.Remove(unit);
        unit.gameObject.SetActive(false); //Temporary, will change when death animations are added.

        if (unit is Player)
        {
            playerCount--;
            if (playerCount <= 0)
            {
                EndBattle(false);
                return;
            }
        }
        else if (unit is Enemy)
        {
            enemyCount--;
            if (enemyCount <= 0)
            {
                EndBattle(true);
                return;
            }
        }

        if (unit.Equals(activeUnit))
        {
            AdvanceTurn();
        }
        else
        {
            UIManager.Instance.UpdateTurnOrder(currentUnits);
        }
    }

    public void EndBattle(bool win)
    {
        controlLocked = true;
        UIManager.Instance.ShowBattleUI(false);
        if (win)
        {
            Debug.Log("Victory");
        }
        else
        {
            Debug.Log("Defeat");
        }
        GameManager.Instance.EndBattle();
    }

    private void OnDisable()
    {
        cameraController.ResetCamera();
        Destroy(board.gameObject);

        foreach (Player pc in GameManager.Instance.playerChars)
        {
            pc.gameObject.SetActive(false);
        }
    }

    int SortBySpeed(Unit u1, Unit u2)
    {
        return u1.spd.CompareTo(u2.spd);
    }
}
