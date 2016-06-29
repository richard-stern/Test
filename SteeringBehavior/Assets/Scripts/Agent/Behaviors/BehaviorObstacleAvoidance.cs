using UnityEngine;
using System.Collections;

public class BehaviorObstacleAvoidance : BehaviorBase
{
	private float m_fAvoidanceLength = 6.0f;
	private float m_fAvoidanceWidth = 3.0f;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorObstacleAvoidance(Agent sAgent, float fWeighting): base(sAgent, fWeighting)
	{
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		float fSpeed = m_sAgent.GetRigidBody().velocity.magnitude;
		float fBoxLength = Mathf.Max(1.0f, (fSpeed / m_sAgent.GetMaxSpeed()) * m_fAvoidanceLength);

		//List<GameObject> lObstaclesWithinRange = new List<GameObject>();
		GameObject oClosestObject = null;
		float fDistToIntersection = float.MaxValue;
		Vector3 v3ClosestLocalPos = Vector3.zero;
		float fClosestRadius = 0.0f;
		
		//Iterate through all objects in the scene. (Could be optimised by first culling based on distance, or using a quadtree)
		GameObject[] aoObstacles = GameObject.FindGameObjectsWithTag("Obstacles");
		foreach(GameObject oObject in aoObstacles)
		{
			float fDist = Vector3.Distance(oObject.transform.position, m_sAgent.transform.position);
			if(fDist > fBoxLength)
				continue;

			Collider oCollider = oObject.GetComponent<Collider>();
			if(!oCollider)
				continue;

			//Convert to local space
			Vector3 v3LocalPos = m_sAgent.transform.InverseTransformPoint(oObject.transform.position);

			//We only need to test against objects with a local Z greater or equal to zero.
			//Other objects can be culled for efficiency.
			if(v3LocalPos.z >= 0.0f)
			{
				//Check to see if the collision box potentially overlaps.
				//Calculate the radius of each object, then expand the radius by half the width of the collision box.
				float fRadius = Mathf.Max(oCollider.bounds.size.x, oCollider.bounds.size.z);
				float fExpandedRadius = fRadius + (m_fAvoidanceWidth * 0.5f);

				//If the local X value is less than the expanded radius, there could be a collision.
				if(Mathf.Abs(v3LocalPos.x) < fExpandedRadius)
				{
					//Line/circle intersection test.
					float fSqrtPart = Mathf.Sqrt(fExpandedRadius * fExpandedRadius - v3LocalPos.x * v3LocalPos.x);
					float fPointZ = v3LocalPos.z - fSqrtPart;

					if(fPointZ <= 0.0f)
					{
						fPointZ = v3LocalPos.z + fSqrtPart;
					}

					if(fPointZ < fDistToIntersection)
					{
						fDistToIntersection = fPointZ;
						oClosestObject = oObject;
						v3ClosestLocalPos = v3LocalPos;
						fClosestRadius = fRadius;
					}
				}
			}
		}

		//Calculate Steering force
		Vector3 v3SteeringForce = Vector3.zero;
		if(oClosestObject)
		{
			//Closer the object, the stronger the force should be.
			float fMultiplier = 5.0f + (fBoxLength - v3ClosestLocalPos.z) / fBoxLength;

			//Calculate laterial force.
			v3SteeringForce.x = (fClosestRadius - v3ClosestLocalPos.x) * fMultiplier;
			Debug.DrawLine(m_sAgent.transform.position, m_sAgent.transform.TransformPoint(v3SteeringForce));

			//Calculate breaking force.
			float fBreakingStrength = 0.2f;
			v3SteeringForce.z = (fClosestRadius - v3ClosestLocalPos.z) * fBreakingStrength;

			//Convert the steering force to world space and return it.
			v3SteeringForce = m_sAgent.transform.TransformPoint(v3SteeringForce);
		}

		return v3SteeringForce;
	}
}