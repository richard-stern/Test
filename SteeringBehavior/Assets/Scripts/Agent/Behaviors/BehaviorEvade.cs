using UnityEngine;
using System.Collections;

public class BehaviorEvade : BehaviorBase
{
	private Agent m_sTarget = null;
	private BehaviorFlee m_sFlee = null;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorEvade(Agent sAgent, float fWeighting, Agent sTarget): base(sAgent, fWeighting)
	{
		m_sTarget = sTarget;
		m_sFlee = new BehaviorFlee(sAgent, 0.0f, Vector3.zero);
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		Vector3 v3TargetsPos = m_sTarget.transform.position;
		Rigidbody targetRB = m_sTarget.GetRigidBody();
		Vector3 v3TargetVelocity = targetRB.velocity;

		//Work out the direction to the target.
		Vector3 v3ToTarget = v3TargetsPos - m_sAgent.transform.position;

		//Not heading straight towards us, so predict where the target is going.
		//Look-ahead time is proportional to the distance between the persuer and target
		//and inversly proprotional to the sum of their velocities.
		float fLookAheadTime = v3ToTarget.magnitude / (m_sAgent.GetMaxSpeed() + v3TargetVelocity.magnitude);

		m_sFlee.SetTargetPos(v3TargetsPos + (v3TargetVelocity * fLookAheadTime));
		return m_sFlee.Calculate();
	}
}