using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable_Tile : Tile_Base 
{
	public override void Init()
	{
		base.Init ();
		Type = CS_Enum.TILE_TYPE.PLACE;
		_placePoint = transform.position;
	}
}
