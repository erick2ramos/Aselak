using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class IntroCameraController : MonoBehaviour {
    public Camera mainCamera;
    public Material transitionMaterial;
    public GameObject tutorialPanel;

    [SerializeField]
    private float transitionPercent;

    private Queue<GameObject> waypoints = new Queue<GameObject>();

    void Start()
    {
        Camera _this = GetComponent<Camera>();
        GameObject goal = GameObject.FindGameObjectWithTag("Finish");
        if (goal != null)
        {
            float prevY = transform.position.y;
            Vector3 auxPos = goal.transform.position;
            auxPos.y = prevY;
            transform.position = auxPos;
        }

        //foreach (GameObject go in GameObject.FindGameObjectsWithTag("Collectable"))
        //{
        //    waypoints.Enqueue(go);
        //}
        waypoints.Enqueue(mainCamera.gameObject);
    }
    

    //[ExecuteInEditMode]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
        // Applies the first material updating the cutoff value for intro "animation"
        transitionMaterial.SetFloat("_Cutoff", transitionPercent);
        Graphics.Blit(source, destination, transitionMaterial);
        
        RenderTexture.ReleaseTemporary(rt);
    }

    public void OnIntroDone()
    {
        StartCoroutine(MoveTo());
    }

    public void FinishIntro()
    {
        mainCamera.enabled = true;
        transform.position = mainCamera.transform.position;
        GetComponent<Camera>().enabled = false;
        gameObject.SetActive(false);
        if (SettingsManager.instance.gameSettings.showTutorial && !GameManager.instance.tutorialShown)
        {
            GameManager.instance.tutorialShown = true;
            MenuManager._instance.ShowScreen(tutorialPanel);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FinishIntro();
        }
    }

    IEnumerator MoveTo()
    {
        float myY = transform.position.y;
        while (waypoints.Count > 0)
        {
            Vector3 pos = waypoints.Dequeue().transform.position;
            pos.y = myY;
            float delta = 0.02f;
            do
            {
                transform.position = Vector3.Lerp(transform.position, pos, delta);
                delta = Mathf.Lerp(delta, 0.1f, 0.03f);
                yield return new WaitForEndOfFrame();
            } while ((transform.position - pos).magnitude > 0.25);
        }
        FinishIntro();
    }
}
