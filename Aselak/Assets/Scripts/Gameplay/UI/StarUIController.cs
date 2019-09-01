using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUIController : MonoBehaviour {

    public bool active;
    private Animator _anim;

    void Awake()
    {
        active = false;
        _anim = GetComponent<Animator>();
    }

    // Sets active to true, and play animation when the player
    // collects a star
    public void Grant()
    {
        active = true;
        _anim.Play("Grant");
    }
}
