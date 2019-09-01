using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {
	private LineRenderer[] _lRenderers = new LineRenderer[2];
    private float cutoff = 0;
    private bool isActive = false;
	// Use this for initialization
	void Awake () {
		_lRenderers[0] = GetComponent<LineRenderer> ();
		_lRenderers [1] = transform.GetChild(0).GetComponentInChildren<LineRenderer> ();
		Deactivate ();
	}

	public void Activate(Vector3[] points){
        points[0].y = points[1].y = -3;
        _lRenderers [0].SetPositions (points);
		Vector3 firstPoint = points [0];
		Vector3 lastPoint = points [points.Length-1];
		Vector3[] headPoints = new Vector3[2];
		headPoints [0] = lastPoint;
		headPoints [1] = lastPoint + ((lastPoint - firstPoint).normalized * 0.25f);
		_lRenderers [1].SetPositions (headPoints);
		foreach(LineRenderer lineRend in _lRenderers){
			lineRend.enabled = enabled;
		}
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(PulseArrow());
        }
    }

	public void Deactivate(){
        if (!isActive) { return; }
		foreach(LineRenderer lineRend in _lRenderers){
			lineRend.enabled = false;
		}
        isActive = false;
    }

    IEnumerator PulseArrow()
    {
        do
        {
            _lRenderers[0].material.SetFloat("_Cutoff", cutoff);
            _lRenderers[1].material.SetFloat("_Cutoff", cutoff);
            cutoff = Mathf.Lerp(cutoff, cutoff + 0.2f, 0.05f) % 4f;
            yield return new WaitForSeconds(0.05f);
        } while (isActive);
        cutoff = 0;
    }
}
