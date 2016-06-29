using UnityEngine;
using System.Collections;

public class BehaviorPersue : BehaviorBase
{
	private GameObject m_oTarget = null;
	private BehaviorSeek m_sSeek = null;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorPersue(Agent sAgent, float fWeighting, GameObject oTarget): base(sAgent, fWeighting)
	{
		m_oTarget = oTarget;
		m_sSeek = new BehaviorSeek(sAgent, 0.0f, Vector2.zero);
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		Transform tTargetTransform = m_oTarget.transform;
		Vector3 v3TargetsPos = tTargetTransform.position;
		Rigidbody targetRB = m_oTarget.GetComponent<Rigidbody>();
		Vector3 v3TargetVelocity = targetRB.velocity;

		//Work out the direction to the target.
		Vector3 v3ToTarget = v3TargetsPos - m_sAgent.transform.position;

		//Check if the target is heading straight towards us.
		//acos(0.95)=18 degrees
		float fAngle = 0.95f;
		float fRelativeHeading = Vector3.Dot(m_sAgent.transform.forward, tTargetTransform.forward);
		float fTargetHeading = Vector3.Dot(v3ToTarget, m_sAgent.transform.forward);
		if(fTargetHeading > 0.0f && fRelativeHeading < -fAngle) 
		{
			m_sSeek.SetTargetPos(v3TargetsPos);
			return m_sSeek.Calculate();
		}

		//Not heading straight towards us, so predict where the target is going.
		//Look-ahead time is proportional to the distance between the persuer and target
		//and inversly proprotional to the sum of their velocities.
		float fLookAheadTime = v3ToTarget.magnitude / (m_sAgent.GetMaxSpeed() + v3TargetVelocity.magnitude);

		m_sSeek.SetTargetPos(v3TargetsPos + (v3TargetVelocity * fLookAheadTime));
		return m_sSeek.Calculate();
	}
}