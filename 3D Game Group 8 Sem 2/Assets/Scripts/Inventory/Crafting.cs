using UnityEngine;

public class Crafting : MonoBehaviour, IInteractable
{
    public void interact()
    {
        Inventory2.instance.ToggleInventory();
        Inventory2.instance.craftingMenu.SetActive(true);
    }

   
}
