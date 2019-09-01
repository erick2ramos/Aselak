using UnityEngine;
using System.Collections;

public class GravMassScript : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BallScript>().Die();
        }
    }
}
