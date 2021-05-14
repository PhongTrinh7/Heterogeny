using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected int boardRows;
    protected int boardColumns;
    protected float moveDelay = 1;
    protected List<Vector3> staticUnwalkables;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ChangeFacingDirection(new Vector2(-1, 0));
    }

    public void SetUpBoardKnowledge(int rows, int columns, List<Vector3> unwalkables)
    {
        boardRows = rows;
        boardColumns = columns;
        staticUnwalkables = unwalkables;
    }

    public virtual IEnumerator TurnSequence(List<Vector3> currentUnwalkables)
    {
        UnitStateChange(UnitState.BUSY);

        currentUnwalkables.Remove(transform.position);

        //List of possible paths to a targets
        List<PathNode> chosenPath = new List<PathNode>();
        GameObject target = null;

        //Find all players
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

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
                target = t;
            }
        }

        /*
        if (chosenPath == null || target == null)
        {
            Debug.Log("No possible routes to targets!");
            EndTurn();
            yield return null;
        }


        //yield return new WaitForSecondsRealtime(1);
        if (chosenPath.Count == 0)
        {
            yield return StartCoroutine(EnemyAbility(target));
            yield return null;
        }

        foreach (PathNode pn in chosenPath)
        {
            Debug.Log(pn.coord);
            facingDirection = Vector3.Normalize(pn.coord - transform.position);
            ChangeFacingDirection(facingDirection);
            yield return StartCoroutine(Move((int)facingDirection.x, (int)facingDirection.y));

            if (currentHealth <= 0)
            {
                yield break;
            }

            if (Vector3.Distance(transform.position, target.transform.position) <= 1)
            {
                yield return StartCoroutine(EnemyAbility(target));
            }
        }*/

        EndTurn();
        yield return null;
    }

    protected IEnumerator EnemyAbility(GameObject target)
    {
        ChangeFacingDirection(target.transform.position - transform.position);
        if (ReadyAbility(0))
        {
            yield return new WaitForSecondsRealtime(moveDelay);
            CastAbility();
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSecondsRealtime(anim.GetCurrentAnimatorStateInfo(0).length + moveDelay);
        }
    }
}
