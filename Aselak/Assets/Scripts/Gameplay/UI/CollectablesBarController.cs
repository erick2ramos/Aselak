using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesBarController : MonoBehaviour {
    public GameObject starUIPrefab;
    private float offsetX = 20f;

    private StarUIController[] stars = new StarUIController[3];
    private int _currentInactive = 0;

    // Instanciate three star ui prefabs in an array;
    void Awake()
    {
        for(int i = 0; i < stars.Length; i++)
        {
            GameObject go = Instantiate(starUIPrefab);
            go.transform.SetParent(transform, false);
            stars[i] = go.GetComponent<StarUIController>();
        }
    }

    // Picks the firt non active star in the array and grants it
    public void Collect()
    {
        if(_currentInactive < stars.Length)
        {
            stars[_currentInactive].Grant();
            _currentInactive++;
        }
    }
}
