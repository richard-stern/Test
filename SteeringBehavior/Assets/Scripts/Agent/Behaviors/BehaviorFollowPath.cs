using UnityEngine;
using System.Collections;

public class BehaviorFollowPath : BehaviorBase
{
	private GameObject[] m_oWayPoints = null;
	private int m_nCurrentWaypoint = 0;
	private float m_fWaypointDistSq = 1.0f;
	private BehaviorSeek m_sSeek = null;

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public BehaviorFollowPath(Agent sAgent, float fWeighting, GameObject[] oWayPoints): base(sAgent, fWeighting)
	{
		m_oWayPoints = oWayPoints;
		m_sSeek = new BehaviorSeek(sAgent, 0.0f, Vector3.zero);
	}

	//----------------------------------------------------------------------
	//----------------------------------------------------------------------
	public override Vector3 Calculate()
	{
		GameObject oCurrentWP = m_oWayPoints[m_nCurrentWaypoint];
		float fDist = Vector3.Distance(oCurrentWP.transform.position, m_sAgent.transform.position);

		if(fDist < m_fWaypointDistSq)
		{
			++m_nCurrentWaypoint;
			if(m_nCurrentWaypoint >= m_oWayPoints.Length)
				m_nCurrentWaypoint = 0;
		}

		m_sSeek.SetTargetPos(oCurrentWP.transform.position);
		return m_sSeek.Calculate();
	}
}