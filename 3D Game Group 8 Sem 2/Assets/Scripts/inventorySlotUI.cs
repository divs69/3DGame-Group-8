using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconDisplay;
    private Items currentItem;

    public void DisplayItem(Items newItem)
    {
        currentItem = newItem;
        if (currentItem != null)
        {
            iconDisplay.sprite = currentItem.icon;
            iconDisplay.enabled = true;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconDisplay.sprite = null;
        iconDisplay.enabled = false;
    }

    // Hooked up to the Button component's OnClick event
    public void OnSlotClicked()
    {
        if (currentItem != null)
        {
            currentItem.Use(); // Trigger equipment logic
            InventoryManager.Instance.RemoveItem(currentItem); // Remove after equipping
        }
    }
}