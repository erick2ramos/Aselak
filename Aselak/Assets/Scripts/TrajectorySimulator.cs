using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectorySimulator : MonoBehaviour {
    public LineRenderer linePath;
    public BallScript player;
    [Range(0.00f, 4.00f)]
    public float scale = 1f;
    public LayerMask layerMask;
    public int segmentCount = 60;

    private Vector3 initSimulation;
    private Collider hitObject;
    private bool active = false;
    private Vector3 previousDir;
    private float previousBurst;

    void Start () {
        linePath = GetComponent<LineRenderer>();
	}
	
	void FixedUpdate () {
		if(player != null)
        {
            if (!active)
            {
                active = true;
                initSimulation = player.transform.position;
            }
            SimulatePath();
        }
	}

    void SimulatePath()
    {
        Vector3[] segments = new Vector3[segmentCount];

        segments[0] = initSimulation;
        Vector3 velDirection = player.pInput.normalized * player.pBurst;
        hitObject = null;

        if (previousBurst == player.pBurst && previousDir == player.pInput)
        {
            return;
        } else
        {
            previousBurst = player.pBurst;
            previousDir = player.pInput;
        }

        for(int i = 1;  i < segmentCount; i++)
        {
            Collider[] hits = Physics.OverlapSphere(segments[i - 1], 0.3f, layerMask);
            if(hits.Length > 0)
            {
                hitObject = hits[0];
                if(hitObject.gameObject.tag == "Threshold")
                {
                    // Get gravity vector
                    Vector3 gravDistance = -(segments[i - 1] - hitObject.transform.position);
                    float grav = gravDistance.sqrMagnitude != 0 ? 
                        hitObject.gameObject.GetComponent<ThresholdScript>().gravity / (gravDistance.sqrMagnitude) :
                        0;
                    Vector3 gravPull = gravDistance.normalized * (grav / 10f);
                    
                    // Apply gravity to previous velocity direction
                    velDirection += gravPull;
                    if (gravDistance.sqrMagnitude <= 0.5f)
                    {
                        velDirection = Vector3.zero;
                    }
                }
            }

            float segTime = velDirection.sqrMagnitude != 0 ? scale / velDirection.magnitude : 0;
            segments[i] = segments[i - 1] + (velDirection * scale / 10f);
        }
        linePath.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
            linePath.SetPosition(i, segments[i]);
    }
}
