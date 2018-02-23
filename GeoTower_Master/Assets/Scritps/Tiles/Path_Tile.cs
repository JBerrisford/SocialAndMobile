using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Tile : Tile_Base, IPathable
{
    //For use within the inspector
	public bool isStartPoint;
    public bool isEndPoint;
    public int railIndex;
    //----------------------------

    private CS_Node node;

	public override void Init()
	{
		base.Init ();
		Type = CS_Enum.TILE_TYPE.PATH;
		_placePoint = transform.position;
		node = new CS_Node(_placePoint, railIndex);
    }

	public CS_Node GetNode ()
	{
		return node;
	}

    public int GetIndex()
    {
        return railIndex;
    }

    public void ResetNode()
    {
        node.Reset();
    }
}
