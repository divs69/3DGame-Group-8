using UnityEngine;

public interface IInteractable
{
    void interact();
}

public class interactor : MonoBehaviour
{
    public static bool LookingAtInteractable { get; private set; }

    public float interactRange = 5f;

    public GameObject interactUI;

    private IInteractable currentInteractable;

    void Start()
    {
        interactUI.SetActive(false);
    }

    void Update()
    {
        DetectInteractable();

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.interact();
        }
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // Look at everything along the ray, nearest first.
        RaycastHit[] hits = Physics.RaycastAll(ray, interactRange);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            // Skip the player's own body (and any duplicate player copy).
            if (hit.collider.GetComponentInParent<FPController>() != null) continue;

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                LookingAtInteractable = true;
                interactUI.SetActive(true);
                return;
            }

            break; // something solid is in the way, so stop looking
        }

        currentInteractable = null;
        LookingAtInteractable = false;
        interactUI.SetActive(false);
    }
}