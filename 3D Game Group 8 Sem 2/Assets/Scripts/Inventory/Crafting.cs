using UnityEngine;

public class Crafting : MonoBehaviour, IInteractable
{
    public void interact()
    {
        Inventory2.instance.ToggleInventory();
        
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Inventory2.instance.craftingMenu.SetActive(true);
            }
                

        }
      


    }

   
}
