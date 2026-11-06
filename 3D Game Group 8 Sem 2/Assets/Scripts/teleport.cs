using UnityEngine;

public class teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        
        if (destination == null)
        {
            Debug.LogWarning("teleport has no destination assigned", this);
            return;
        }
        
        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null) controller.enabled = false;

        other.transform.SetPositionAndRotation(destination.position, destination.rotation);

        if (controller != null) controller.enabled = true;
    }


}
