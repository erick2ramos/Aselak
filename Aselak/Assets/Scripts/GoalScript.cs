using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalScript : MonoBehaviour {
	public Text _text;
	public bool finished = false;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
            //other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            int score = other.GetComponent<BallScript>().starsCollected;
            GameManager.instance.playerScore.SetScore(
                LevelManager._instance.currentLevel.id,
                LevelManager._instance.currentStage.id, 
                score
            );
            GameManager.instance.GameOver(score, false);
		}
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BallScript>().Win(gameObject);
        }
    }
}
