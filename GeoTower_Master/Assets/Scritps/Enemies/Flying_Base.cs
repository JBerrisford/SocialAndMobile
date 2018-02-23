using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Base : Enemy_Base
{
    public override void Init()
    {
        base.Init();
        myType = CS_Enum.ENEMY_TYPE.FLYING;
    }
}
