using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public enum ShipMode
{
    Exploration,
    Directional,
    Burst,
    NoPower,
    Dead,
    Launch
}

public class BallScript : MonoBehaviour {
    public float maxForce = 1000f;
	public float minForce = 20f;
	public float score = 0f;
    public bool died = false;
    public int starsCollected = 0;
    public Font unipix;
    public AudioClip explosionFX;
    public AudioSource shipSourceFX;
    public GameObject explosionParticlesFx;

    // Tester probe
    public GameObject probePrefab;
    public bool test;

    public ShipMode mode;

    public ParticleSystem system
    {
        get {
            if(_cachedSystem == null)
            {
                _cachedSystem = GetComponentInChildren<ParticleSystem>();
            }
            return _cachedSystem;
        }
    }

    public ArrowScript directionalArrow
    {
        get { return GetComponentInChildren<ArrowScript>(); }
    }

    public bool CanStart
    {
        get { return !kickstart; }
    }

	public float maxImpulse = 2f;
	private float minImpulse = 0.25f;

	public Vector3 pInput;
	public float pBurst = 1.55f;
    private bool kickstart = false;
	private LineRenderer lineRenderer;
    private Rigidbody _rb;
    private ParticleSystem _cachedSystem;
    private GameObject _proxyMouseMark;
    private float fuel;
    
    private GameObject _deltaVTxt;


    private RectTransform canvas;
    private GameObject uiCanvas;

    // Variables for making impulse from everywhere in the screen;
    [SerializeField]
    Vector3 mouseStart;

    bool validLaunch = false;

    public void Score(GameObject go){
        starsCollected++;
		score += go.GetComponent<CollectableScript> ().points;
	}

    void Start()
    {
        system.Stop(true);
        _rb = GetComponent<Rigidbody>();
        GetComponentInChildren<ArrowScript>().Deactivate();

        mode = ShipMode.Exploration;

        if (GameManager.instance.previousLaunch != null)
        {
            pInput = GameManager.instance.previousLaunch.direction;
            pBurst = GameManager.instance.previousLaunch.burst;
        } else
        {
            pInput = transform.forward;
            pBurst = 0;
        }
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        uiCanvas = GameObject.Find("Canvas/PlayerUI");
        _deltaVTxt = new GameObject("ShipDeltaV");
        _deltaVTxt.transform.SetParent(uiCanvas.transform.Find("TopBar").transform, false);

        Text txt = GenerateCustomText(_deltaVTxt, 30);
        ((RectTransform)_deltaVTxt.transform).anchoredPosition = new Vector2(220f, -20f);

        SetMode(mode);
    }

    Text GenerateCustomText(GameObject parent, int fontSize)
    {
        Text txt = parent.AddComponent<Text>();
        txt.color = new Color(0f, 0.48f, 1f);
        txt.fontSize = fontSize;
        txt.font = unipix;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        txt.verticalOverflow = VerticalWrapMode.Overflow;
        txt.alignment = TextAnchor.MiddleCenter;
        return txt;
    }

    public void SetMode(ShipMode newMode)
    {
        mode = newMode;
    }

    public void ToggleMode()
    {
        mode = (ShipMode)(((int)mode + 1) % Enum.GetNames(typeof(ShipMode)).Length);
    }

    public float Burst
    {
        get { return pBurst; }
        set { pBurst = value; }
    }

    public Vector3 Direction
    {
        get { return pInput.normalized; }
        set { pInput = value; }
    }

    void Update () {
        // Maintain the models orientation to the velocity's direction 
        Vector3 velDir = _rb.velocity.normalized;
        if (velDir.magnitude > 0)
        {
            float speed = 3f * Time.deltaTime;
            Quaternion deltaRot = new Quaternion();
            deltaRot.SetLookRotation(velDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, deltaRot, speed);
        } else
        {
            float speed = 3f * Time.deltaTime;
            Quaternion deltaRot = new Quaternion();
            deltaRot.SetLookRotation(pInput, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, deltaRot, speed);
        }


        if(!GameManager.instance.paused && Camera.main != null && Camera.main.enabled)
        {
            if(!kickstart)
                SendProbe();
        }
    }

    void WriteDeltaV()
    {
        _deltaVTxt.GetComponent<Text>().text = "delta-V: " + (_rb.velocity.magnitude).ToString("0.000") + " m/s";
    }

    void CancelInput()
    {
        validLaunch = false;
        GetComponentInChildren<ArrowScript>().Deactivate();
        if (_proxyMouseMark != null)
        {
            Destroy(_proxyMouseMark);
        }
    }

    public void Launch()
    {
        // Change ship mode to nopower
        SetMode(ShipMode.Launch);
        _rb.velocity = pInput * pBurst;

        GameManager.instance.StoreLaunch(pInput, pBurst);
        StartEngine();
        kickstart = true;
        // Deactivate mode UI selector
        uiCanvas.transform.Find("BottomBar/LaunchButton").GetComponent<Button>().interactable = false;
        if(simulator != null)
        {
            Destroy(simulator);
        }
        CancelInput();
    }

    // Allows the ship to make another launch
    public void Refuel()
    {
        _rb.velocity = Vector3.zero;
        SetMode(ShipMode.Exploration);
        kickstart = false;
    }

    // Testing without lossing so much time
    float probeCooldown = 0;
    GameObject simulator;
    void SendProbe()
    {
        if(simulator == null)
        {
            simulator = Instantiate(probePrefab, transform.position, transform.rotation);
            simulator.GetComponent<TrajectorySimulator>().player = this;
        }
    }

    // Win programaticaly animated
    public void Win(GameObject other)
    {
        // Programatically animate the ship to enter the goal vortex
        Vector3 deltaPos = - transform.position + other.transform.position;
        Vector3 mainVel = _rb.velocity;
        _rb.velocity = (mainVel + deltaPos).normalized * mainVel.magnitude;
        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.04f);

        // Set ship to look at center of goal
        Quaternion deltaRot = new Quaternion();
        Vector3 lookAtPoint = other.transform.position + new Vector3(0, -4, 0);
        deltaRot.SetLookRotation(lookAtPoint - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, deltaRot, 0.1f);

        // Scale down ship
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.01f);
    }

    public void Die()
    {
        died = true;
        SoundManager.instance.PlaySingle(explosionFX);
        Instantiate(explosionParticlesFx, transform.position, Quaternion.Euler(new Vector3(90,0,0)));
        Camera.main.GetComponent<AselakCameraFollow>().shakeTime = 0.8f;
        GameManager.instance.GameOver((int)score, died);
        Destroy(gameObject);
    }

    // Turns on the particle system and waits until the fuel is out
    void StartEngine()
    {
        float seconds = 3f;
        system.Play(true);
        shipSourceFX.Play();
        // Stops the particle system after the fuel has run out
        StartCoroutine(WaitFor(seconds, () =>
        {
            SetMode(ShipMode.NoPower);
            shipSourceFX.Stop();
            system.Stop(true);
        }));
    }

    IEnumerator WaitFor(float secs, UnityAction callback)
    {
        yield return new WaitForSeconds(secs);
        callback();
    }

}
