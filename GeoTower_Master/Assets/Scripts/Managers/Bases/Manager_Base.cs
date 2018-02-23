using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager_Base : MonoBehaviour, IInit
{
	public virtual void Init()
	{
        GameManager.StateChangeEvent += BeginNewState;
        GameManager.InGameStateChangeEvent += InGameStateChange;
	}

    public abstract void InGameStateChange();
    public abstract void BeginNewState();
}
