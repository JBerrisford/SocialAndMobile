using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Current project uses a lot of data coupling in the scripts and should be transfered over to
 * the interfaces and their functions. This will just make it easier to interact with scripts
 * as well as making it easier to store their references within the manager mainly. 
 */

public interface IWall
{

}

public interface IPathable
{
    CS_Node GetNode();
    int GetIndex();
}

public interface IInit
{
    void Init();
}

public interface IInGameStateChange
{
    void InGameSubscribe();
    void InGameStateChange();
}

public interface IEnemyAdvanced
{
    void Ability();
    void DropItem(); //<-- Stretch Goal
}

public interface IEnemy
{
    IEnumerator TakeDamage(CS_Enum.DAMAGE_TYPE damageType, float damage);
    void DeathCheck();
    void SetPath(List<Vector2> newPath);
}
