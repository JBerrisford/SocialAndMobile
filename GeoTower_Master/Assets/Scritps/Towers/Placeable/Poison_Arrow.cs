using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Arrow : Arrow_Tower
{
    public override void MyInit()
    {
        print("Init Hit");
        attackRate = 0.3f;
        damage = 10.0f;
        targetType = CS_Enum.TARGET_TYPE.FIRST;
        damageType = CS_Enum.DAMAGE_TYPE.POISON;
        _cost = -75;
        canAttack = true;
    }

    public override void CustomUpgrade()
    {

    }
}
