using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectImages : MonoBehaviour
{
    public InventoryManager inventorymanager;

    [Header("Collectables Images")]
    [SerializeField] private RawImage[] CollectableImages;
    [SerializeField] private string[] itemNames;

    void Update()
    {
        for (int i = 0; i < CollectableImages.Length; i++)
        {
            UpdateInventoryImage(CollectableImages[i], itemNames[i]);
        }
    }

    private void UpdateInventoryImage(RawImage InventoryImage, string itemName)
    {
        bool hasItem = inventorymanager.HasItem(itemName);
        InventoryImage.enabled = hasItem;
    }
}
