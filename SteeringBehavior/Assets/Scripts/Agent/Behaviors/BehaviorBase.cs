using UnityEngine;
using System.Collections;

public abstract class BehaviorBase
{
	protected Agent m_sAgent = null;

	private bool m_bEnabled = false;
	private float m_fWeighting = 0.0f;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorBase(Agent sAgent, float fWeighting)
	{
		m_sAgent = sAgent;
		m_fWeighting = fWeighting;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public abstract Vector3 Calculate();

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public bool GetEnabled()
	{
		return m_bEnabled;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void SetEnabled(bool bEnabled)
	{
		m_bEnabled = bEnabled;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public float GetWeighting()
	{
		return m_fWeighting;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void SetWeighting(float fWeighting)
	{
		m_fWeighting = fWeighting;
	}
}