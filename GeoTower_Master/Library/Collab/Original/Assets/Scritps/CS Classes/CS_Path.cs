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
		//Reverse only works for the first half of the wave. Why?
		pathNodes = newPath;
		PathSort ();
	}

	private void PathSort()
	{
		//BUGGED - One the second round of sorting a wave, lists are reversed from their reveersed versions.
		current = pathNodes [0];
		Debug.Log (current);

	}
}
