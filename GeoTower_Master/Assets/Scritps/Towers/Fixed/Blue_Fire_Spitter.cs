using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Fire_Spitter : Fire_Spitter
{
    public override void MyInit()
    {
        base.MyInit();

        damage *= 1.5f;
        attackRate *= 0.75f;
        specialRate *= 0.75f;
        _cost = -150;
    }

    public override void CustomUpgrade()
    {
        //Upgrades.Remove("Upgrade 1");
        //Upgrades.Remove("Upgrade 2");
    }
}
