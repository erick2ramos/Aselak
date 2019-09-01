using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {
    public GameObject[] screens;
    public float seconds;

    private GameObject canvas;
    private GameObject overlay;

    private GameObject current;

	// Use this for initialization
	void Start () {
        canvas = gameObject;
        overlay = new GameObject("Overlay");
        overlay.transform.SetParent(canvas.transform);
        Image overlayImg = overlay.AddComponent<Image>();
        overlayImg.color = Color.black;
        RectTransform rt = overlay.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0,0);
        rt.anchorMax = new Vector2(1,1);
        rt.anchoredPosition3D = Vector2.zero;

        StartCoroutine(SetSplash());
	}

    IEnumerator SetSplash()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("MainMenu");
        async.allowSceneActivation = false;
        foreach (GameObject img in screens)
        {
            overlay.GetComponent<Image>().canvasRenderer.SetAlpha(1);
            overlay.GetComponent<Image>().CrossFadeAlpha(0, 0.5f, false);
            current = img;
            img.SetActive(true);
            yield return new WaitForSeconds(seconds);

            overlay.GetComponent<Image>().canvasRenderer.SetAlpha(0);
            overlay.GetComponent<Image>().CrossFadeAlpha(1, 0.5f, false);
            yield return new WaitForSeconds(1f);

            current.SetActive(false);
        }
        async.allowSceneActivation = true;
    }
}
