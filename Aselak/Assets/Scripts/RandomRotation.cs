using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour {
	void Update () {
        // Sets random rotation to acopled transform
        transform.Rotate(new Vector3(0f, 8f * Time.deltaTime, 10f * Time.deltaTime));
    }
}
