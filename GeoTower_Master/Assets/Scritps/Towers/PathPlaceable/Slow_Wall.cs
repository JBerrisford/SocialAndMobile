using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_Wall : Wall
{
    public override void MyInit()
    {
        _cost = -75;
    }

    public override void CustomUpgrade()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
