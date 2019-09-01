using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text playerLives;

    private Rect[] uiRects;
    private Slider powerSlider;
    private Button launchButton;

    // Handle directional input
    private Vector3 startPoint;
    private bool validStart;

    private Canvas thisCanvas;

    private void Start()
    {
        launchButton = transform.Find("BottomBar/LaunchButton").GetComponent<Button>();
        //powerSlider = transform.FindChild("RightBar/PowerBar").GetComponent<Slider>();
        thisCanvas = GetComponentInParent<Canvas>();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("UIInput");
        uiRects = new Rect[gos.Length];
        for(int i = 0; i < gos.Length; i++)
        {
            RectTransform rt = gos[i].GetComponent<RectTransform>();
            uiRects[i] = rt.rect;
            uiRects[i].x = uiRects[i].x >= 0 ? uiRects[i].x + rt.anchoredPosition.x : Screen.width + (uiRects[i].x + rt.anchoredPosition.x);
            uiRects[i].y = uiRects[i].y >= 0 ? uiRects[i].y + rt.anchoredPosition.y : Screen.height + (uiRects[i].y + rt.anchoredPosition.y);
        }
        launchButton.interactable = false;
        if (GameManager.instance.previousLaunch != null)
        {
            //powerSlider.value = GameManager.instance.previousLaunch.burst * 100;
            //launchButton.interactable = powerSlider.value <= 200 && powerSlider.value > 0; 
            SetDirection(GameManager.instance.previousLaunch.direction);
        }
    }

    private void Update()
    {
        playerLives.text = "x " + GameManager.instance.playerLives.ToString();

        launchButton.interactable = (GameManager.instance.player != null) && (GameManager.instance.player.Burst > 0)
            && GameManager.instance.player.CanStart;
    }

    public void SetPower(float newValue)
    {
        if(GameManager.instance.player != null)
            GameManager.instance.player.Burst = newValue / 100;
        launchButton.interactable = (newValue <= 200 && newValue > 0);
    }

    public void SetDirection(Vector3 newValue)
    {
        if(GameManager.instance.player != null)
        {
            GameManager.instance.player.Direction = newValue;
        }
    }

    public void Launch()
    {
        GameManager.instance.player.Launch();
    }

    public bool IsPointOnGUI(Vector3 point)
    {
        bool isIn = false;
        foreach (Rect rect in uiRects)
        {

            if (point.x >= rect.x && point.x <= rect.x + rect.width && 
                point.y >= rect.y && point.y <= rect.y + rect.height)
            {
                isIn = true;
            }
        }
        return isIn;
    }
}
