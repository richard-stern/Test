using UnityEngine;
using System.Collections;

//----------------------------------------------------------------------
//----------------------------------------------------------------------
public class StateMachine
{
	private GameObject m_ParentObject = null;
	private BaseState[] m_States = null;
	private int m_nCurrentState = -1;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public StateMachine(int nStateCount)
	{
		m_States = new BaseState[nStateCount];
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void AddState(int nIndex, BaseState state)
	{
		if(nIndex >= m_States.Length)
		{
			Debug.LogError("State out of bounds.");
			return;
		}

		m_States[nIndex] = state;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void ChangeState(int nIndex)
	{
		if(nIndex >= m_States.Length)
		{
			Debug.LogError("State out of bounds.");
			return;
		}

		if(m_nCurrentState >= 0)
		{
			if(m_States[m_nCurrentState] != null)
				m_States[m_nCurrentState].OnExit(m_ParentObject);
		}

		m_nCurrentState = nIndex;
		if(m_nCurrentState >= 0)
		{
			if(m_States[m_nCurrentState] != null)
				m_States[m_nCurrentState].OnEnter(m_ParentObject);
		}
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void Update()
	{
		if(m_nCurrentState < 0 || m_nCurrentState >= m_States.Length)
			return;

		m_States[m_nCurrentState].OnReason(m_ParentObject);
		m_States[m_nCurrentState].OnAct(m_ParentObject);
	}
}
