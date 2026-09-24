using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool hovering; //determine if your hovering over a draggable item when your going to drag it 

    private IngredientData helditem;
    private int itemAmount;

    private Image iconImage;

    private TextMeshProUGUI amountTxt;

    private void Awake()
    {
        iconImage = transform.GetChild(0).GetComponent<Image>();
        amountTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public IngredientData GetItem()
    {
        return helditem;
    }

    public int GetAmount()
    {
        return itemAmount;
    }

    public void SetItem(IngredientData item, int amount = 1)
    {
        helditem = item;
        itemAmount = amount;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (helditem != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = helditem.itemIcon;
            amountTxt.text = itemAmount.ToString();
        }
        else 
        {
            iconImage.enabled = false;
            amountTxt.text = "";
        }

    }

    public int AddAmount(int amountToAdd)
    {
        itemAmount += amountToAdd;
        UpdateSlot();
        return itemAmount;
    }

    public int RemoveAmount(int amountToRemove) 
    {
        itemAmount -= amountToRemove;
        if(itemAmount <= 0)
        {
            ClearSlot();
        }
        else
        {
            UpdateSlot();
        }

        return itemAmount;
    }

    public void ClearSlot()
    {
        helditem = null;
        itemAmount = 0;
        UpdateSlot();
    }

    public bool HasItem()
    {
        return helditem != null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
