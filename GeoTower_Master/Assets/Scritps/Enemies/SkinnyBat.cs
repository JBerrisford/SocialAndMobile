using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnyBat : Flying_Base
{
    public override void Init()
    {
        base.Init();

        health = 50.0f;
        movementSpeed = 1.25f;
        damage = 1;
        value = 10;
    }
}
