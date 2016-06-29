using UnityEngine;
using System.Collections;

public class BehaviorOffsetPersuit : BehaviorBase
{
	private GameObject m_oLeader = null;
	private Vector3 m_v3Offset = Vector3.zero;
	private BehaviorArrive m_sArrive = null;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorOffsetPersuit(Agent sAgent, float fWeighting, GameObject oLeader, Vector3 v3Offset): base(sAgent, fWeighting)
	{
		m_oLeader = oLeader;
		m_v3Offset = v3Offset;
		m_sArrive = new BehaviorArrive(sAgent, 0.0f, Vector3.zero);
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		Rigidbody rLeader = m_oLeader.GetComponent<Rigidbody>();
		if(!rLeader)
			return Vector3.zero;

		//Convert offset to world space
		Vector3 v3TargetPos = m_oLeader.transform.TransformVector(m_v3Offset);

		//Calculate direction to target offset.
		Vector3 v3Dir = v3TargetPos - m_sAgent.transform.position;

		//Look ahead to predict where the leader is moving.
		float fLookAheadTime = v3Dir.magnitude / m_sAgent.GetMaxSpeed() + rLeader.velocity.magnitude;

		m_sArrive.SetTargetPos(v3TargetPos + rLeader.velocity * fLookAheadTime);
		return m_sArrive.Calculate();
	}
}