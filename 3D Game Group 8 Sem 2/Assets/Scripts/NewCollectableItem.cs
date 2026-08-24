using UnityEngine;

public class NewCollectableItem : MonoBehaviour
{
    [Header("Item Information")]
    public string itemName;
    private bool playernearby = false;

    private void Update()
    {
        if (playernearby && Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playernearby = true;


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playernearby = false;
        }
    }



    private void CollectItem()
    {
        NewInventoryManager.Instance.AddItem(itemName);

        Destroy(gameObject);
    }

}
