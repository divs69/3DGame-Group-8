using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;


[System.Serializable]

public class Elements
{
    public IngredientData ingredient;
    public int amount;
}

[CreateAssetMenu(fileName = "Recipes", menuName = "Scriptable Objects/NewRecipes")]
public class Recipes : ScriptableObject
{
    public List<Elements> elements;
    public IngredientData result;
    public int resultAmount = 1;
}
