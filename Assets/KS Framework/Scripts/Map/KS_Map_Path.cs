using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KS_Map_Path : MonoBehaviour {

    public NavMeshAgent agent;
    LineRenderer line;

	// Use this for initialization
	void Start () {

        KS_FullMap.WayPointMarkerAdded += OnWaypoint;
        KS_FullMap.WayPointRemoved += OnWayPointRemoved;

        line = this.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
        }
    }
	
    void OnWaypoint(Vector3 position)
    {
        agent.SetDestination(position);
    }

    void OnWayPointRemoved()
    {
        agent.ResetPath();
        /*line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);*/
    }

	// Update is called once per frame
	void FixedUpdate () {
		if(agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            DrawPath(agent.path);
        }

	}

    private void DrawPath(NavMeshPath path)
    {
        var line = this.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
        }

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }
    }
}
