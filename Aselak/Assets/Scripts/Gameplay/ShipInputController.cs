using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInputController : MonoBehaviour{
    public BallScript ship;
    public PreviousLaunch launch;

    private Vector3 direction;
    private float burst;

    private Camera mainCamera;
    private AselakCameraFollow mainCameraController;
    private bool dragging;
    private float radius;

    float minZoom;
    float maxZoom;

    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCameraController = mainCamera.GetComponent<AselakCameraFollow>();
        radius = GetComponent<SphereCollider>().radius;

        minZoom = mainCameraController.minZoom;
        maxZoom = mainCameraController.maxZoom;
    }

    private void Update()
    {
        // Raycast check for dragging 
        if (dragging)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                dragging = false;
                FeedBack(dragging);
                mainCameraController.preventMovement = false;
            } else if (Input.GetButton("Fire1"))
            {
                mainCameraController.preventMovement = true;
                OnMouseDrag();
            }
        } else if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray hitRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(hitRay, out hit)) {
                dragging = hit.collider == GetComponent<Collider>();
                FeedBack(dragging);
            }
        }

        if(Camera.main != null && Camera.main.enabled)
        {
            float zoomPercent = ((mainCamera.orthographicSize - minZoom) / (maxZoom - minZoom));
            GetComponent<SphereCollider>().radius = radius + 
                (1.25f * zoomPercent);
        }
    }

    void OnMouseDrag()
    {
        Vector3 dragWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        dragWorldPos.y = 0;
        Vector3 newDir = -(dragWorldPos - transform.position);
        if (newDir.magnitude > GetComponent<SphereCollider>().radius)
        {
            mainCameraController.Peek(transform.position, newDir.normalized, 2);
            // LERP ALL THE THINGS
            Vector3 deltaDir = Vector3.Lerp(ship.Direction, newDir, Vector3.Angle(newDir, ship.Direction) / 180);
            ship.Direction = deltaDir;
            ship.Burst = Mathf.Clamp(Mathf.Lerp(
                ship.Burst, 
                (newDir.magnitude - GetComponent<SphereCollider>().radius), Mathf.Abs(ship.Burst - 
                    (newDir.magnitude - GetComponent<SphereCollider>().radius)) / 2), 0, 2);
        }
    }

    void FeedBack(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
            StartCoroutine(FadeTo(child, active ? 1 : 0, 0.3f));
        }
    }

    IEnumerator FadeTo(Transform transform, float aValue, float aTime)
    {
        float alpha = transform.GetComponent<SpriteRenderer>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
    }

}
