using UnityEngine;
using System.Collections;

public class BehaviorSeek : BehaviorBase
{
	private Vector3 m_v3TargetPos = Vector3.zero;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorSeek(Agent sAgent, float fWeighting, Vector3 v3Target): base(sAgent, fWeighting)
	{
		m_v3TargetPos = v3Target;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{ 
		//Calculate direction to target.
		Vector3 v3Dir = m_v3TargetPos - m_sAgent.transform.position;
		v3Dir.Normalize();

		//Work out fastest velocity.
		Vector3 v3DesiredVelocity = v3Dir * m_sAgent.GetMaxSpeed();

		//Calculate the steering force so that the objects seeks gradually 
		//towards the target over time instead of going directly towards it.
		Vector3 v3SteeringForce = v3DesiredVelocity - m_sAgent.GetRigidBody().velocity;

		return v3SteeringForce;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void SetTargetPos(Vector3 v3Pos)
	{
		m_v3TargetPos = v3Pos;
	}
}