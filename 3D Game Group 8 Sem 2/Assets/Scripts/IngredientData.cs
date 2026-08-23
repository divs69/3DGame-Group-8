using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Potion Shop/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    [TextArea(2, 5)] public string description;
    public int maxStackSize = 50;
}
