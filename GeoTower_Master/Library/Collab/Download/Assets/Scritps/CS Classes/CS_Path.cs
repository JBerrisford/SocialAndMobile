using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Path
{
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
