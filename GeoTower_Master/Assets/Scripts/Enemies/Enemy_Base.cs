using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Base : MonoBehaviour, IEnemy
{
	protected float health;
    protected float movementSpeed;
    protected int damage;
    protected int value;

	protected CS_Path path;

    protected CS_Enum.ENEMY_TYPE myType;
	protected CS_Enum.DAMAGE_TYPE resistence;
    private PolygonCollider2D cb;
    private SpriteRenderer sr;

    private bool canDamage;

	private int _railIndex;
	public int RailIndex
	{
		set{_railIndex = value;}
		get{return _railIndex;}
	}

	public virtual void Init()
	{
        cb = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        path = new CS_Path ();
        canDamage = true;

        myType = CS_Enum.ENEMY_TYPE.GROUND;
	}

	public void FindNewPath()
	{
		SetPath (Wave_Manager.Instance.GetMyPath(_railIndex, CanFlyCheck()));
	}

	public void SetPath(List<Vector2> newPath)
	{
		path.SetPath (newPath);
	}

    public IEnumerator TakeDamage (CS_Enum.DAMAGE_TYPE damageType, float damageToTake)
	{
		if(damageType != resistence)
		{
			health -= damageToTake;
		}
		else
		{
			health -= (damageToTake / 2);
		}

		DeathCheck ();

		yield return null;
	}

	public void DeathCheck ()
	{
        if (health <= 0.0f)
        {
            AI_Manager.Instance.GivePlayerMoney(value, this.gameObject);
            StartCoroutine(Death());
        }
        else if (health < 25)
            sr.color = Color.red;
        else if (health < 50)
            sr.color = new Color(1.0f, 0.65f, 0.0f, 1.0f);
        else if (health < 75)
            sr.color = Color.yellow;
    }

    private void WaitForDeath()
    {
        canDamage = false;
        sr.enabled = false;
        cb.enabled = false;
    }

    public IEnumerator Death()
    {
        WaitForDeath();      
        yield return new WaitForSeconds(0.5f);
        NaturalDeath();
    }

    private void NaturalDeath()
    {
        AI_Manager.Instance.DestroyMe(this.gameObject);
        Destroy(this.gameObject);
    }

	public IEnumerator Movement(float delay)
    {      
        bool isMoving = true;

		yield return new WaitForSeconds (delay);
        cb.enabled = true;

        while (isMoving)
        {
            if (path.current == path.pathNodes[path.pathNodes.Count - 1])
            { 
                isMoving = false;
                break;
            }

            Vector2 lerpStart = path.current;
            Vector2 lerpEnd = path.pathNodes[path.pathNodes.IndexOf(path.current) + 1];

            float t = 0.0f;

            while (t < 1.0f)
            {   
                t += 0.025f * movementSpeed;
                transform.position = Vector2.Lerp(lerpStart, lerpEnd, t);
                yield return new WaitForFixedUpdate();
            }

            path.current = lerpEnd;
        }

        if (canDamage)
        {
            AI_Manager.Instance.HitPlayer(damage);
            NaturalDeath();
        }
    }

    public bool CanFlyCheck()
    {
        if (myType == CS_Enum.ENEMY_TYPE.FLYING)
            return true;

        return false;
    }
}
