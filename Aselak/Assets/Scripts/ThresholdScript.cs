using UnityEngine;
using System.Collections;

public class ThresholdScript : MonoBehaviour {
    // public
    public float gravity = 9.0f;
    [Range(0,50)]
    public int segments = 50;
    
    // private
	private float radius;
    private LineRenderer lr;

	void Start () {
		radius = GetComponent<SphereCollider> ().radius;
        lr = GetComponent<LineRenderer>();
        lr.positionCount = segments + 1;
        lr.useWorldSpace = false;
        CreateSegments();
        lr.enabled = true;
	}

    void CreateSegments()
    {
        float x, z;

        float angle = 20f;

        for(int i = 0; i < segments + 1; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lr.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }

	void OnTriggerStay(Collider other){
        if (other != null)
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                Vector3 diff = this.transform.position - other.gameObject.transform.position;
			    Vector3 diffDir = diff.normalized;
			    float force = (gravity) / (diff.sqrMagnitude);
                
                Debug.DrawLine(other.transform.position, other.GetComponent<Rigidbody>().velocity + other.transform.position, Color.green);
                other.GetComponent<Rigidbody>().velocity += (diffDir * force / 50f);
                Debug.DrawLine(other.transform.position, (diff * force / 50f) + other.transform.position, Color.red);
            }
		}
	}
}
