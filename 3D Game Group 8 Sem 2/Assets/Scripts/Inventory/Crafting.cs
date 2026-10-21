using UnityEngine;
using UnityEngine.UIElements;

public class Crafting : MonoBehaviour
{
    //public void interact()
    //{
    //    Inventory2.instance.ToggleInventory();
    //    Inventory2.instance.craftingMenu.SetActive(true);
    //}

    public GameObject craftingMenu; 

    private void OnTriggerEnter(Collider other)
    {
        // Check if the overlapping object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            craftingMenu.SetActive(true);
            Debug.Log("The crafting panel opened");

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            craftingMenu.SetActive(false);

        }
    }


}
