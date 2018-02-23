using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiked_Wall : Wall
{
    public override void MyInit()
    {
        _cost = -75;
        targetType = CS_Enum.TARGET_TYPE.AOE;
        damageType = CS_Enum.DAMAGE_TYPE.PHYSICAL;
        damage = 2.0f;

        canAttack = true;
    }

    public override void CustomUpgrade()
    {
        
    }

    public override void Ability(List<GameObject> enemy)
    {
        Debug.Log("hit");

        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            Player_Manager.Instance.SendDamage(damageType, damage, enemiesInRange[i]);
        }
    }
}
