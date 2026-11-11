using System;
using UnityEngine;

public class CollectableIngredient : MonoBehaviour, ICollectable
{
    public static event Action<CollectableIngredient> OnAnyCollected;

    [Header("Item Data Link")]
    [SerializeField] private IngredientData data;

    public IngredientData Data => data;
    private bool collected;

    public void OnCollected()
    {
        if (collected || Inventory2.instance == null || !Inventory2.instance.Additem(data, 1))
            return;

        collected = true;
        OnAnyCollected?.Invoke(this);
        Destroy(gameObject);
    }
}