using UnityEngine;
using System.Collections;

public class BehaviorWander : BehaviorBase
{
	private float m_fWanderRadius = 50.0f;
	private float m_fWanderDistance = 1.0f;
	private float m_fWanderJitter = 180.15f;
	private Vector3 m_v3WanderTarget = Vector3.zero;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorWander(Agent sAgent, float fWeighting): base(sAgent, fWeighting)
	{
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		///Randomly adjust wander target by a random amount.
		Vector3 v3Offset = new Vector3(UnityEngine.Random.value * m_fWanderJitter, 0.0f, UnityEngine.Random.value * m_fWanderJitter);
		m_v3WanderTarget += v3Offset;

		//Scale wander target to the desiered radius.
		m_v3WanderTarget.Normalize();
		m_v3WanderTarget *= m_fWanderRadius;

		Vector3 v3Target = m_sAgent.transform.position + (m_sAgent.transform.forward * m_fWanderDistance) + m_v3WanderTarget;

		return v3Target - m_sAgent.transform.position;
	}
}