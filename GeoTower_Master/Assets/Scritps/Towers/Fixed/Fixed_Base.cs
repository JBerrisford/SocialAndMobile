using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fixed_Base : Tower_Base
{
    protected bool canSpecial;
    protected bool isSpecialing;
    protected float specialRate;

    public override void Init()
    {
        base.Init();
        Type = CS_Enum.TILE_TYPE.FIXED;
        canSpecial = true;
        isSpecialing = false;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !enemiesInRange.Contains(collision.gameObject))
        {
            enemiesInRange.Add(collision.gameObject);

            if (!isAttacking && canAttack && IsPlaced)
            {
                AttackCo = StartCoroutine(AttackLoop());
            }
        }
    }

    public override IEnumerator AttackLoop()
    {
        isSpecialing = true;

        if (CanSpecial())
        {
            canSpecial = false;
            Special();
            StartCoroutine(SpecialReset());
        }
        else
            isSpecialing = false;

        
        yield return base.AttackLoop();
    }

    public virtual bool CanSpecial()
    {
        if (canSpecial && HasTarget())
            return true;

        return false;
    }

    public virtual IEnumerator SpecialReset()
    {
        yield return new WaitForSeconds(specialRate);
        canSpecial = true;
    }

    public abstract void Special();
}
