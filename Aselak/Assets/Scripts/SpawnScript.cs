using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject mainCamera;
    private GameObject _player;
	// Use this for initialization
	void Awake () {
        _player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        mainCamera.GetComponent<AselakCameraFollow>().Player = _player.transform;
	}
    private void Start()
    {
        GameManager.instance.player = _player.GetComponent<BallScript>();
    }
}
