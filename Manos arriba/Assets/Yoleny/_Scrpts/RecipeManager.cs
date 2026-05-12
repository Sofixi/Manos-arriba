using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    // Lista de ingredientes posibles
    public List<IngredientType> allIngredients =
    new List<IngredientType>();

    // Receta jugador 1
    public List<IngredientType> player1Recipe =
    new List<IngredientType>();

    // Receta jugador 2
    public List<IngredientType> player2Recipe =
    new List<IngredientType>();

    // Ronda actual
    public int currentRound = 1;

    void Start()
    {
        GenerateRecipes();
    }

    void GenerateRecipes()
    {
        // Limpia recetas anteriores
        player1Recipe.Clear();
        player2Recipe.Clear();

        // Cantidad depende de la ronda
        int ingredientCount = currentRound + 1;

        // Genera receta jugador 1
        for (int i = 0; i < ingredientCount; i++)
        {
            IngredientType randomIngredient =
            allIngredients[Random.Range(0, allIngredients.Count)];

            player1Recipe.Add(randomIngredient);
        }

        // Genera receta jugador 2
        for (int i = 0; i < ingredientCount; i++)
        {
            IngredientType randomIngredient =
            allIngredients[Random.Range(0, allIngredients.Count)];

            player2Recipe.Add(randomIngredient);
        }

        // Mostrar recetas en consola
        Debug.Log("=== RECETA PLAYER 1 ===");

        foreach (IngredientType ingredient in player1Recipe)
        {
            Debug.Log(ingredient);
        }

        Debug.Log("=== RECETA PLAYER 2 ===");

        foreach (IngredientType ingredient in player2Recipe)
        {
            Debug.Log(ingredient);
        }
    }
}