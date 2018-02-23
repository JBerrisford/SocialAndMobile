using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Node
{
    //Holds all information for A* path finding.
    private Vector2 myPosition;
    public Path_Tile parent;
    public int gScore;
    public int hScore;
    public int railIndex;

    private int _fScore;
    public int FScore
    {
        get { return gScore + hScore; }
    }

    public List<Path_Tile> neighbours;
    public List<Path_Tile> blockedNeighbours;

    //Constructor
    public CS_Node(Vector3 myNewPosition, int index)
    {
        myPosition = myNewPosition;

        neighbours = new List<Path_Tile>();
        neighbours = SetNeighbours(new Vector2(myPosition.x, myPosition.y), false);

        blockedNeighbours = new List<Path_Tile>();
        blockedNeighbours = SetNeighbours(new Vector2(myPosition.x, myPosition.y), true);

        railIndex = index;
        parent = null;
    }

    //Setting Neighbours
    public List<Path_Tile> SetNeighbours(Vector2 position, bool isBlocking)
    {
        List<Path_Tile> newNeighbours = new List<Path_Tile>();

        int x = 0;

        //Decides how many neighbours the list will have. 4 for A*, 8 for the path blocking check.
        if (isBlocking)
            x = 8;
        else
            x = 4;

        for (int y = 0; y < x; y++)
        {
            Vector2 dir = new Vector2(0, 0);

            switch (y)
            {
                case 0:
                    dir = Vector2.up;
                    break;
                case 1:
                    dir = Vector2.right;
                    break;
                case 2:
                    dir = Vector2.down;
                    break;
                case 3:
                    dir = Vector2.left;
                    break;
                case 4:
                    dir = new Vector2(-1, -1);
                    break;
                case 5:
                    dir = new Vector2(1, 1);
                    break;
                case 6:
                    dir = new Vector2(-1, 1);
                    break;
                case 7:
                    dir = new Vector2(1, -1);
                    break;
                default:
                    Debug.Log("Error in graph raycast");
                    break;
            }

            RaycastHit2D ray = Physics2D.Raycast(position, dir, 2.0f);

            if (ray.collider != null)
            {
                Path_Tile pt = ray.collider.gameObject.GetComponent<Path_Tile>();

                if (pt is IPathable)
                {
                    newNeighbours.Add(pt);
                }
            }
        }

        return newNeighbours;
    }

    //Returns the Neighbours list for A* pathfinding.
    public List<Path_Tile> GetPathFindingNodes()
    {
        return neighbours;
    }

    // Returns the blocking list for placement check.
    public List<Path_Tile> GetTowerBlockingNodes()
    {
        return blockedNeighbours;
    }

    //Score Reset
    public void Reset()
    {
        gScore = 0;
        hScore = 0;
        parent = null;
    }

    //Settings the Scores initially
    public void SetScores(Path_Tile endPoint, Path_Tile source)
    {
        gScore = SetGScore(source);
        hScore = SetHScore(endPoint);
    }

    //Setting H Score
    public int SetHScore(Path_Tile end)
    {
        int tempHScore = 0;

        float xDistance = Mathf.Abs(myPosition.x - end.transform.position.x);
        float yDistance = Mathf.Abs(myPosition.y - end.transform.position.y);

        if (xDistance > yDistance)
            tempHScore = (int)(14 * yDistance + 10 * (xDistance - yDistance));
        else
            tempHScore = (int)(14 * xDistance + 10 * (yDistance - xDistance));

        return tempHScore;
    }

    //Setting G Score
    public int SetGScore(Path_Tile source)
    {
        int newGScore = 0;

        float dist = Vector2.Distance(new Vector2(source.transform.position.x, source.transform.position.y), new Vector2(myPosition.x, myPosition.y));

        if (dist <= 1)
        {
            newGScore = 10;
        }
        else
        {
            newGScore = 14;
        }

        return newGScore;
    }
}
