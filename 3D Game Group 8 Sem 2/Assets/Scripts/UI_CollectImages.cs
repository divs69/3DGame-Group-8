using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectImages : MonoBehaviour
{
    public InventoryManager inventorymanager;

    [Header("Collectables Images")]
    [SerializeField] private RawImage[] CollectableImage;
    [SerializeField] private string[] itemNames;

    void Update()
    {
        for (int i = 0; i < CollectableImage.Length; i++)
        {
            UpdateCollectableImage(CollectableImage[i], itemNames[i]);
        }
    }

    private void UpdateCollectableImage(RawImage CollectableImage, string itemName)
    {
        bool HasItem = inventorymanager.HasItem(itemName);
        Debug.Log($"UI CHECK: {itemName} = {HasItem}");
        CollectableImage.enabled = (HasItem);
    }
}
