using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {
    public Vector3 spawnDirection;
    public float spawnVelocity;
    public GameObject prefabToSpawn;
    public float spawnCooldown;
    public float startDelay;
    public Animator anim;

    private float cooldownTimer;
    private GameObject holder;

    private void Start()
    {
        spawnDirection = spawnDirection.normalized;
        holder = new GameObject("Holder");
        holder.transform.SetParent(transform);
        transform.Find("Model").rotation = Quaternion.Euler(0, Mathf.Atan2(spawnDirection.z, spawnDirection.x) * Mathf.Rad2Deg, 0);
        if(startDelay > 0)
        {
            cooldownTimer = Time.time + startDelay;
        }
    }

    private void Update()
    {
        // Check if the cooldown time has passed and spawns another object prefab
        // with starting velocity, prefab must have rigidbody
        if(Time.time > cooldownTimer)
        {
            cooldownTimer = Time.time + spawnCooldown;
            Spawn();
        }
    }

    // Spawns a prefab that must have a rigidbody to assign the spawner velocity
    public void Spawn()
    {
        anim.Play("SpawnObject");
        GameObject go = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        go.transform.SetParent(holder.transform);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if(rb != null)
            rb.velocity = spawnDirection * spawnVelocity;
    }
}
