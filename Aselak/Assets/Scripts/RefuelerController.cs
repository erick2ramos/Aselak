using System.Collections;
using UnityEngine;
using System;

public class RefuelerController : MonoBehaviour {
    public GameObject model;
    private bool rotating = true;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            DockShip(other.gameObject);
            rotating = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            rotating = true;
        }
    }

    private void Update()
    {
        if(rotating)
            model.transform.Rotate(Vector3.forward, 10 * Time.deltaTime);
    }

    private void DockShip(GameObject ship)
    {
        BallScript bs = ship.GetComponent<BallScript>();
        Rigidbody rb = bs.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        StartCoroutine(Dock(ship, () =>
        {
            bs.Refuel();
        }));
    }

    IEnumerator Dock(GameObject ship, Action callback)
    {
        Transform tf = ship.GetComponent<Transform>();
        while ((tf.position - transform.position).sqrMagnitude >= 0.01f)
        {
            tf.position = Vector3.Lerp(tf.position, transform.position, 0.04f);
            yield return new WaitForEndOfFrame();
        }

        callback();
    }
}
