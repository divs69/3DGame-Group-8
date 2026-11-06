using UnityEngine;
using UnityEngine.InputSystem;

public class grimoiretest : MonoBehaviour
{
    [SerializeField] private string playerTag = "WorkingPlayer";

    private bool playerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        Debug.Log("the grimoire was picked up"); 
        Destroy(gameObject);
    }
}
