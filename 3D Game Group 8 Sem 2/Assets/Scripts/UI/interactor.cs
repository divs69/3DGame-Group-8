using UnityEngine;

public interface IInteractable
{
    void interact();
}

public class interactor : MonoBehaviour
{
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
        Ray ray = new Ray (Camera.main.transform.position, Camera .main.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactUI.SetActive(true);
                return;
            }

        }

        currentInteractable = null;
        interactUI.SetActive(false);
    }
}
