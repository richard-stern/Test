using UnityEngine;
using System.Collections;

public abstract class BaseState
{
	public virtual void OnEnter(GameObject oObject) { }
	public virtual void OnExit(GameObject oObject) { }
	public abstract void OnReason(GameObject oObject);
	public abstract void OnAct(GameObject oObject);
}
