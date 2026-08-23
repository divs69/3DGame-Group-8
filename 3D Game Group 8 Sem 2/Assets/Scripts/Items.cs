using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Items : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isEquippable;

    public virtual void Use()
    {
        Debug.Log("Using or Equipping: " + itemName);
    }
}
