using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RecipeIngredientData
{
    public string itemName;
    public int quantity;

}

public class InventoryManager : MonoBehaviour
{
    public HashSet<RecipeIngredientData> inventory = new HashSet<RecipeIngredientData>();


    public void AddItem(string itemName)

    {
        Debug.Log("Adding to inventory: " + itemName);
        RecipeIngredientData existingItem = inventory.FirstOrDefault(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity++;
        }
        else
        {

            RecipeIngredientData newItem = new RecipeIngredientData { itemName = itemName, quantity = 1 };
            inventory.Add(newItem);
        }
        Debug.Log("inventory now contains:");
        foreach (RecipeIngredientData item in inventory) 
        {
            Debug.Log(item.itemName + "X" + item.quantity);
        }
    }

    public bool HasItem(string itemName)
    {
        Debug.Log("has item checking for [" + itemName + "]");
        RecipeIngredientData existingItem = inventory.FirstOrDefault(item => item.itemName == itemName);
        bool result = existingItem != null && existingItem.quantity > 0;
        Debug.Log("has item result:" + result);
        return result;
    }


}
