using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Manager : Singleton_Base<AI_Manager>
{
    public Dictionary<GameObject, Enemy_Base> activeUnits = new Dictionary<GameObject, Enemy_Base>();

	public override void Init ()
	{
		base.Init ();
	}

    public void AddUnit(GameObject newEnemyGO, Enemy_Base newEnemyEB)
    {
        activeUnits.Add(newEnemyGO, newEnemyEB);
    }

    private void PrepWave()
    {
        foreach(KeyValuePair<GameObject, Enemy_Base> temp in activeUnits)
        {
            temp.Value.Init();
			temp.Value.SetPath(Wave_Manager.Instance.GetMyPath(temp.Value.RailIndex, temp.Value.CanFlyCheck()));
        }
    }

    private void StartWave()
    {
		float delay = 0.0f;

        foreach (KeyValuePair<GameObject, Enemy_Base> temp in activeUnits)
        {
            temp.Value.StartCoroutine(temp.Value.Movement(delay));
            delay += 1.0f;
        }
    }

    public void DealDamage(CS_Enum.DAMAGE_TYPE type, float damage, GameObject hit)
    {
        if (activeUnits.ContainsKey(hit))
            StartCoroutine(activeUnits[hit].TakeDamage(type, damage));
    }

    public void HitPlayer(int damage)
    {
        Player_Manager.Instance.TakeDamage(damage);
    }

    public void GivePlayerMoney(int value, GameObject source)
    {
        Player_Manager.Instance.ModifyMoney(value);
        activeUnits.Remove(source);
        UnitCheck();
    }

    public void DestroyMe(GameObject source)
    {
        activeUnits.Remove(source);
        UnitCheck();
    }

    private void UnitCheck()
    {
        if(activeUnits.Count <= 0 && GameManager.Instance.InGameState != CS_Enum.IN_GAME_STATE.REST_PHASE)
        {
            GameManager.Instance.NewInGameState(CS_Enum.IN_GAME_STATE.END_WAVE);
        }
    }

    private void ResetVariables()
    { 
        activeUnits.Clear();
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
                PrepWave();
                StartWave();
                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:
                ResetVariables();
                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:

                break;
        }
    }
}
