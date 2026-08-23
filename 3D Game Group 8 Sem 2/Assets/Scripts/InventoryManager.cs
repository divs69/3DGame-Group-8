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
    }

    public bool HasItem(string itemName)
    {
        RecipeIngredientData existingItem = inventory.FirstOrDefault(item => item.itemName == itemName);
        return existingItem != null && existingItem.quantity > 0;
    }


}
