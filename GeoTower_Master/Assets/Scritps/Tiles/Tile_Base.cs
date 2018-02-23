using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Tile_Base : MonoBehaviour, IInit, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public GameObject highLightBlue;
	public GameObject highLightRed;

	protected CS_Enum.TILE_TYPE _type;
	public CS_Enum.TILE_TYPE Type
	{
		set {_type = value; }
		get {return _type;}
	}

	private bool _isOpen;
	public bool IsOpen
	{
		set{_isOpen = value;}
		get{return _isOpen;}
	}
		
	protected Tower_Base _tower;
    public Tower_Base Tower
    {
        set { _tower = value; }
    }

    protected Vector3 _placePoint;
    public Vector3 PlacePoint
    {
        get{ return _placePoint; }
    }

	public virtual void Init ()
	{
		IsOpen = true;
	}

    private bool WillBlockPath(Tile_Base tileToCheck)
    {
        bool tempBool = false;

        foreach (Path_Tile temp in tileToCheck.GetComponent<IPathable>().GetNode().GetTowerBlockingNodes())
        {
            if (!temp.IsOpen)
            {
                if (temp.railIndex != this.GetComponent<IPathable>().GetIndex())
                {
                    tempBool = true;
                }
            }
        }

        if (!IsOpen)
            tempBool = true;

        if (tempBool)
            return true;
        else
            return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_tower != null && !Player_Manager.Instance.IsBuilding)
        {
            Player_Manager.Instance.SelectTower(_tower);
        }
        else if(!Player_Manager.Instance.IsBuilding)
        {
            Player_Manager.Instance.DeselectTower();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
	{
        if (Player_Manager.Instance.IsBuilding)
        {
            Player_Manager.Instance.NewPosition(PlacePoint, this);

            if (Player_Manager.Instance.GetTowerType() == CS_Enum.TILE_TYPE.PATH && GameManager.Instance.InGameState == CS_Enum.IN_GAME_STATE.REST_PHASE)
            {
                if(Player_Manager.Instance.CurrentTile is IPathable && !WillBlockPath(Player_Manager.Instance.CurrentTile))
                {
                    HighlightSelf(true, true);
                    Player_Manager.Instance.CanBuild = true;
                }
                else
                {
                    HighlightSelf(true, false);
                    Player_Manager.Instance.CanBuild = false;
                    UI_Manager.Instance.ErrorMsg(CS_Strings.cantBlockPath);
                }
            }
            else if (Player_Manager.Instance.GetTowerType() == Type && _isOpen)
            {
                HighlightSelf(true, true);
                Player_Manager.Instance.CanBuild = true;
            }
            else
            {
                HighlightSelf(true, false);
                Player_Manager.Instance.CanBuild = false;
            }  
        }
        else
        {
            HighlightSelf(true, true);
        }
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HighlightSelf (false, false);

        if (Player_Manager.Instance.IsBuilding)
        {
            Player_Manager.Instance.CanBuild = false;
            Player_Manager.Instance.NewPosition(new Vector3(50, 50, 0), null);
        }
    }

	public void HighlightSelf(bool canHighlight, bool isBlue)
	{
		if(canHighlight)
		{
			if (isBlue) 
			{
				highLightBlue.SetActive(true);
				highLightRed.SetActive(false);
			} 
			else if (!isBlue) 
			{
				highLightBlue.SetActive(false);
				highLightRed.SetActive(true);
			}	
		}
		else if(!canHighlight)
		{
			highLightBlue.SetActive(false);
			highLightRed.SetActive(false);
		}
	}

    public void CloseNode(Tower_Base newTower)
    {
        IsOpen = false;
        _tower = newTower;
        Map_Manager.Instance.AddToClosed(this);
    }

	public void OpenNode()
	{
		IsOpen = true;
		_tower = null;
		Map_Manager.Instance.AddToOpen (this);
	}
}
