using System;
using UnityEngine;
using UnityEngine.Events;

public class TutorialInteractable : MonoBehaviour, IInteractable
{
    public static event Action<TutorialInteractable> OnUsed;
    public UnityEvent onUse;

    public void interact()
    {
        onUse.Invoke();
        if (OnUsed != null) OnUsed(this);
    }
}