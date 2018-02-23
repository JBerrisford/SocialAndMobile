using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Spitter : Fixed_Base
{
    public override void MyInit()
    {
		damage = 7.5f;

		damageType = CS_Enum.DAMAGE_TYPE.FIRE;
		targetType = CS_Enum.TARGET_TYPE.AOE;

		attackRate = 1.5f;
        specialRate = 5.0f;
        _cost = -100;
        _upgradeCost = -150;
    }

    public override void CustomUpgrade()
    {
        //Upgrades.Add("Upgrade 1", Blue_Fire_Spitter);
        //Upgrades.Add("Upgrade 2", Fire_Ball_Spitter);

        upgradeNames = new string[2];

        upgradeNames[0] = ("Blue Flame " + _upgradeCost);
        upgradeNames[1] = ("Fire Balls " + _upgradeCost);
    }

    private void Blue_Fire_Spitter()
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Towers/BlueFireSpitter"), transform.position, Quaternion.identity) as GameObject;
        ReplaceTower(go);
    }

    private void Fire_Ball_Spitter()
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Towers/FireBallSpitter"), transform.position, Quaternion.identity) as GameObject;
        ReplaceTower(go);
    }

    public override void Ability(List<GameObject> enemy)
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            Player_Manager.Instance.SendDamage(damageType, damage, enemiesInRange[i]);
        }
    }

    public override void Special()
    {
        
    }

    public override List<GameObject> GetTarget()
    {
        return enemiesInRange;
    }
}
 