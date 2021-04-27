using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //Board size
    public int rows;
    public int columns;

    //Board components to layout
    public GameObject[] outerWallTiles;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public List<Vector3> unwalkables;
    public GameObject backgroundMid;

    //Units
    private Enemy[] enemies;
    public List<Unit> units;

    //misc
    private Transform boardHolder;            //A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions;      //A list of possible locations to place tiles.

    public void SetUp(Enemy[] enemies)
    {
        this.enemies = enemies;

        //Reset our list of gridpositions.
        InitialiseList();

        //Creates the outer walls and floor.
        BoardSetup();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(wallTiles, 5, 8);

        //Instantiate the players.
        LayoutUnits();
    }

    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        gridPositions = new List<Vector3>();

        //Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = transform;

        Instantiate(backgroundMid, new Vector3(0, 0, 0), Quaternion.identity).transform.SetParent(boardHolder);

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -1; x < columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //Check if the current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    //Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity).transform.SetParent(boardHolder);
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }


    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        unwalkables = new List<Vector3>();
        
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Store position as unwalkable for pathfinding.
            unwalkables.Add(randomPosition);

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity);

            //Set parent to board.
            instance.transform.SetParent(boardHolder);
        }
    }

    void LayoutUnits()
    {
        units = new List<Unit>();
        int y = 0;
        foreach (Player pc in GameManager.Instance.playerChars)
        {
            if (pc.currentHealth > 0)
            {
                //Store position as unwalkable for pathfinding.
                pc.transform.position = new Vector3(0, rows - 1 - 3 * y, 0);
                pc.gameObject.SetActive(true);
                units.Add(pc);
                y++;
            }
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            int range = 3;//Random.Range(1, 4);
            for (int j = 0; j < range; j++)
            {
                units.Add(Instantiate(enemies[i], new Vector3(columns - 1, (rows - 1) - (rows / ((enemies.Length+1) * range)) * j, 0), Quaternion.identity, boardHolder));
            }
        }
    }
}
