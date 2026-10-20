using System;
using UnityEngine;
using UnityEngine.Events;

// TEMPORARY stand-in for the real grimoire pickup, so the tutorial can be tested.
// Remove it once nash's stuff works
public class TutorialInteractable : MonoBehaviour, IInteractable
{
    public static event Action<TutorialInteractable> OnUsed;

    public bool hideAfterUse = true;
    public GameObject hideTarget;
    public UnityEvent onUse;

    public void interact()
    {
        Debug.Log("[Tutorial] Interacted with: " + name);
        onUse.Invoke();
        if (OnUsed != null) OnUsed(this);
        if (hideAfterUse) (hideTarget != null ? hideTarget : gameObject).SetActive(false);
    }
}