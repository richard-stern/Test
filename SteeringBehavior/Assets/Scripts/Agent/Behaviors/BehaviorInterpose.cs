using UnityEngine;
using System.Collections;

public class BehaviorInterpose : BehaviorBase
{
	private GameObject m_oObjectA = null;
	private GameObject m_oObjectB = null;
	private BehaviorArrive m_sArrive = null;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorInterpose(Agent sAgent, float fWeighting, GameObject oObjectA, GameObject oObjectB): base(sAgent, fWeighting)
	{
		m_oObjectA = oObjectA;
		m_oObjectB = oObjectB;

		m_sArrive = new BehaviorArrive(sAgent, 0.0f, Vector3.zero);
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		Rigidbody rObjectA = m_oObjectA.GetComponent<Rigidbody>();
		Rigidbody rObjectB = m_oObjectB.GetComponent<Rigidbody>();

		if(!rObjectA || !rObjectB)
			return Vector3.zero;

		//Calculate the current point between the two objects and how long it will take to get there.
		Vector3 v3MidPoint = (rObjectA.position + rObjectB.position) * 0.5f;
		float fTimeToMidPoint = Vector3.Distance(m_sAgent.transform.position, v3MidPoint) / m_sAgent.GetMaxSpeed();

		//Calculate the future point between the two objects.
		Vector3 v3PosA = rObjectA.position + rObjectA.velocity * fTimeToMidPoint;
		Vector3 v3PosB = rObjectB.position + rObjectB.velocity * fTimeToMidPoint;
		v3MidPoint = (v3PosA + v3PosB) * 0.5f;

		m_sArrive.SetTargetPos(v3MidPoint);
		return m_sArrive.Calculate();
	}
}