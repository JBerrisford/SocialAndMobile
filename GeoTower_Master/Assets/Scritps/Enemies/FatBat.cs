using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBat : Flying_Base
{
    public override void Init()
    {
        base.Init();

        health = 150.0f;
        movementSpeed = 0.75f;
        damage = 1;
        value = 10;
    }
}
