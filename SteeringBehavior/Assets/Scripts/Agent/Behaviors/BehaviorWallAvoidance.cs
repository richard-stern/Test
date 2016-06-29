using UnityEngine;
using System.Collections;

public class BehaviorWallAvoidance : BehaviorBase
{
	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorWallAvoidance(Agent sAgent, float fWeighting): base(sAgent, fWeighting)
	{
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		float fRayDist = 5.0f;
		Vector3 v3SteeringForce = Vector3.zero;

		//Work out 3 rays to feel ahead.
		Vector3[] av3Rays = new Vector3[3];
		av3Rays[0] = Vector3.Normalize(m_sAgent.transform.forward - m_sAgent.transform.right);
		av3Rays[1] = m_sAgent.transform.forward;
		av3Rays[2] = Vector3.Normalize(m_sAgent.transform.forward + m_sAgent.transform.right);

		float fDistToWall = float.MaxValue;
		GameObject oClosestWall = null;
		Vector3 v3CollisionPoint = Vector3.zero;
		Vector3 v3Normal = Vector3.zero;

		int nLayerMask = LayerMask.NameToLayer("Wall");

		//Send out three rays to check for walls
		for(int i = 0; i < av3Rays.Length; ++i)
		{
			RaycastHit hit;
			Debug.DrawLine(m_sAgent.transform.position, m_sAgent.transform.position + av3Rays[i] * fRayDist);
			if(Physics.Raycast(m_sAgent.transform.position, av3Rays[i], out hit, fRayDist, nLayerMask))
			{
				//record the closest impact point
				if(hit.distance < fDistToWall)
				{
					fDistToWall = hit.distance;
					oClosestWall = hit.collider.gameObject;
					v3CollisionPoint = hit.point;
					v3Normal = hit.normal;
				}
			}

			//Calculate steering force
			if(oClosestWall)
			{
				Vector3 v3RayEnd = m_sAgent.transform.position + av3Rays[i] * fRayDist;
				Vector3 v3Overshoot = v3RayEnd - v3CollisionPoint;

				//Vector3 v3PredictedPos = m_sAgent.transform.position + (m_sAgent.transform.forward * fRayDist);
				//Vector3 v3Overshoot = v3PredictedPos - v3CollisionPoint;

				v3SteeringForce = v3Normal * v3Overshoot.magnitude;
			}
		}

		return v3SteeringForce;
	}
}