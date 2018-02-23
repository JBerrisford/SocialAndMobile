using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CS_Enum
{
	public enum GAME_STATE
	{
		INIT = 0,
		MAIN_MENU,
		NEW,
		LOAD,
		SAVE,
		START_GAME,
		IN_GAME,
        SETTINGS,
        CREDITS,
        GAME_OVER,
		EXIT
	};

	public enum IN_GAME_STATE
	{
        NOT_ACTIVE = 0,
		STANDBY,
		REST_PHASE,
		START_WAVE,
		END_WAVE,
		PAUSED
	};

	public enum TILE_TYPE
	{
		FIXED = 0,
		PLACE,
		PATH
	};

	public enum DAMAGE_TYPE
	{
		PHYSICAL = 0,
		ARCANE,
		FIRE,
		FROST,
        POISON
	};

	public enum TARGET_TYPE
	{
		FIRST = 0,
		LAST,
		AOE
	};

    public enum ENEMY_TYPE
    {
        GROUND = 0,
        FLYING
    };
}
