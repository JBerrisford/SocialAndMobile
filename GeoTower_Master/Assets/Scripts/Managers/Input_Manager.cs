using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : Singleton_Base<Input_Manager> 
{
    private bool canInput;

    public override void Init ()
	{
		base.Init ();
        canInput = false;
	}

	void Update () 
	{
        if (canInput)
        {
            if (Input.GetMouseButtonDown(0)) // Left Click
            {
                if (Player_Manager.Instance.IsBuilding)
                {
                    Player_Manager.Instance.Build();
                }
                else if(GameManager.Instance.GameState == CS_Enum.GAME_STATE.IN_GAME)
                {
                   //Player_Manager.Instance.DeselectTower();
                }
            }
            if (Input.GetMouseButtonDown(1)) // Right Click
            {
                if (Player_Manager.Instance.IsBuilding)
                {
                    Player_Manager.Instance.CancelBuild();
                }
            }
        }
	}

    public override void BeginNewState()
    {
        switch (GameManager.Instance.GameState)
        {
            case CS_Enum.GAME_STATE.MAIN_MENU:
                canInput = false;
                break;
            case CS_Enum.GAME_STATE.NEW:

                break;
            case CS_Enum.GAME_STATE.LOAD:
                canInput = false;
                break;
            case CS_Enum.GAME_STATE.SAVE:
                canInput = false;
                break;
            case CS_Enum.GAME_STATE.START_GAME:
                GameManager.Instance.NewGameState(CS_Enum.GAME_STATE.IN_GAME);
                break;
            case CS_Enum.GAME_STATE.IN_GAME:

                break;
            case CS_Enum.GAME_STATE.GAME_OVER:
                canInput = false;
                break;
            case CS_Enum.GAME_STATE.EXIT:
                canInput = false;
                break;
        }
    }

    public override void InGameStateChange()
    {
        switch (GameManager.Instance.InGameState)
        {
            case CS_Enum.IN_GAME_STATE.NOT_ACTIVE:
                canInput = false;
                break;
            case CS_Enum.IN_GAME_STATE.STANDBY:
                canInput = true;
                GameManager.Instance.NewInGameState(CS_Enum.IN_GAME_STATE.REST_PHASE);
                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:
                GameManager.Instance.NewInGameState(CS_Enum.IN_GAME_STATE.REST_PHASE);
                break;
        }
    }
}
