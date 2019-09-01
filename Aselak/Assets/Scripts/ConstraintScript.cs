using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintScript : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BallScript>().Die();
        } else if(other.gameObject.tag == "Asteroid")
        {
            Destroy(other.gameObject);
        }
    }
}
