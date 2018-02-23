using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed_Tile : Tile_Base 
{
	public override void Init()
	{
		base.Init ();
		Type = CS_Enum.TILE_TYPE.FIXED;
		_placePoint = transform.position - new Vector3 (0, 0, 0);
	}
}
