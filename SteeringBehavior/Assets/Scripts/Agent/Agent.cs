using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//----------------------------------------------------------------------
//----------------------------------------------------------------------
public class Agent : MonoBehaviour
{
	private Rigidbody m_RigidBody = null;
	private float m_fMaxSpeed = 3.0f;

	List<BehaviorBase> m_lBehaviorList = new List<BehaviorBase>();

	//----------------------------------------------------------------------
	// Use this for initialization
	//----------------------------------------------------------------------
	void Awake()
	{
		m_RigidBody = GetComponent<Rigidbody>();
        Debug.Log("weeeeeeee");
	}
	
    //This is a note that Ben wrote.

	//----------------------------------------------------------------------
	// Update is called once per frame
	//----------------------------------------------------------------------
	void Update()
	{
		//Calculate combined total of all steering forces.
		Vector3 v3SteeringForce = Calculate();

		//Convert steering force into acceleration, taking mass into account.
		Vector3 v3Acceleration = v3SteeringForce / m_RigidBody.mass;

		//Add the acceleration onto the objects velocity.
		Vector3 v3Velocity = m_RigidBody.velocity + (v3Acceleration * Time.deltaTime);

		//Limit velocity so we don't exceed the object's max speed.
		m_RigidBody.velocity = Vector3.ClampMagnitude(v3Velocity, m_fMaxSpeed);
		if(m_RigidBody.velocity.magnitude > 0.01f)
		{
			Vector3 v3Facing = m_RigidBody.velocity;
			v3Facing.y = 0.0f;
			v3Facing.Normalize();
			if(v3Facing.magnitude > 0.0f)
				transform.forward = v3Facing;
		}
		else
		{
			//A comment
			m_RigidBody.velocity = Vector3.zero;
		}
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	Vector3 Calculate()
	{
		Vector3 v3SteeringForce = Vector3.zero;
		Vector3 v3Force;

		foreach(BehaviorBase sBehavior in m_lBehaviorList)
		{
			if (sBehavior.GetEnabled())
			{
				v3Force = sBehavior.Calculate() * sBehavior.GetWeighting();
				if (!AccumulateForce(ref v3SteeringForce, v3Force))
					return v3SteeringForce;
			}
		}

		return v3SteeringForce;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	bool AccumulateForce(ref Vector3 v3RunningTotal, Vector3 v3ForceToAdd)
	{
		float fMagnitude = v3RunningTotal.magnitude;
		float fMagnitudeRemaining = m_fMaxSpeed - fMagnitude;

		if(fMagnitudeRemaining <= 0.0f)
			return false;

		float fMagnitudeToAdd = v3ForceToAdd.magnitude;
		if(fMagnitudeToAdd < fMagnitudeRemaining)
		{
			v3RunningTotal += v3ForceToAdd;
		}
		else
		{
			v3RunningTotal += v3ForceToAdd.normalized * fMagnitudeRemaining;
		}

		return true;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public Rigidbody GetRigidBody()
	{
		return m_RigidBody;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public float GetMaxSpeed()
	{
		return m_fMaxSpeed;
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public void AddBehavior(BehaviorBase sBehavior)
	{
		m_lBehaviorList.Add(sBehavior);
	}
}
