using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Path
{
    //Housing for the enemies paths. Won't be edited during the round and will be initalized during the prep phase.

	public Vector2 current;
	public List<Vector2> pathNodes;

	public CS_Path()
	{
		pathNodes = new List<Vector2> ();
	}

	public void SetPath(List<Vector2> newPath)
	{
		pathNodes = newPath;
		current = pathNodes [0];
	}
}
