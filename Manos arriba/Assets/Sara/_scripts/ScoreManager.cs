using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // =========================================================
    // SCORE MANAGER
    // Se encarga de calcular el puntaje de cada ronda
    // y acumularlo durante las 3 rondas.
    // =========================================================

    public static ScoreManager Instance;

    [Header("Managers")]

    // Referencia al RecipeManager de la ronda actual
    public RecipeManager recipeManager;

    // Referencia jugador 1
    public PlayerGrab player1Grab;

    // Referencia jugador 2
    public PlayerGrab player2Grab;


    [Header("Round Scores")]

    // Puntaje obtenido en ESTA ronda por jugador 1
    public int player1Score;

    // Puntaje obtenido en ESTA ronda por jugador 2
    public int player2Score;


    [Header("Total Scores")]

    // Puntaje acumulado de todas las rondas
    public int player1TotalScore;

    public int player2TotalScore;


    [Header("Round")]

    // Número de ronda actual
    public int currentRound = 1;

    // Cantidad total de rondas
    public int totalRounds = 3;


    [Header("Similarity")]

    // Similitud de esta ronda
    [HideInInspector]
    public float player1Similarity;

    [HideInInspector]
    public float player2Similarity;


    private void Awake()
    {
        // Evita tener más de un ScoreManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Mantener este objeto entre escenas
        DontDestroyOnLoad(gameObject);
    }


    // =========================================================
    // CALCULAR RESULTADOS DE LA RONDA
    // =========================================================

    public void CalculateRoundResults()
    {
        // Verificar referencias
        if (recipeManager == null)
        {
            Debug.LogError("ScoreManager: No hay RecipeManager asignado.");
            return;
        }

        if (player1Grab == null)
        {
            Debug.LogError("ScoreManager: No hay Player1Grab asignado.");
            return;
        }

        if (player2Grab == null)
        {
            Debug.LogError("ScoreManager: No hay Player2Grab asignado.");
            return;
        }


        // -----------------------------------------------------
        // OBTENER INGREDIENTES
        // -----------------------------------------------------

        List<IngredientType> player1Ingredients =
            GetPlayerIngredients(player1Grab);

        List<IngredientType> player2Ingredients =
            GetPlayerIngredients(player2Grab);


        // -----------------------------------------------------
        // CALCULAR SIMILITUD
        // -----------------------------------------------------

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


        // -----------------------------------------------------
        // PUNTAJE DE ESTA RONDA
        // -----------------------------------------------------

        player1Score =
            Mathf.RoundToInt(player1Similarity);

        player2Score =
            Mathf.RoundToInt(player2Similarity);


        // -----------------------------------------------------
        // SUMAR AL ACUMULADO
        // -----------------------------------------------------

        player1TotalScore += player1Score;

        player2TotalScore += player2Score;


        // -----------------------------------------------------
        // DEBUG
        // -----------------------------------------------------

        Debug.Log("================================");
        Debug.Log("        RESULTADOS RONDA " + currentRound);
        Debug.Log("================================");

        Debug.Log(
            "Player 1 similitud: "
            + player1Similarity
            + "%"
        );

        Debug.Log(
            "Player 2 similitud: "
            + player2Similarity
            + "%"
        );

        Debug.Log(
            "Player 1 ronda: "
            + player1Score
        );

        Debug.Log(
            "Player 2 ronda: "
            + player2Score
        );

        Debug.Log(
            "Player 1 TOTAL: "
            + player1TotalScore
        );

        Debug.Log(
            "Player 2 TOTAL: "
            + player2TotalScore
        );

        Debug.Log("================================");
    }


    // =========================================================
    // OBTENER INGREDIENTES DEL JUGADOR
    // =========================================================

    List<IngredientType> GetPlayerIngredients(
        PlayerGrab playerGrab
    )
    {
        // Crear copia del inventario
        List<IngredientType> ingredients =
            new List<IngredientType>(
                playerGrab.inventory
            );


        // -----------------------------------------------------
        // REVISAR SI TIENE UN INGREDIENTE EN LA MANO
        // -----------------------------------------------------

        if (playerGrab.heldObject != null)
        {
            Ingredient ingredient =
                playerGrab.heldObject.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                ingredients.Add(
                    ingredient.ingredientType
                );
            }
        }


        return ingredients;
    }


    // =========================================================
    // COMPARAR RECETA
    // =========================================================

    float CompareRecipe(
        List<IngredientType> recipe,
        List<IngredientType> ingredients
    )
    {
        // Evitar división entre 0
        if (recipe == null || recipe.Count == 0)
        {
            return 0;
        }


        // Cantidad de ingredientes correctos
        int correctIngredients = 0;


        // Copia temporal
        List<IngredientType> ingredientsCopy =
            new List<IngredientType>(ingredients);


        // Recorrer receta
        foreach (IngredientType ingredient in recipe)
        {
            // Si encuentra el ingrediente
            if (ingredientsCopy.Contains(ingredient))
            {
                correctIngredients++;

                // Eliminarlo para evitar contar
                // el mismo ingrediente dos veces
                ingredientsCopy.Remove(ingredient);
            }
        }


        // Calcular porcentaje
        float similarity =
            ((float)correctIngredients /
            recipe.Count) * 100f;


        return similarity;
    }


    // =========================================================
    // AVANZAR DE RONDA
    // =========================================================

    public void AdvanceRound()
    {
        currentRound++;

        Debug.Log(
            "Avanzando a la ronda "
            + currentRound
        );
    }


    // =========================================================
    // REINICIAR PARTIDA COMPLETA
    // =========================================================

    public void ResetGame()
    {
        player1Score = 0;
        player2Score = 0;

        player1TotalScore = 0;
        player2TotalScore = 0;

        player1Similarity = 0;
        player2Similarity = 0;

        currentRound = 1;

        Debug.Log("Puntajes reiniciados.");
    }
}