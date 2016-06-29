using UnityEngine;
using System.Collections;

public class BehaviorArrive : BehaviorBase
{
	private Vector3 m_v3TargetPos = Vector3.zero;

	private float m_fStoppingDist = 5.0f;
	private float m_fStoppingSpeed = 10.0f;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorArrive(Agent sAgent, float fWeighting, Vector3 v3Target): base(sAgent, fWeighting)
	{
		m_v3TargetPos = v3Target;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		//Calculate direction to target.
		Vector3 v3Dir = m_v3TargetPos - m_sAgent.transform.position;
		float fDist = v3Dir.magnitude;

		if(fDist > 0.05f)
		{
			//Calculate speed based on how close the object is to its target.
			//Slowing to a stop gradually as is approaches.
			float fSpeed = m_sAgent.GetMaxSpeed() * (fDist / m_fStoppingDist);
			fSpeed = Mathf.Min(fSpeed, m_sAgent.GetMaxSpeed());

			//Work out velocity.
			Vector3 v3DesiredVelocity = v3Dir * fSpeed / fDist;

			//Calculate the steering force so that the objects seeks gradually 
			//towards the target over time instead of going directly towards it.
			Vector3 v3SteeringForce = (v3DesiredVelocity - m_sAgent.GetRigidBody().velocity);

			//If we're slowing down, scale up the stopping speed drastically to bring things to a stop in time.
			if(fSpeed - m_sAgent.GetRigidBody().velocity.magnitude < 0.0f)
				v3SteeringForce *= m_fStoppingSpeed;

			return v3SteeringForce;
		}
		else return Vector3.zero;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void SetTargetPos(Vector3 v3Pos)
	{
		m_v3TargetPos = v3Pos;
	}
}