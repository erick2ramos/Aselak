using UnityEngine;
using System.Collections;

public class CollectableScript: MonoBehaviour {
	public int points;
    private GameObject _playerUI;
    [SerializeField]
    private AudioClip collectSound;
    private Transform model;
    private Vector2 randomize;

    void Awake()
    {
        _playerUI = GameObject.Find("Canvas/PlayerUI/TopBar/Collectables");
        model = transform.Find("Model");
        randomize = new Vector2(Random.Range(2,10), Random.Range(2,10));
    }


    private void Update()
    {
        model.Rotate(new Vector3(0f, randomize.x * Time.deltaTime, randomize.y * Time.deltaTime));
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player")
        {
            BallScript bs = other.GetComponent<BallScript>();
            bs.Score(gameObject);
            _playerUI.GetComponent<CollectablesBarController>().Collect();
            SoundManager.instance.PlaySingle(collectSound, pitch: 3f);
            Destroy(gameObject);
        }
	}
}
