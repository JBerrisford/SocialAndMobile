using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Tower_Base : MonoBehaviour, IInit
{
    public delegate void UpgradeType();
    public Dictionary<string, UpgradeType> Upgrades = new Dictionary<string, UpgradeType>();

    protected bool isAttacking;
	protected bool canAttack;

    protected Coroutine AttackCo;

	protected float attackRate;
	protected float damage;

	protected CS_Enum.DAMAGE_TYPE damageType;
	protected CS_Enum.TARGET_TYPE targetType;

	protected CS_Enum.TILE_TYPE _type;
	public CS_Enum.TILE_TYPE Type
	{
		set {_type = value; }
		get {return _type;}
	}

    protected int _cost;
    public int Cost
    {
        get { return _cost; }
    }

    protected int _upgradeCost;
    public int UpgradeCost
    {
        get { return _upgradeCost; }
    }

    protected bool _isPlaced;
	public bool IsPlaced
	{
		get{return _isPlaced; }
	}

	protected Tile_Base occupiedTiles;
	protected List<GameObject> enemiesInRange = new List<GameObject>();

    public GameObject Highlight;

    protected string[] upgradeNames;

	public virtual void Init()
	{
        _isPlaced = false;
		targetType = CS_Enum.TARGET_TYPE.FIRST;
        MyInit();
        UpgradeInit();
    }

    public abstract void MyInit();

    public void ToggleHightlight(bool canToggle)
    {
        Highlight.SetActive(canToggle);
    }

    public void Upgrade(string key)
    {
        Upgrades[key]();

        Player_Manager.Instance.RemoveTower(this, false);

        if (AttackCo != null)
            StopCoroutine(AttackCo);

		Player_Manager.Instance.ToggleHighlights -= ToggleHightlight;
        Destroy(this.gameObject);
    }

    public virtual void UpgradeInit()
    {
        Upgrades.Add("Destroy", DestroyTower);
        CustomUpgrade();
    }

    public abstract void CustomUpgrade();

    protected void ReplaceTower(GameObject go)
    {
        Tower_Base goTB = go.GetComponent<Tower_Base>();

        occupiedTiles.CloseNode(goTB);
        goTB.occupiedTiles = occupiedTiles;
        goTB.occupiedTiles.Tower = goTB;
        occupiedTiles = null;
        go.transform.position = transform.position;

        goTB.Init();
        goTB._isPlaced = true;
        Player_Manager.Instance.AddTower(goTB, false);
        Player_Manager.Instance.ToggleHighlights += goTB.ToggleHightlight;
    }

    public void DestroyTower()
    {
        Player_Manager.Instance.RemoveTower(this, true);

        if (AttackCo != null)
            StopCoroutine(AttackCo);

        if(occupiedTiles != null)
            occupiedTiles.OpenNode();

		Player_Manager.Instance.ToggleHighlights -= ToggleHightlight;
        Destroy(this.gameObject);
    }

    public void ResetEnemies()
    {
        enemiesInRange.Clear();
    }

	public void Place(Tile_Base sourceNode)
	{
		_isPlaced = true;
        gameObject.layer = 10;
		transform.position = sourceNode.PlacePoint;
		Player_Manager.Instance.AddTower(this, true);
        Player_Manager.Instance.ToggleHighlights += ToggleHightlight;
    }

	public void AddDefaultTile(Tile_Base defaultTile)
	{
		occupiedTiles = defaultTile;
		Place (defaultTile);
        canAttack = true;
	}

    public Tile_Base GetOccupiedTiles()
    {
        return occupiedTiles;
    }

    public string GetUpgradeName(int index)
    {
        return upgradeNames[index];
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        print("Enemy Found");

        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Flying") && !enemiesInRange.Contains(collision.gameObject))
        {
            print("Enemy Registered");
            enemiesInRange.Add(collision.gameObject);

            if (!isAttacking && canAttack && IsPlaced)
            {
                AttackCo = StartCoroutine(AttackLoop());
            }
        }
    }

	public virtual void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Enemy" && enemiesInRange.Contains(collision.gameObject))
        {
            enemiesInRange.Remove(collision.gameObject);
        }
	}

	public virtual IEnumerator AttackLoop ()
	{
		isAttacking = true;

		if (HasTarget() && canAttack) 
		{
            canAttack = false;
			Ability (GetTarget());
            StartCoroutine(AttackReset());
        } 
		else 
		{
            isAttacking = false;
            StopCoroutine(AttackCo);
		}

        yield return null;
	}

    protected virtual IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
        StartCoroutine(AttackLoop());
    }

    protected bool HasTarget()
    {
        if (enemiesInRange.Count > 0)
            return true;

        return false;
    }

    public abstract void Ability(List<GameObject> enemy);
	public abstract List<GameObject> GetTarget ();
}
