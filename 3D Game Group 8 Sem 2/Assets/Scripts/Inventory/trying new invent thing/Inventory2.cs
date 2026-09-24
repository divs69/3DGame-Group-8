using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Inventory2 : MonoBehaviour
{
    public IngredientData MushroomItem;
    public IngredientData CrystalItem;
    public GameObject hotbarObj;
    public GameObject inventorySlotParent;

    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<Slot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Additem(CrystalItem, 3);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Additem(MushroomItem, 3);
        }
    }

            public void Additem(IngredientData itemToAdd, int amount)
            {
                int remaining = amount;


                foreach (Slot slot in allSlots)
                {
                    if (slot.HasItem() && slot.GetItem() == itemToAdd)
                    {
                        int currentAmount = slot.GetAmount();
                        int maxStack = itemToAdd.maxStackSize;


                        if (currentAmount < maxStack)
                        {
                            int spaceLeft = maxStack - currentAmount;
                            int amountToAdd = Mathf.Min(spaceLeft, remaining);

                            slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                            remaining -= amountToAdd;

                            if (remaining <= 0)
                                return;
                        }
                    }
                }


                foreach (Slot slot in allSlots)
                {
                    if (!slot.HasItem())
                    {
                        int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                        slot.SetItem(itemToAdd, amountToPlace);
                        remaining -= amountToPlace;

                        if (remaining <= 0)
                            return;
                    }
                }

                if (remaining > 0)
                {
                    Debug.Log("inventory is full, could not add item" + remaining + "of" + itemToAdd.itemName);
                }
            }
}