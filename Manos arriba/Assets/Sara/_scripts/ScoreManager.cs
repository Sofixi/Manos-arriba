using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Script de evaluación, comparación suma de puntos. 
    // Referencia al RecipeManager
    public RecipeManager recipeManager;

    // Referencia jugador 1
    public PlayerGrab player1Grab;

    // Referencia jugador 2
    public PlayerGrab player2Grab;

    // Puntaje total jugador 1
    public int player1Score;

    // Puntaje total jugador 2
    public int player2Score;

    // Similitud jugador 1
    [HideInInspector]
    public float player1Similarity;

    // Similitud jugador 2
    [HideInInspector]
    public float player2Similarity;

    // Método que calcula resultados de ronda
    public void CalculateRoundResults()
    {
        // Obtiene ingredientes jugador 1
        List<IngredientType> player1Ingredients =
        GetPlayerIngredients(player1Grab);

        // Obtiene ingredientes jugador 2
        List<IngredientType> player2Ingredients =
        GetPlayerIngredients(player2Grab);

        // Calcula similitud jugador 1
        player1Similarity =
        CompareRecipe(recipeManager.player1Recipe,
        player1Ingredients);

        // Calcula similitud jugador 2
        player2Similarity =
        CompareRecipe(recipeManager.player2Recipe,
        player2Ingredients);

        // Convierte porcentaje en puntos
        player1Score += Mathf.RoundToInt(player1Similarity);

        player2Score += Mathf.RoundToInt(player2Similarity);

        // Mostrar resultados
        Debug.Log("=== RESULTADOS ===");

        Debug.Log("Player 1 similitud: "
        + player1Similarity + "%");

        Debug.Log("Player 2 similitud: "
        + player2Similarity + "%");

        Debug.Log("Player 1 total: "
        + player1Score);

        Debug.Log("Player 2 total: "
        + player2Score);
    }

    // Método que obtiene ingredientes del jugador
    List<IngredientType> GetPlayerIngredients
    (PlayerGrab playerGrab)
    {
        // Copia inventario
        List<IngredientType> ingredients =
        new List<IngredientType>(playerGrab.inventory);

        // Revisar si tiene ingrediente en mano
        if (playerGrab.heldObject != null)
        {
            // Obtener Ingredient
            Ingredient ingredient =
            playerGrab.heldObject.GetComponent<Ingredient>();

            // Agregar ingrediente actual
            ingredients.Add(ingredient.ingredientType);
        }

        return ingredients;
    }

    // Método que compara receta con ingredientes
    float CompareRecipe(List<IngredientType> recipe,
    List<IngredientType> ingredients)
    {
        // Cantidad correcta
        int correctIngredients = 0;

        // Copia temporal
        List<IngredientType> ingredientsCopy =
        new List<IngredientType>(ingredients);

        // Recorre receta
        foreach (IngredientType ingredient in recipe)
        {
            // Si encuentra ingrediente correcto
            if (ingredientsCopy.Contains(ingredient))
            {
                // Suma coincidencia
                correctIngredients++;

                // Elimina ingrediente usado
                ingredientsCopy.Remove(ingredient);
            }
        }

        // Calcula porcentaje
        float similarity =
        ((float)correctIngredients /
        recipe.Count) * 100f;

        return similarity;
    }
}
