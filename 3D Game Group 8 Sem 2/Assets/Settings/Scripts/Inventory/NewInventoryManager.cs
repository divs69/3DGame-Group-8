using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NewInventoryManager : MonoBehaviour
{

    public static NewInventoryManager Instance;
    private HashSet<string> CollectedItems = new HashSet<string>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        if (!CollectedItems.Contains(itemName))
        {
            CollectedItems.Add(itemName);
            Debug.Log("Collected:" + itemName);
        }
    }

    public bool HasItem(string itemName)
    {
        return CollectedItems.Contains(itemName);
    }
}
