using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lista de tipos de ingredientes posibles
public enum IngredientType
{
    Mantequilla,
    Leche,
    Huevo
}

public class Ingredient : MonoBehaviour
{
    // Tipo de ingrediente de este objeto
    public IngredientType ingredientType;
}
