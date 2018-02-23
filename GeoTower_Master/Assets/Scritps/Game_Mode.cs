using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Mode : Manager_Base
{ 
	public override void Init()
	{
        base.Init();
	}

    public override void BeginNewState()
    {
        switch (GameManager.Instance.GameState)
        {
            case CS_Enum.GAME_STATE.IN_GAME:
                
                break;
        }
    }

    public override void InGameStateChange()
    {
        switch (GameManager.Instance.InGameState)
        {
            case CS_Enum.IN_GAME_STATE.NOT_ACTIVE:

                break;
            case CS_Enum.IN_GAME_STATE.STANDBY:

                break;
            case CS_Enum.IN_GAME_STATE.REST_PHASE:

                break;
            case CS_Enum.IN_GAME_STATE.START_WAVE:

                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:

                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:

                break;
            default:
                Debug.Log(gameObject.name + " - Failed Begin In-Game State Switch");
                break;
        }
    }

    //Divide into sub classes for seperation of concerns.
    /*void Normal()
	{
		//Wooo
	}

	void Hard()
	{
		
	}

	void Endless()
	{
		
	}*/
}
