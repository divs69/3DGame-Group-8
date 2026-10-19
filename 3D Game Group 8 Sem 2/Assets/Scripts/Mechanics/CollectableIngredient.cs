using System;
using UnityEngine;

public class CollectableIngredient : MonoBehaviour, ICollectable
{
    public static event Action<CollectableIngredient> OnAnyCollected;

    [Header("Item Data Link")]
    [SerializeField] private IngredientData data;

    public IngredientData Data => data;

    public void OnCollected()
    {
        Debug.Log($"Collected: {data.itemName} via Blep!");

        if (Inventory2.instance != null)
            Inventory2.instance.Additem(data, 1);
        else
            Debug.LogWarning("Inventory2.instance is null - did you add 'instance = this;' to Inventory2's Awake?");

        if (OnAnyCollected != null) OnAnyCollected(this);
        Destroy(gameObject);
    }
}