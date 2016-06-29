using UnityEngine;
using System.Collections;

//----------------------------------------------------------------------
//----------------------------------------------------------------------
public class FollowMouse : MonoBehaviour
{
	private BehaviorSeek m_sSeek = null;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	void Awake()
	{
		Agent sAgent = GetComponent<Agent>();

		BehaviorWallAvoidance sWallAvoid = new BehaviorWallAvoidance(sAgent, 1.0f);
		//sWallAvoid.SetEnabled(true);
		sAgent.AddBehavior(sWallAvoid);

		BehaviorObstacleAvoidance sObsAvoid = new BehaviorObstacleAvoidance(sAgent, 1.0f);
		sObsAvoid.SetEnabled(true);
		sAgent.AddBehavior(sObsAvoid);

		m_sSeek = new BehaviorSeek(sAgent, 0.9f, Vector3.zero); 
		m_sSeek.SetEnabled(true);
		sAgent.AddBehavior(m_sSeek);
	}
	
	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 v3TargetPos = Input.mousePosition;
			v3TargetPos.z = Camera.main.transform.position.y;
			v3TargetPos = Camera.main.ScreenToWorldPoint(v3TargetPos);
			//Debug.DrawLine(Vector3.zero, v3TargetPos);

			m_sSeek.SetTargetPos(v3TargetPos);
		}
	}
}
