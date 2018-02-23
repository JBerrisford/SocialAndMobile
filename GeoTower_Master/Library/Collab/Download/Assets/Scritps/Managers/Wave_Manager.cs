using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Manager : Singleton_Base<Wave_Manager>
{
	List<Vector2> flyingEnemyRail = new List<Vector2> ();

	public Dictionary<int, List<Vector2>> groundEnemyRails = new Dictionary<int, List<Vector2>>();

	List<Path_Tile> startPoints = new List<Path_Tile> ();
	List<Path_Tile> endPoints = new List<Path_Tile> ();

	public override void Init ()
	{
		base.Init ();
	}

    private void InitLists()
    {
        endPoints.Add(GameObject.FindGameObjectWithTag("EndPoint").GetComponent<Path_Tile>());
        GameObject[] sp = GameObject.FindGameObjectsWithTag("StartPoint");

        foreach (GameObject temp in sp)
        {
            Path_Tile pt = temp.GetComponent<Path_Tile>();

            foreach (Path_Tile ptn in pt.GetNode().neighbours)
            {
                if (!startPoints.Contains(ptn))
                    startPoints.Add(ptn);
            }
        }

        foreach (Path_Tile temp in startPoints)
        {
            if (Map_Manager.Instance.GeneratePathfinding(temp, endPoints[0], startPoints.IndexOf(temp)))
            {
                Debug.Log("Path Made");
            }
            else
            {
                Debug.Log("Failed to make path");
            }
        }
    }
	
	public void GetPath(Path_Tile start, Path_Tile end, int railIndex)
	{
		Map_Manager.Instance.GeneratePathfinding (start, end, railIndex);
	}

	public void FindNewPath()
	{
        ResetVariables();
        InitLists();
	}

    public void SpawnTestWave()
    {
        int x = 0;

		foreach (Path_Tile temp in startPoints) 
		{
			GameObject go = (GameObject)Instantiate (Resources.Load ("Prefabs/Enemies/Test"), temp.transform.position, Quaternion.identity);
			Enemy_Base newEnemy = go.GetComponent<Enemy_Base> ();

			newEnemy.RailIndex = x;
			AI_Manager.Instance.AddUnit (newEnemy);

			x++;
		}
    }

	public List<Vector2> GetMyPath(int pathIndex)
	{
		return groundEnemyRails[pathIndex];
	}

    public void SetMyPath(int pathIndex, List<Vector2> newPath)
    {
        groundEnemyRails.Add(pathIndex, newPath);
    }

    private void ResetVariables()
    {
        flyingEnemyRail.Clear();
        groundEnemyRails.Clear();
        startPoints.Clear();
        endPoints.Clear();
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
                InitLists();
                break;
            case CS_Enum.IN_GAME_STATE.REST_PHASE:
                //Select next wave                
                //Spawn Next Wave
                SpawnTestWave();
                break;
            case CS_Enum.IN_GAME_STATE.START_WAVE:

                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:
                //Give player money for the wave
                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:

                break;
        }
    }
}
