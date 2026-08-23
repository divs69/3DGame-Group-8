using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Items> items = new List<Items>();
    public Transform slotContainer; // Drag InventoryPanel here
    public GameObject slotPrefab;   // Drag your InventorySlot prefab here

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(Items item)
    {
        items.Add(item);
        UpdateUI();
    }

    public void RemoveItem(Items item)
    {
        items.Remove(item);
        UpdateUI();
    }

    void UpdateUI()
    {
        // Clear old visual slots
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        // Generate updated visual slots
        foreach (Items item in items)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotUI = newSlot.GetComponent<InventorySlotUI>();
            slotUI.DisplayItem(item);
        }
    }
}