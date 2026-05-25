using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lista de tipos de ingredientes posibles

   public enum IngredientType
{
    Mantequilla,
    Leche,
    Agua,
    Huevo,
    Cocoa,
    Sal,
    Esencia_de_vainilla,
    Polvo_para_hornear,
    Crema_Pastelera,
    Harina,
    Azucar
}


public class Ingredient : MonoBehaviour
{
    // Tipo de ingrediente de este objeto
    public IngredientType ingredientType;
    
}
