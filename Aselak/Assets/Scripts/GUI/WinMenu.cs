using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour {
    // Sets up the win/loose menu animating score, updating text
    // according to winning conditions
    public Text title;
    public GameObject starsPanel;
    public GameObject starUIPrefab;
    public Button nextLevelBtn;

    private GameObject[] stars;

    public void SetupMenu(int finalScore, int maxScore, bool died)
    {
        // Title change on conditions
        title.text =  died || finalScore <= 0  ? "Fail" : "Success";
        // Posible actions: retry always, next level if finalScore > 0 and not died
        if (!died && finalScore > 0)
        {
            nextLevelBtn.gameObject.SetActive(true);
        }

        // Panel instantiate stars
        stars = new GameObject[maxScore];
        for(int i = 0; i < maxScore; i++)
        {
            GameObject star = Instantiate(starUIPrefab, starsPanel.GetComponent<RectTransform>(), false);
            stars[i] = star;
        }
    }

    public void GrantStars(int finalScore)
    {
        // Timed animate stars
        for (int i = 0; i < finalScore; i++)
        {
            if(stars[i] != null)
                stars[i].GetComponent<StarUIController>().Grant();
        }
    }
}
