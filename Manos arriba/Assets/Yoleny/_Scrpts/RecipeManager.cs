using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
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
        LoadRecipesByRound();
    }

    void LoadRecipesByRound()
    {
        // Limpia recetas anteriores
        player1Recipe.Clear();
        player2Recipe.Clear();

        switch (currentRound)
        {
            // =========================
            // RONDA 1
            // =========================
            case 1:

                // PLAYER 1
                player1Recipe.Add(IngredientType.Harina);
                player1Recipe.Add(IngredientType.Azucar);
                player1Recipe.Add(IngredientType.Huevo);

                // PLAYER 2
                player2Recipe.Add(IngredientType.Leche);
                player2Recipe.Add(IngredientType.Mantequilla);
                player2Recipe.Add(IngredientType.Sal);

                break;

            // =========================
            // RONDA 2
            // =========================
            case 2:

                // PLAYER 1
                player1Recipe.Add(IngredientType.Cocoa);
                player1Recipe.Add(IngredientType.Leche);
                player1Recipe.Add(IngredientType.Polvo_para_hornear);
                player1Recipe.Add(IngredientType.Azucar);

                // PLAYER 2
                player2Recipe.Add(IngredientType.Harina);
                player2Recipe.Add(IngredientType.Huevo);
                player2Recipe.Add(IngredientType.Esencia_de_vainilla);
                player2Recipe.Add(IngredientType.Agua);

                break;

            // =========================
            // RONDA 3
            // =========================
            case 3:

                // PLAYER 1
                player1Recipe.Add(IngredientType.Harina);
                player1Recipe.Add(IngredientType.Crema_Pastelera);
                player1Recipe.Add(IngredientType.Leche);
                player1Recipe.Add(IngredientType.Esencia_de_vainilla);
                player1Recipe.Add(IngredientType.Azucar);

                // PLAYER 2
                player2Recipe.Add(IngredientType.Cocoa);
                player2Recipe.Add(IngredientType.Huevo);
                player2Recipe.Add(IngredientType.Mantequilla);
                player2Recipe.Add(IngredientType.Polvo_para_hornear);
                player2Recipe.Add(IngredientType.Sal);

                break;
        }

        // Mostrar recetas
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