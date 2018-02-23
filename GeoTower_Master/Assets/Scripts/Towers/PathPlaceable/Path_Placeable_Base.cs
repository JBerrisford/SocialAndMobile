using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathPlaceable_Base : Tower_Base, IWall
{
	public override void Init ()
	{
		base.Init ();
		Type = CS_Enum.TILE_TYPE.PATH;
	}
}
