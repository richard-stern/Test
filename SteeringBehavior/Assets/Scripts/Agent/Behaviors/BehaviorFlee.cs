using UnityEngine;
using System.Collections;

public class BehaviorFlee : BehaviorBase
{
	private Vector3 m_v3TargetPos = Vector3.zero;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorFlee(Agent sAgent, float fWeighting, Vector3 v3Target): base(sAgent, fWeighting)
	{
		m_v3TargetPos = v3Target;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		//Calculate direction away from the target.
		Vector3 v3Dir = m_sAgent.transform.position - m_v3TargetPos;
		v3Dir.Normalize();

		//Work out fastest velocity.
		Vector3 v3DesiredVelocity = v3Dir * m_sAgent.GetMaxSpeed();

		//Calculate the steering force so that the objects flees gradually 
		//away from the target over time.
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