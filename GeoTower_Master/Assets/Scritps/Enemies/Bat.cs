using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Flying_Base
{
    public override void Init()
    {
        base.Init();

        health = 100.0f;
        movementSpeed = 1.0f;
        damage = 1;
        value = 10;
    }
}
