using UnityEngine;

public class CollectableIngredient : MonoBehaviour, ICollectable
{
    [Header("Item Data Link")]
    [SerializeField] private IngredientData data;

    public InventoryManager inventorymanager;

    public IngredientData Data => data;

    public void OnCollected()
    {
        Debug.Log($"Collected: {data.itemName} via Blep!");
        inventorymanager.AddItem(gameObject.name);

        Destroy(gameObject);

    }
}
