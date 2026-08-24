using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SlotsCauldron : MonoBehaviour, IDropHandler
{


    [Header("Accepted Ingredients")]
    public string requireditemName;
    [Header("Images For Slots")]
    public Image slotimage;
    [HideInInspector]
    public bool isfilled = false;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null)
        { 
            return;
        }

        Interactimages draggeditem = droppedObject.GetComponent<Interactimages>();

        if (draggeditem == null)
        {
            return;
        }

        if (draggeditem.itemName == requireditemName)
        {
            FillSlot(draggeditem);
        }
        else 
        {
            Debug.Log("Wrong Ingredient");
        }

    }

    private void FillSlot(Interactimages draggeditem)
    {
        if (isfilled)
        {
            return;
            isfilled = true;
            slotimage.sprite = draggeditem.GetComponent<Image>().sprite;
            slotimage.enabled = true;
            draggeditem.gameObject.SetActive(false);
            CauldronManager.Instance.CheckRecipe();
        }
    }


}
