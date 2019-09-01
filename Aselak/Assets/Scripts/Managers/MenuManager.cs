using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Handles every UI menu panel shown in screen
public class MenuManager : MonoBehaviour {

    public static MenuManager _instance;
    private RectTransform canvas;
    private GameObject loadedScreensParent;

    [SerializeField]
    private Stack<PanelMenu> loadedMenus;

	void Awake () {
		if(_instance == null)
        {
            _instance = this;
        } else
        {
            Destroy(gameObject);
        }
        SetUp();
        DontDestroyOnLoad(gameObject);
	}

    // Sets up the manager pointers
    private void OnLevelWasLoaded(int level)
    {
        _instance.SetUp();
    }

    // Initialize the screen manager by seaching the canvas
    public void SetUp()
    {
        loadedMenus = new Stack<PanelMenu>();
        loadedScreensParent = new GameObject("LoadedMenus");
        RectTransform parent = GameObject.Find("Canvas").GetComponent<RectTransform>();
        GameObject go = new GameObject("UIMenu");
        canvas = go.AddComponent<RectTransform>();
        canvas.anchorMin = new Vector2(0, 0);
        canvas.anchorMax = new Vector2(1, 1);
        canvas.anchoredPosition = new Vector2(0, 0);
        canvas.pivot = new Vector2(1 , 1);
        canvas.sizeDelta = new Vector2(0, 0);
        canvas.SetParent(parent, false);
    }

    // Instantiates a new screen prefab and stacks it
    public PanelMenu ShowScreen(GameObject menuPrefab, UnityAction listener = null, bool forceHide = true) {
        GameObject menu = Instantiate(menuPrefab, canvas, false);
        PanelMenu m = new GameObject("Menu").AddComponent<PanelMenu>();
        m.transform.SetParent(loadedScreensParent.transform);
        if(listener != null)
            m.OnMenuShown.AddListener(listener);

        // Setup the new menu object and put canvas as its parent
        m.SetUp(canvas, menu);
        if(loadedMenus.Count > 0 && forceHide)
        {
            loadedMenus.Peek().OnMenuHidden.AddListener(
                () => {
                    loadedMenus.Push(m);
                    loadedMenus.Peek().Show();
                }
            );
            loadedMenus.Peek().Hide();
        } else
        {
            loadedMenus.Push(m);
            loadedMenus.Peek().Show();
        }
        return m;
    }

    // Hides and deletes the screen on top of the stack and sets as active the next
    // if there is one
    public void BackScreen(UnityAction listener = null) {
        if (loadedMenus.Count > 0)
        {
            PanelMenu topScreen = loadedMenus.Pop();

            if (listener != null)
            {
                topScreen.OnMenuHidden.AddListener(listener);
            }
            topScreen.OnMenuHidden.AddListener(
                () =>
                {
                    if(loadedMenus.Count > 0)
                    {
                        loadedMenus.Peek().Show();
                    }
                    Destroy(topScreen.gameObject);
                }    
            );
            topScreen.Hide(true);
        }
    }

    public void Clear()
    {
        while(loadedMenus.Count > 0)
        {
            PanelMenu topScreen = loadedMenus.Pop();
            Destroy(topScreen.Panel.gameObject);
            Destroy(topScreen);
        }
    }
}