using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Base : MonoBehaviour
{
    protected CS_Enum.DAMAGE_TYPE damageType;
    protected GameObject target;
    protected Rigidbody2D rb;

    protected Vector3 origin;

    protected float movementSpeed;
    protected float damage;

    protected float lifeSpan;

    public virtual void Init(GameObject myTower, GameObject newTarget, Vector3 newOrigin, CS_Enum.DAMAGE_TYPE newDamageType, float myDamage)
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        damageType = newDamageType;
        target = newTarget;
        origin = newOrigin;
        StartCoroutine(Movement(myTower));

        //---------- DEBUG CODE -------------
        movementSpeed = 50000.0f;
        damage = myDamage;
        lifeSpan = 7.5f;
        //-----------------------------------
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.tag == target.tag)
        {
            DealDamage(GetTargets());
            Destroy(this.gameObject);
        }
    }

    public virtual void DealDamage(GameObject target)
    {
        Player_Manager.Instance.SendDamage(damageType, damage, target);
    }

    public virtual IEnumerator Movement(GameObject origin)
    {
        if (target != null)
        {
            Vector3 direction = Vector3.Normalize(target.transform.position - transform.position);
            rb.AddForce(direction * 50000.0f * Time.smoothDeltaTime);
            StartCoroutine(DeathCheck());
            yield return null;
        }
        else
            Destroy(this.gameObject);
    }

    private IEnumerator DeathCheck()
    {
        yield return new WaitForEndOfFrame();

        if (Vector3.Distance(origin, transform.position) > lifeSpan)
        {
            Destroy(this.gameObject);
        }

        StartCoroutine(DeathCheck());
    }

    public GameObject GetTargets()
    {
        return target;
    }
}
