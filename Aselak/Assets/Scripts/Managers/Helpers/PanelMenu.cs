using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelMenu : MonoBehaviour
{
    GameObject menu;
    public RectTransform Panel
    {
        get { return menu.GetComponent<RectTransform>(); }
    }
    Animator anim;
    public bool isPopup;
    public UnityEvent OnMenuShown;
    public UnityEvent OnMenuHidden;

    public void SetUp(RectTransform parent, GameObject scr)
    {
        menu = scr;
        RectTransform rt = menu.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        anim = menu.GetComponent<Animator>();
        OnMenuShown = new UnityEvent();
        OnMenuHidden = new UnityEvent();
    }

    // Shows the ui menu on screen playing an animation if has one
    public void Show()
    {
        if (anim != null)
        {
            anim.SetBool("IsOpen", true);
            StartCoroutine(WaitFor("Open"));
        }
        else if (OnMenuShown != null)
        {
            OnMenuShown.Invoke();
            OnMenuShown.RemoveAllListeners();
        }
    }

    // Hides the ui menu on screen playing an animation if has one 
    public void Hide(bool selfDestroy = false)
    {
        if (selfDestroy)
            OnMenuHidden.AddListener(Delete);
        if (anim != null)
        {
            anim.SetBool("IsOpen", false);
            StartCoroutine(WaitFor("Closed"));
        }
        else if (OnMenuHidden != null)
        {
            OnMenuHidden.Invoke();
            OnMenuHidden.RemoveAllListeners();
        }
    }

    void Delete()
    {
        Destroy(menu);
    }

    private IEnumerator WaitFor(string state)
    {
        do
        {
            yield return null;
        } while (!anim.GetCurrentAnimatorStateInfo(0).IsName(state));
        if (OnMenuShown != null)
        {
            OnMenuShown.Invoke();
            OnMenuShown.RemoveAllListeners();
        }
        if (OnMenuHidden != null)
        {
            OnMenuHidden.Invoke();
            OnMenuHidden.RemoveAllListeners();
        }
    }
}
