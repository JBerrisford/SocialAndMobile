using System.Collections.Generic;
using UnityEngine;

public class Arrow_Tower : Placeable_Base 
{
    public GameObject arrow;

    public override void MyInit()
    {
        attackRate = 0.5f;
        damage = 7.5f;
        targetType = CS_Enum.TARGET_TYPE.FIRST;
        _cost = -50;
        _upgradeCost = -75;
    }

    public override void CustomUpgrade()
    {
        Upgrades.Add("Upgrade 1", MagicArrows);
        Upgrades.Add("Upgrade 2", PoisonArrows);

        upgradeNames = new string[2];

        upgradeNames[0] = ("Arcane Arrows " + _upgradeCost);
        upgradeNames[1] = ("Poison Arrows " + _upgradeCost);
    }

    public void MagicArrows()
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Towers/MagicArrowTower"), transform.position, Quaternion.identity) as GameObject;
        ReplaceTower(go);
    }

    public void PoisonArrows()
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Towers/PoisonArrowTower"), transform.position, Quaternion.identity) as GameObject;
        ReplaceTower(go);
    }

    public override List<GameObject> GetTarget ()
	{
        List<GameObject> enemyToHit = new List<GameObject>();

        switch(targetType)
        {
            case CS_Enum.TARGET_TYPE.FIRST:
                enemyToHit.Add(enemiesInRange[0]);
                break;
            case CS_Enum.TARGET_TYPE.LAST:
                enemyToHit.Add(enemiesInRange[enemiesInRange.Count - 1]);
                break;
        }

        return enemyToHit;
	}

	public override void Ability (List<GameObject> enemy)
	{
        GameObject go = Instantiate(arrow, transform.position, Quaternion.identity);
        go.GetComponent<Projectile_Base>().Init(this.gameObject, enemy[0], transform.position, damageType, damage);
	}
}
