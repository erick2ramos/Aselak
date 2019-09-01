using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour {
    public Vector3 startingDirection;
    public float speed;
    public GameObject model;

    private Rigidbody rb;
    private Quaternion eulerRotations;

	// Update is called once per frame
	void Awake () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = (startingDirection.normalized * speed);
        eulerRotations = Random.rotation;
	}

    void Update()
    {
        model.transform.Rotate(eulerRotations.eulerAngles * Time.deltaTime * 0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BallScript>().Die();
        }
    }
}

