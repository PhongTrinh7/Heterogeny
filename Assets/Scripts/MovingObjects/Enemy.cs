using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected int boardRows;
    protected int boardColumns;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ChangeFacingDirection(new Vector2(-1, 0));
    }

    public void SetUpBoardKnowledge(int rows, int columns)
    {
        boardRows = rows;
        boardColumns = columns;
    }

    public IEnumerator TurnSequence(List<Vector3> currentUnwalkables)
    {
        UnitStateChange(UnitState.BUSY);

        //Find all players
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        currentUnwalkables.Remove(transform.position);

        //List of possible paths to a targets
        List<PathNode> chosenPath = new List<PathNode>();
        int minPathLength = boardRows * boardColumns;

        foreach (GameObject t in targets)
        {
            currentUnwalkables.Remove(t.transform.position); //Make sure the target is reachable.

            Pathfinding pathfinding = new Pathfinding(boardColumns, boardRows, currentUnwalkables);

            currentUnwalkables.Add(t.transform.position); //Reinsert target for the next iteration.

            List<PathNode> path = pathfinding.FindPath((int)transform.position.x, (int)transform.position.y, (int)t.transform.position.x, (int)t.transform.position.y);

            if (path != null && path.Count < minPathLength)
            {
                minPathLength = path.Count;
                chosenPath = path;
            }
        }
        foreach (PathNode pn in chosenPath)
        {
            Debug.Log(pn.coord);
        }

        //SmoothMovement(chosenPath[chosenPath.Count - 1].coord, 1);
        EndTurn();
        yield return null;
    }
}
