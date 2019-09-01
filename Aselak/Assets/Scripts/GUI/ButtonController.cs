using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour {
    public AudioClip clip;
    public GameObject menuPrefab;

	void Start () {
        // Add a click sound listener to the component button
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySingle(clip);
        });

        // Stack menu onclick if a menu prefab is linked to the button
        if (menuPrefab != null)
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                MenuManager._instance.ShowScreen(menuPrefab);
            });
        }
    }

    public void Back()
    {
        MenuManager._instance.BackScreen();
    }

    public void Close()
    {
        MenuManager._instance.Clear();
    }
}
