using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Placeable_Base : Tower_Base 
{
	public override void Init ()
	{
		base.Init ();
		Type = CS_Enum.TILE_TYPE.PLACE;
	}
}
