using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // =====================================================
    // SCRIPT DE EVALUACIÓN Y PUNTAJE
    // =====================================================

    [Header("Managers")]

    // Referencia al RecipeManager
    public RecipeManager recipeManager;

    // Referencia jugador 1
    public PlayerGrab player1Grab;

    // Referencia jugador 2
    public PlayerGrab player2Grab;


    // =====================================================
    // CONFIGURACIÓN DE PUNTOS
    // =====================================================

    [Header("Configuración de puntos")]

    // Puntos por cada ingrediente correcto
    public int pointsPerIngredient = 20;

    // Puntos extra por robar un ingrediente
    public int pointsPerSteal = 10;

    // Máximo de puntos que puede dar la similitud
    public int maxSimilarityPoints = 50;


    // =====================================================
    // PUNTAJE DE LA RONDA
    // =====================================================

    [Header("Round Scores")]

    // Puntaje de la ronda jugador 1
    public int player1Score;

    // Puntaje de la ronda jugador 2
    public int player2Score;


    // =====================================================
    // PUNTAJE TOTAL
    // =====================================================

    [Header("Total Scores")]

    // Puntaje acumulado jugador 1
    public int player1TotalScore;

    // Puntaje acumulado jugador 2
    public int player2TotalScore;


    // =====================================================
    // SIMILITUD
    // =====================================================

    [Header("Similarity")]

    // Porcentaje de similitud jugador 1
    [HideInInspector]
    public float player1Similarity;

    // Porcentaje de similitud jugador 2
    [HideInInspector]
    public float player2Similarity;


    // =====================================================
    // RESULTADOS DETALLADOS
    // =====================================================

    [Header("Desglose de puntos")]

    // Puntos por ingredientes correctos
    public int player1IngredientPoints;
    public int player2IngredientPoints;

    // Puntos por ingredientes robados
    public int player1StealPoints;
    public int player2StealPoints;

    // Puntos obtenidos por similitud
    public int player1SimilarityPoints;
    public int player2SimilarityPoints;


    // =====================================================
    // CALCULAR RESULTADOS
    // =====================================================

    public void CalculateRoundResults()
    {
        // -------------------------------------------------
        // OBTENER INGREDIENTES
        // -------------------------------------------------

        List<IngredientType> player1Ingredients =
            GetPlayerIngredients(player1Grab);

        List<IngredientType> player2Ingredients =
            GetPlayerIngredients(player2Grab);


        // -------------------------------------------------
        // CALCULAR SIMILITUD
        // -------------------------------------------------

        player1Similarity =
            CompareRecipe(
                recipeManager.player1Recipe,
                player1Ingredients
            );

        player2Similarity =
            CompareRecipe(
                recipeManager.player2Recipe,
                player2Ingredients
            );


        // -------------------------------------------------
        // CONTAR INGREDIENTES CORRECTOS
        // -------------------------------------------------

        int player1CorrectIngredients =
            CountCorrectIngredients(
                recipeManager.player1Recipe,
                player1Ingredients
            );

        int player2CorrectIngredients =
            CountCorrectIngredients(
                recipeManager.player2Recipe,
                player2Ingredients
            );


        // -------------------------------------------------
        // PUNTOS POR INGREDIENTES
        // -------------------------------------------------

        player1IngredientPoints =
            player1CorrectIngredients *
            pointsPerIngredient;

        player2IngredientPoints =
            player2CorrectIngredients *
            pointsPerIngredient;


        // -------------------------------------------------
        // PUNTOS POR ROBAR
        // -------------------------------------------------

        player1StealPoints =
            player1Grab.stolenIngredientsThisRound *
            pointsPerSteal;

        player2StealPoints =
            player2Grab.stolenIngredientsThisRound *
            pointsPerSteal;


        // -------------------------------------------------
        // PUNTOS POR SIMILITUD
        // -------------------------------------------------

        player1SimilarityPoints =
            Mathf.RoundToInt(
                (player1Similarity / 100f) *
                maxSimilarityPoints
            );

        player2SimilarityPoints =
            Mathf.RoundToInt(
                (player2Similarity / 100f) *
                maxSimilarityPoints
            );


        // -------------------------------------------------
        // PUNTAJE TOTAL DE LA RONDA
        // -------------------------------------------------

        player1Score =
            player1IngredientPoints +
            player1StealPoints +
            player1SimilarityPoints;

        player2Score =
            player2IngredientPoints +
            player2StealPoints +
            player2SimilarityPoints;


        // -------------------------------------------------
        // SUMAR AL TOTAL ACUMULADO
        // -------------------------------------------------

        player1TotalScore += player1Score;
        player2TotalScore += player2Score;


        // =================================================
        // DEBUG
        // =================================================

        Debug.Log("=================================");
        Debug.Log("        RESULTADOS DE RONDA");
        Debug.Log("=================================");

        Debug.Log(
            "Player 1 similitud: "
            + player1Similarity
            + "%"
        );

        Debug.Log(
            "Player 1 ingredientes: "
            + player1CorrectIngredients
            + " x "
            + pointsPerIngredient
            + " = "
            + player1IngredientPoints
        );

        Debug.Log(
            "Player 1 robos: "
            + player1Grab.stolenIngredientsThisRound
            + " x "
            + pointsPerSteal
            + " = "
            + player1StealPoints
        );

        Debug.Log(
            "Player 1 puntos similitud: "
            + player1SimilarityPoints
        );

        Debug.Log(
            "Player 1 PUNTAJE RONDA: "
            + player1Score
        );

        Debug.Log(
            "Player 1 TOTAL: "
            + player1TotalScore
        );


        Debug.Log("---------------------------------");


        Debug.Log(
            "Player 2 similitud: "
            + player2Similarity
            + "%"
        );

        Debug.Log(
            "Player 2 ingredientes: "
            + player2CorrectIngredients
            + " x "
            + pointsPerIngredient
            + " = "
            + player2IngredientPoints
        );

        Debug.Log(
            "Player 2 robos: "
            + player2Grab.stolenIngredientsThisRound
            + " x "
            + pointsPerSteal
            + " = "
            + player2StealPoints
        );

        Debug.Log(
            "Player 2 puntos similitud: "
            + player2SimilarityPoints
        );

        Debug.Log(
            "Player 2 PUNTAJE RONDA: "
            + player2Score
        );

        Debug.Log(
            "Player 2 TOTAL: "
            + player2TotalScore
        );

        Debug.Log("=================================");
    }


    // =====================================================
    // OBTENER INGREDIENTES DEL JUGADOR
    // =====================================================

    List<IngredientType> GetPlayerIngredients(
        PlayerGrab playerGrab
    )
    {
        List<IngredientType> ingredients =
            new List<IngredientType>(
                playerGrab.inventory
            );


        // Revisar si tiene un ingrediente en la mano

        if (playerGrab.heldObject != null)
        {
            Ingredient ingredient =
                playerGrab.heldObject
                .GetComponent<Ingredient>();

            if (ingredient != null)
            {
                ingredients.Add(
                    ingredient.ingredientType
                );
            }
        }

        return ingredients;
    }


    // =====================================================
    // CONTAR INGREDIENTES CORRECTOS
    // =====================================================

    int CountCorrectIngredients(
        List<IngredientType> recipe,
        List<IngredientType> ingredients
    )
    {
        int correctIngredients = 0;

        List<IngredientType> ingredientsCopy =
            new List<IngredientType>(
                ingredients
            );


        foreach (
            IngredientType ingredient
            in recipe
        )
        {
            if (
                ingredientsCopy.Contains(
                    ingredient
                )
            )
            {
                correctIngredients++;

                ingredientsCopy.Remove(
                    ingredient
                );
            }
        }

        return correctIngredients;
    }


    // =====================================================
    // COMPARAR RECETA
    // =====================================================

    float CompareRecipe(
        List<IngredientType> recipe,
        List<IngredientType> ingredients
    )
    {
        int correctIngredients =
            CountCorrectIngredients(
                recipe,
                ingredients
            );


        // Evitar división entre cero

        if (recipe.Count == 0)
        {
            return 0;
        }


        // Calcular porcentaje

        float similarity =
            (
                (float)correctIngredients /
                recipe.Count
            ) * 100f;


        return similarity;
    }
}

