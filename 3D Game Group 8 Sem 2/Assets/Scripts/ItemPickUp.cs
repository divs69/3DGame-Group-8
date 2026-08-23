using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    // Drag your ScriptableObject Item data asset here in the Inspector
    public Items itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding with the item is the Player
        if (other.CompareTag("Player"))
        {
            // Send the item data to our top-right inventory system
            InventoryManager.Instance.AddItem(itemData);

            // Destroy the physical item on the ground so it disappears
            Destroy(gameObject);
        }
    }
}
