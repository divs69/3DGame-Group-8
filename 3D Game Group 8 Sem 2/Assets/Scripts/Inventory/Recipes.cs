using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;


[System.Serializable]

public class Element
{
    public IngredientData ingredient;
    public int amount;
}

[CreateAssetMenu(fileName = "Recipes", menuName = "Scriptable Objects/NewRecipes")]
public class Recipes : ScriptableObject
{
    public List<Element> elements;
    public IngredientData result;
    public int resultAmount = 1;
}
