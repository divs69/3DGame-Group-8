using UnityEngine;
using UnityEngine.UI;


public class NewUIInventory : MonoBehaviour
{
    [Header("Inventory Manager")]
    public NewInventoryManager inventorymanager;
    [Header("Inventory Images")]
    public GameObject MushroomTwoImage;

    private void Start()
    {
        MushroomTwoImage.gameObject.SetActive(false);

        UpdateInventoryUI();
    }

    private void Update()
    {
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        if (inventorymanager == null) 
        {
            Debug.LogError("Inventory manager is not assigned");
            return;

        }

        MushroomTwoImage.gameObject.SetActive(inventorymanager.HasItem("MushroomTwo"));
    }


}
