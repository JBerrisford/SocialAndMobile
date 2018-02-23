using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Ball_Spitter : Fixed_Base
{
    public override void MyInit()
    {
        _cost = -150;
    }

    public override void Ability(List<GameObject> enemy)
    {

    }

    // Use this for initialization
    public override void CustomUpgrade()
    {

    }

    public override void Special()
    {

    }

    public override List<GameObject> GetTarget()
    {
        List<GameObject> targets = new List<GameObject>();

        return targets;
    }
}
