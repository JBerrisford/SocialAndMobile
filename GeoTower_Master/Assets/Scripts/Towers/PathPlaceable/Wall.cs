using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : PathPlaceable_Base 
{
    public override void MyInit()
    {
        _cost = -50;
        _upgradeCost = -75;
        canAttack = false;
    }

    public override void CustomUpgrade()
    {
        //Upgrades.Add("Upgrade 1", SlowWall);
        //Upgrades.Add("Upgrade 2", SpikedWall);

        upgradeNames = new string[2];

        upgradeNames[0] = ("Slowing Wall " + _upgradeCost);
        upgradeNames[1] = ("Damage Wall " + _upgradeCost);
    }

    private void SlowWall()
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Towers/SlowWall"), transform.position, Quaternion.identity) as GameObject;
        ReplaceTower(go);
    }

    private void SpikedWall()
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Towers/SpikedWall"), transform.position, Quaternion.identity) as GameObject;
        ReplaceTower(go);
    }

    public override List<GameObject> GetTarget()
    {
        return enemiesInRange;
    }

    public override void Ability(List<GameObject> enemy)
    {

    }
}
