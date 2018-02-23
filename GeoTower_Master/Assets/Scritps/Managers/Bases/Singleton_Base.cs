using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton_Base<T> : Manager_Base where T : Component
{
	protected static T _instance;
	public static T Instance
	{
		get{ 
			if (_instance == null) {
				_instance = FindObjectOfType<T> ();
				if (_instance == null) {
					GameObject go = new GameObject ();
					go.name = typeof(T).Name;
					_instance = go.AddComponent<T> ();
				}
			}

			return _instance;
		}
	}

	public override void Init()
	{
		if(_instance == null)
		{
			_instance = this as T;
			base.Init ();
		}
		else
		{
			Debug.Log ("Dup Destroyed");
			Destroy (gameObject);
		}
	}
}
