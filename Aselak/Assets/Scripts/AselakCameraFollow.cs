using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AselakCameraFollow : MonoBehaviour {
	public float zSmooth = 8f; // How smoothly the camera catches up with it's target movement in the z axis.
	public float zMargin = 1f; // Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the z axis.
	public float xMargin = 1f; // Distance in the y axis the player can move before the camera follows.
    public float shakeTime;
    public float minZoom = 6f;
    public float maxZoom = 12f;

    public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.
    public bool preventMovement;


    private Vector3 pivotOrigin;
    private Camera _this;
    private bool validMoveStart;
    private Canvas canvas;
    private UIController uiController;

    //Controls for handling double click on editor mode
    bool firstClick = false;
    public bool isDoubleClick = false;
    bool timerRunning = false;
    float doubleClickTimer;

    // Controls for handling peek movement
    Vector3 peekPosition;
    bool goPeek;

    public Transform Player
    {
        get { return m_Player; }
        set { m_Player = value; }
    }

    private Transform m_Player;
    private Vector3 averagePosition;

    void Awake(){
        // Setting up the reference.
        _this = GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        uiController = GameObject.Find("Canvas/PlayerUI").GetComponent<UIController>();
	}

    private void Start()
    {
        int length = 0;
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Collectable"))
        {
            averagePosition += go.transform.position;
            length++;
        }
        averagePosition /= length;
    }

	// Update is called once per frame
	void Update () {

        if (!GameManager.instance.paused && m_Player != null && GetComponent<Camera>().enabled)
        {
            switch (GameManager.instance.player.mode)
            {
                case ShipMode.NoPower:
                case ShipMode.Launch:
                    {
                        TrackPlayer();
                        break;
                    }
                default:
                    {
                        MoveCamera();
                        HandlePinchZoom();
                        break;
                    }
            }

            if (goPeek)
            {
                transform.position = Vector3.Lerp(transform.position, peekPosition, 0.8f * Time.deltaTime);
                goPeek = (transform.position - peekPosition).magnitude >= 0.2f;

            }
        }


        if (shakeTime > 0)
        {
            goPeek = false;
            transform.position += new Vector3(Mathf.Log(shakeTime + 1f) * Mathf.Cos(shakeTime * 90f), 0, 0);
            shakeTime -= Time.deltaTime;
        }
        else
            shakeTime = 0;
    }

    public void MoveCamera()
    {
        if (preventMovement)
            return;
        // Double click move for editor
#if UNITY_EDITOR
        if (Input.touchCount < 2)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                pivotOrigin = Input.mousePosition;
                if (!uiController.IsPointOnGUI(pivotOrigin))
                {
                    validMoveStart = true;
                }
                else
                {
                    validMoveStart = false;
                }
                if (!firstClick)
                {
                    firstClick = true;
                    doubleClickTimer = Time.time;
                } else
                {
                    firstClick = false;
                    isDoubleClick = true;
                }
            }
            else if (Input.GetButtonUp("Fire1") && isDoubleClick)
            {
                isDoubleClick = false;
            } else if (Input.GetButton("Fire1") && validMoveStart && isDoubleClick)
            {
                goPeek = false;
                Vector3 pos = _this.ScreenToViewportPoint(pivotOrigin - Input.mousePosition);
                Vector3 move = new Vector3(pos.x, 0, pos.y);
                transform.Translate(move, Space.World);
            }
            if (firstClick && Time.time > doubleClickTimer + 0.25f)
            {
                firstClick = false;
            }
        }
#endif

        // Double touch drag to move camera
        //if(Input.touchCount == 2)
        //{
        //    // Capture each touch
        //    Touch tZero = Input.GetTouch(0);
        //    Touch tOne = Input.GetTouch(1);

        //    if(tZero.phase == TouchPhase.Moved && tOne.phase == TouchPhase.Moved)
        //    {
        //        // Calculate delta for previous and current position
        //        float currentSep = Vector2.Distance(tZero.position, tOne.position);
        //        float prevSep = Vector2.Distance(tZero.position - tZero.deltaPosition, tOne.position - tOne.deltaPosition);

        //        // if the delta distance between previous and current position
        //        // is low its recognized as a camera pan
        //        if (Mathf.Abs(currentSep - prevSep) < 10f)
        //        {
        //            // Pan camera based on the average of both touches movement delta distance
        //            Vector2 deltaMove = (tZero.deltaPosition + tOne.deltaPosition) / 2;
        //            Vector3 move = new Vector3(-deltaMove.x, 0, -deltaMove.y);
        //            transform.Translate(move * 5f * Time.deltaTime, Space.World);
        //        }
        //    }
        //}

        // Move with 1 touch
        if (Input.touchCount == 1)
        {
            Touch tZero = Input.GetTouch(0);
            
            if(tZero.phase == TouchPhase.Moved)
            {
                goPeek = false;
                Vector3 pos = tZero.deltaPosition;
                Vector3 move = new Vector3(-pos.x, 0, -pos.y);
                transform.Translate(move * 5f * Time.deltaTime, Space.World);
            }
        }


        // Clamps the camera to the playable area
        // TODO: maybe better as an individual function?
        float camHeight = _this.orthographicSize * 2.0f;
        float camWidth = camHeight * _this.pixelWidth / _this.pixelHeight;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -40f + (camWidth / 2), 40f - (camWidth / 2)),
            10f,
            Mathf.Clamp(transform.position.z, -40f + (camHeight / 2), 40f - (camHeight / 2))
        );
    }

	private bool CheckXMargin (){
		// Returns true if the distance between the camera and the player in the z axis is greater than the y margin.
		return Mathf.Abs(transform.position.x - m_Player.position.x) > xMargin;
	}

	private bool CheckZMargin (){
		// Returns true if the distance between the camera and the player in the z axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - m_Player.position.z) > zMargin;
	}

	private void TrackPlayer(){
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetZ = transform.position.z;

		// If the player has moved beyond the x margin...
		if (CheckXMargin())
		{
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth*Time.deltaTime);
		}

		// If the player has moved beyond the y margin...
		if (CheckZMargin())
		{
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetZ = Mathf.Lerp(transform.position.z, m_Player.position.z, zSmooth*Time.deltaTime);
		}

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetZ = Mathf.Clamp(targetZ, minXAndY.y, maxXAndY.y);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, transform.position.y, targetZ);
	}

    void HandlePinchZoom()
    {
#if UNITY_EDITOR
        // Mouse scroll zoom
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _this.orthographicSize = Mathf.Lerp(_this.orthographicSize, Mathf.Max(_this.orthographicSize - 0.5f, minZoom), 0.2f);
        } else if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _this.orthographicSize = Mathf.Lerp(_this.orthographicSize, Mathf.Min(_this.orthographicSize + 0.5f, maxZoom), 0.2f);
        }
#endif
        //Pinch zoom logic
        if(Input.touchCount == 2)
        {
            Touch tZero = Input.GetTouch(0);
            Touch tOne = Input.GetTouch(1);

            if (tZero.phase == TouchPhase.Moved && tOne.phase == TouchPhase.Moved)
            {
                // Calculate delta distances from previous and current
                float prevSep = Vector2.Distance(tZero.position - tZero.deltaPosition, tOne.position - tOne.deltaPosition);
                float currSep = Vector2.Distance(tZero.position, tOne.position);

                // Final delta between previous and current position
                float deltaMagnitude = prevSep - currSep;

                // Decide the direction of the pinch
                float sign = 0;
                if (deltaMagnitude > 0.1f)
                    sign = 1;
                else if (deltaMagnitude < -0.1f)
                    sign = -1;

                // See if the final delta is big so its recognized as a pinch
                if (Mathf.Abs(deltaMagnitude) >= 10f)
                {
                    // Lerp to new zoom value
                    _this.orthographicSize = Mathf.Clamp(
                        Mathf.Lerp(_this.orthographicSize,
                            _this.orthographicSize + (1f * sign),
                            0.4f),
                        minZoom,
                        maxZoom);
                }
            }
        }

        // Clamps the camera to the playable area
        // TODO: maybe better as an individual function?
        float camHeight = _this.orthographicSize * 2.0f;
        float camWidth = camHeight * _this.pixelWidth / _this.pixelHeight;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -40f + (camWidth / 2), 40f - (camWidth / 2)),
            10f,
            Mathf.Clamp(transform.position.z, -40f + (camHeight / 2), 40f - (camHeight / 2))
        );
    }

    public void Peek(Vector3 center, Vector3 direction, float distance)
    {
        float length = 0;
        averagePosition = Vector3.zero;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Collectable"))
        {
            float weight = 1 / (m_Player.transform.position - go.transform.position).sqrMagnitude;
            averagePosition += (weight) * go.transform.position;
            length += weight;
        }
        center.y = transform.position.y;
        averagePosition += (center + (direction * distance));
        length++;
        peekPosition = averagePosition / length;
        goPeek = true;
    }
}