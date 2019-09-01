using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompassController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

    public UIController controller;

    private Vector3 compassCenter;
    private Vector3 startDirection = Vector3.forward;
    private RectTransform _thisRt;
    private Image markPoint;

    private bool pointerDown;

    void Start()
    {
        _thisRt = GetComponent<RectTransform>();
        compassCenter = new Vector3(Screen.width/2, Screen.height/2);
        markPoint = GetComponent<Image>();
    }

    private void Update()
    {
        if (pointerDown && Input.touchCount < 2 && GameManager.instance.player != null)
        {
            Vector3 newDir = (Input.mousePosition - compassCenter).normalized;
            if ((Input.mousePosition - compassCenter).magnitude > markPoint.GetComponent<RectTransform>().rect.width / 2)
                MoveArrow(newDir * -1);
        }

        if(GameManager.instance.player != null)
        {
            switch (GameManager.instance.player.mode)
            {
                case ShipMode.Launch:
                case ShipMode.NoPower:
                case ShipMode.Dead:
                    {
                        markPoint.enabled = false;
                        break;
                    }
                default:
                    {
                        markPoint.enabled = true;
                        break;
                    }
            }
        }
    }

    public void MoveArrow(Vector3 newDir)
    {
        newDir.z = newDir.y;
        newDir.y = 0;

        // Activate directional arrow on ship
        Vector3[] points = new Vector3[2];
        BallScript player = GameManager.instance.player;
        points[0] = player.transform.position;
        points[1] = player.transform.position + (newDir);
        player.directionalArrow.Activate(points);

        // Send direction to player through ui controller
        controller.SetDirection(newDir);
    }

    public void OnPointerDown(PointerEventData eData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eData)
    {
        pointerDown = false;
        GameManager.instance.player.directionalArrow.Deactivate();
    }
}
