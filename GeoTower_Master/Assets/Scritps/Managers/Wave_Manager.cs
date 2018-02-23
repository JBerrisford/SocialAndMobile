using System.Collections.Generic;
using UnityEngine;

public class Wave_Manager : Singleton_Base<Wave_Manager>
{
	Dictionary<int, List<Vector2>> groundEnemyRails = new Dictionary<int, List<Vector2>>();
    Dictionary<int, List<Vector2>> flyingEnemyRails = new Dictionary<int, List<Vector2>>();

    List<Path_Tile> startPoints = new List<Path_Tile> ();
	List<Path_Tile> endPoints = new List<Path_Tile> ();

    CS_EasyWaves myWaves;
    public string GetWaveCurrentMax
    {
        get { return myWaves.wavesCurrent.ToString() + " / " + myWaves.wavesMax.ToString(); }
    }

	public override void Init ()
	{
		base.Init ();
        myWaves = new CS_EasyWaves();
	}

    public void InitLists()
    {
        ResetVariables();

        GameObject[] ep = GameObject.FindGameObjectsWithTag("EndPoint");
        GameObject[] sp = GameObject.FindGameObjectsWithTag("StartPoint");

        foreach (GameObject temp in sp)
        {
            Path_Tile pt = temp.GetComponent<Path_Tile>();

            foreach (Path_Tile ptn in pt.GetNode().neighbours)
            {
                if(ptn.isStartPoint)
                    startPoints.Add(ptn);
            }
        }

        foreach (GameObject temp in ep)
        {
            Path_Tile pt = temp.GetComponent<Path_Tile>();

            if (pt.isEndPoint)
                endPoints.Add(pt);
        }

        startPoints.Sort((x, y) => x.railIndex.CompareTo(y.railIndex));
        endPoints.Sort((x, y) => x.railIndex.CompareTo(y.railIndex));

        int i = 0;

        foreach (Path_Tile temp in endPoints)
        {
            if(endPoints.IndexOf(temp) % 2 == 0)
            {
                Map_Manager.Instance.GeneratePathfinding (startPoints[0], endPoints[i], endPoints.IndexOf (temp));
            }
			else if (endPoints.IndexOf(temp) % 2 == 1)
            {
                Map_Manager.Instance.GeneratePathfinding(startPoints[1], endPoints[i], endPoints.IndexOf(temp));
            }

            i++;
		}

        if(flyingEnemyRails.Count <= 0)
        {
            foreach(KeyValuePair<int, List<Vector2>> temp in groundEnemyRails)
            {
                flyingEnemyRails.Add(temp.Key, temp.Value);
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

    public List<string> SelectWave()
    {
        List<string> enemiesToSpawn = new List<string>();
        enemiesToSpawn = myWaves.Waves[myWaves.wavesCurrent];
        return enemiesToSpawn;
    }

    public void SpawnNextWave()
    {
        int x = 3;

        foreach (string tempEnemy in SelectWave())
        {
            x++;

            if (x >= endPoints.Count)
            {
                x = 0;
            }

            if (x % 2 == 0)
            {
                SpawnUnit(tempEnemy, startPoints[0], x);
            }
            else
            {
                SpawnUnit(tempEnemy, startPoints[1], x);
            }
        }
    }

    private void SpawnUnit(string unit, Path_Tile startPos, int railIndex)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load(unit), startPos.transform.position, Quaternion.identity);
        Enemy_Base newEnemy = go.GetComponent<Enemy_Base>();

        newEnemy.RailIndex = railIndex;
        AI_Manager.Instance.AddUnit(go, newEnemy);
    }

	public List<Vector2> GetMyPath(int pathIndex, bool isFlying)
	{
        if (isFlying)
        {
            return flyingEnemyRails[pathIndex];
        }

        return groundEnemyRails[pathIndex];
	}

    public void SetMyPath(int pathIndex, List<Vector2> newPath)
    {
        groundEnemyRails.Add(pathIndex, newPath);
    }

    private void ResetVariables()
    {
        //flyingEnemyRails.Clear();
        groundEnemyRails.Clear();
        startPoints.Clear();
        endPoints.Clear();
        myWaves.ResetWaves();
    }

    public override void BeginNewState()
    {
        GameManager.StateChangeEvent -= BeginNewState;
    }

    public void WinCheck()
    {
        if(myWaves.wavesCurrent > myWaves.wavesMax)
        {
            UI_Manager.Instance.GameOver(true);
        }
    }

    public override void InGameStateChange()
    {
        switch (GameManager.Instance.InGameState)
        {
            case CS_Enum.IN_GAME_STATE.NOT_ACTIVE:
                print("NotActive Wave - hit");
                ResetVariables();
                break;
            case CS_Enum.IN_GAME_STATE.STANDBY:
                print("StandBy Wave - hit");
                InitLists();
                break;
            case CS_Enum.IN_GAME_STATE.REST_PHASE:
                SpawnNextWave();
                break;
            case CS_Enum.IN_GAME_STATE.START_WAVE:
                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:
                WinCheck();
                myWaves.wavesCurrent++;
                Player_Manager.Instance.ModifyMoney(100);
                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:

                break;
        }
    }
}
