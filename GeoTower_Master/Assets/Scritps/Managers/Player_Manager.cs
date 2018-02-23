using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Manager : Singleton_Base<Player_Manager>
{
    public delegate void Toggle(bool canToggle);
    public event Toggle ToggleHighlights; 

    List<Tower_Base> activeTowers = new List<Tower_Base>();

    const int HEALTH = 20;
    private int _health;
    public int Health
    {
        get { return _health; }
    }

    const int MONEY = 500;
    private int _money;
    public int Money
    {
        get { return _money; }
    }

    private Tower_Base selectedTower;
    private Tile_Base _currentTile;
    public Tile_Base CurrentTile
    {
        get{ return _currentTile; }
    }

	private bool _isBuilding;
	public bool IsBuilding
	{
		set{_isBuilding = value; }
		get{ return _isBuilding;}
	}

	private bool _canBuild;
	public bool CanBuild
	{
		set{_canBuild = value;}
		get{ return _canBuild;}
	}

    public bool CanBuildWalls
    { 
        get { return _canBuild; }
    }

	public override void Init ()
	{
		base.Init ();
        _health = HEALTH;
        _money = MONEY;
	}

    public void UpgradeTower(string key)
    {
        if ((_money + selectedTower.UpgradeCost) >= 0)
        {
            selectedTower.Upgrade(key);
        }
        else
        {
            UI_Manager.Instance.ErrorMsg(CS_Strings.cantAffordUpgrade);
        }
    }

    public void SelectTower(Tower_Base go)
    {
        if (selectedTower != null)
            DeselectTower();

        BeginToggle(false);
        selectedTower = go;
        selectedTower.ToggleHightlight(true);
        UI_Manager.Instance.UpdateTowerUI(selectedTower);
    }

    public void BeginToggle(bool canToggle)
    {
        ToggleHighlights(canToggle);
        UI_Manager.Instance.UpdateTowerUI(selectedTower);
    }

    public void DeselectTower()
    {
        if (ToggleHighlights != null)
            BeginToggle(false);

        selectedTower = null;  
        UI_Manager.Instance.UpdateTowerUI(null);
    }

	public void SetTowerToBuild(Tower_Base go)
	{
        if(ToggleHighlights != null)
            BeginToggle(false);

        if (IsBuilding)
        {
            CancelBuild();
        }

        if (GameManager.Instance.InGameState != CS_Enum.IN_GAME_STATE.REST_PHASE && go is IWall)
        {
            UI_Manager.Instance.ErrorMsg(CS_Strings.cantPlaceWallsNow);
        }
        else
        {
            selectedTower = go;
            IsBuilding = true;
        }
	}

	public CS_Enum.TILE_TYPE GetTowerType()
	{
		return selectedTower.Type;
	}

	public void NewPosition (Vector3 newPos, Tile_Base newTile)
	{
		selectedTower.transform.position = newPos;
		_currentTile = newTile;
	}

	public void Build()
	{
        if (_canBuild)
        {
            IsBuilding = false;
            selectedTower.AddDefaultTile(_currentTile);
            //ModifyMoney(selectedTower.Cost);
            NotBuilding();
        }
        else if (!_canBuild && _currentTile != null) 
		{
            UI_Manager.Instance.ErrorMsg(CS_Strings.cantPlaceHere);
		}
        else if (!_canBuild && _currentTile == null)
        {
            UI_Manager.Instance.ErrorMsg(CS_Strings.selectATileToPlace);
        }
    }

	public void CancelBuild()
	{
		IsBuilding = false;
		Destroy (selectedTower.gameObject);
		NotBuilding ();
	}

	void NotBuilding ()
	{
		CanBuild = false;
		selectedTower = null;
		_currentTile = null;
	}

	public void AddTower(Tower_Base towerToAdd, bool canNodeAction)
	{
		activeTowers.Add (towerToAdd);

        if (canNodeAction)
        {
            Map_Manager.Instance.NodeAction(towerToAdd, false);
        }

        ModifyMoney(towerToAdd.Cost);
	}

    public void RemoveTower(Tower_Base tower, bool canRefund)
    {
        if (activeTowers.Contains(tower))
        {
            activeTowers.Remove(tower);

            if (canRefund)
                ModifyMoney((tower.Cost / 2) * -1);

            DeselectTower();
        }
    }

	public void SendDamage(CS_Enum.DAMAGE_TYPE type, float damage, GameObject target)
	{
		AI_Manager.Instance.DealDamage (type, damage, target);
	}

    public void TakeDamage(int damageToTake)
    {
        _health -= damageToTake;
        if(_health <= 0)
        {
            UI_Manager.Instance.GameOver(false);
        }
    }

    public void ModifyMoney(int value)
    {
        _money += value;
    }

    private void ResetVariables()
    {


        GameObject tempTower;

        for(int i = 0; i < activeTowers.Count; i++)
        {
            tempTower = activeTowers[i].gameObject;
            activeTowers.Remove(activeTowers[i]);
            Destroy(tempTower);
        }

        activeTowers.Clear();
        NotBuilding();

        _money = MONEY;
        _health = HEALTH;
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

                break;
            case CS_Enum.IN_GAME_STATE.REST_PHASE:
                break;
            case CS_Enum.IN_GAME_STATE.START_WAVE:
                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:
                foreach (Tower_Base temp in activeTowers)
                    temp.ResetEnemies();
                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:

                break;
        }
    }
}
