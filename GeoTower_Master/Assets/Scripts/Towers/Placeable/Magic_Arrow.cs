using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Arrow : Arrow_Tower
{
    public override void MyInit()
    {
        attackRate = 0.6f;
        damage = 15.0f;
        targetType = CS_Enum.TARGET_TYPE.FIRST;
        damageType = CS_Enum.DAMAGE_TYPE.ARCANE;
        _cost = -75;
        canAttack = true;
    }

    public override void CustomUpgrade()
    {

    }
}
