using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : Singleton_Base<Map_Manager>
{
	List<Tile_Base> openGrid = new List<Tile_Base>();
	List<Tile_Base> closedGrid = new List<Tile_Base>();

	List<Tile_Base> pathfindingTiles = new List<Tile_Base>();

	public override void Init ()
	{
		base.Init ();
	}

	void SetLists()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag ("Tile");

		foreach(GameObject temp in go)
		{
			Tile_Base newTile = temp.GetComponent<Tile_Base>();
			openGrid.Add (newTile);

			if(newTile is IPathable)
			{
				pathfindingTiles.Add (newTile);

				if(newTile.GetComponent<Path_Tile>().isStartPoint)
					newTile.gameObject.tag = "StartPoint";
                else if(newTile.GetComponent<Path_Tile>().isEndPoint)
                    newTile.gameObject.tag = "EndPoint";
                else
					newTile.gameObject.tag = "Path";
			}
		}
	}

    private void InitTiles()
    {
        foreach (Tile_Base temp in openGrid)
            temp.Init();
    }

    public void NodeAction(Tower_Base towerBase, bool canOpen)
    {
        if (canOpen)
        {
            towerBase.GetOccupiedTiles().OpenNode();
        }
        else
        {
            towerBase.GetOccupiedTiles().CloseNode(towerBase);
        }

        if (towerBase is IWall)
        {
            Wave_Manager.Instance.InitLists();
        }
    }

	public void AddToClosed(Tile_Base tile)
	{
		openGrid.Remove(tile);
		closedGrid.Add (tile);

		if (tile is IPathable)
			Wave_Manager.Instance.FindNewPath ();
	}

	public void AddToOpen(Tile_Base tile)
	{
		closedGrid.Remove(tile);
		openGrid.Add (tile);

		if (tile is IPathable)
			Wave_Manager.Instance.FindNewPath ();
	}

	public bool GeneratePathfinding(Path_Tile start, Path_Tile end, int routeList)
	{
		List<Path_Tile> open = new List<Path_Tile> ();
		List<Path_Tile> closed = new List<Path_Tile> ();

        foreach(Tile_Base temp in closedGrid)
        {
            if(temp is IPathable)
            {
                closed.Add(temp as Path_Tile);
            }
        }

        Path_Tile current = start;
        int currentRail = end.railIndex;
        current.GetNode().parent = null;
		open.Add (current);

		while(open.Count > 0)
		{
			if (current == end) 
			{
                List<Vector2> newPath = new List<Vector2>();

                while(current.GetNode().parent != null)
                {
                    newPath.Add(current.GetNode().parent.transform.position);
                    current = current.GetNode().parent;
                }

				newPath.Reverse ();
                newPath.Add(end.transform.position);
                Wave_Manager.Instance.SetMyPath(routeList, newPath);

                return true;
			}

            foreach(Path_Tile temp in current.GetNode().GetPathFindingNodes())
            {
                if (!closed.Contains(temp))
                {
                    if (!open.Contains(temp))
                    {
                        temp.GetNode().SetScores(end, current);
                        temp.GetNode().parent = current;
                        temp.GetNode().gScore += current.GetNode().gScore;
                        open.Add(temp);
                    }
                    else if (open.Contains(temp))
                    {
                        if(temp.GetNode().gScore < current.GetNode().gScore + temp.GetNode().SetGScore(current))
                        {
                            temp.GetNode().parent = current;
                            temp.GetNode().gScore = temp.GetNode().SetGScore(current) + current.GetNode().gScore;
                        }
                    }
                }
            }

            open.Sort((x, y) => x.GetNode().FScore.CompareTo(y.GetNode().FScore));

            open.Remove(current);
            closed.Add(current);

            if (open.Count <= 0)
                break;

            if (open.Find(x => x.railIndex == currentRail) == true)
            {
                current = open.Find(x => x.railIndex == currentRail);
            }
            else
            {
                if (currentRail < 2)
                {
                    current = open.Find(x => x.railIndex == currentRail + 2);
                }
                else if (currentRail > 1)
                {
                    current = open.Find(x => x.railIndex == currentRail - 2);
                }
                else
                {
                    current = open[0];
                }
            }
		}

		return false;
	}
    
    private void ResetVariables()
    {
        openGrid.Clear();
        closedGrid.Clear();
        pathfindingTiles.Clear();
    }

    public override void BeginNewState()
    {
        GameManager.StateChangeEvent -= BeginNewState;
    }

    public override void InGameStateChange()
    {
        switch (GameManager.Instance.InGameState)
        {
            case CS_Enum.IN_GAME_STATE.NOT_ACTIVE:
                ResetVariables();
                break;
            case CS_Enum.IN_GAME_STATE.STANDBY:
                SetLists();
                InitTiles();
                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:

                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:

                break;
        }
    }
}
