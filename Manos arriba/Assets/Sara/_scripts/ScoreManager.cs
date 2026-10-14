
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Managers")]
    public RecipeManager recipeManager;
    public PlayerGrab player1Grab;
    public PlayerGrab player2Grab;

    [Header("Guardar puntajes entre niveles")]
    public bool reiniciarAcumuladoAlIniciar = false;

    private const string CLAVE_PUNTAJE_P1 = "PuntajeAcumuladoP1";
    private const string CLAVE_PUNTAJE_P2 = "PuntajeAcumuladoP2";

    [Header("Configuración de puntos")]
    public int pointsPerIngredient = 20;
    public int pointsPerSteal = 10;
    public int maxSimilarityPoints = 50;

    [Header("Round Scores")]
    public int player1Score;
    public int player2Score;

    [Header("Total Scores")]
    public int player1TotalScore;
    public int player2TotalScore;

    [Header("Similarity")]
    [HideInInspector] public float player1Similarity;
    [HideInInspector] public float player2Similarity;

    [Header("Desglose de puntos")]
    public int player1IngredientPoints;
    public int player2IngredientPoints;

    public int player1StealPoints;
    public int player2StealPoints;

    public int player1SimilarityPoints;
    public int player2SimilarityPoints;

    // =====================================================
    // CARGAR PUNTAJES AL INICIAR CADA NIVEL
    // =====================================================

    private void Awake()
    {
        if (reiniciarAcumuladoAlIniciar)
        {
            PlayerPrefs.DeleteKey(CLAVE_PUNTAJE_P1);
            PlayerPrefs.DeleteKey(CLAVE_PUNTAJE_P2);
            PlayerPrefs.Save();

            reiniciarAcumuladoAlIniciar = false;
        }

        player1TotalScore =
            PlayerPrefs.GetInt(CLAVE_PUNTAJE_P1, 0);

        player2TotalScore =
            PlayerPrefs.GetInt(CLAVE_PUNTAJE_P2, 0);

        Debug.Log("PUNTAJE RECUPERADO AL INICIAR NIVEL");
        Debug.Log("P1 acumulado: " + player1TotalScore);
        Debug.Log("P2 acumulado: " + player2TotalScore);
    }

    // =====================================================
    // CALCULAR RESULTADOS DE LA RONDA
    // =====================================================

    public void CalculateRoundResults()
    {
        if (recipeManager == null ||
            player1Grab == null ||
            player2Grab == null)
        {
            Debug.LogError(
                "ScoreManager: falta asignar RecipeManager o PlayerGrab."
            );
            return;
        }

        List<IngredientType> player1Ingredients =
            GetPlayerIngredients(player1Grab);

        List<IngredientType> player2Ingredients =
            GetPlayerIngredients(player2Grab);

        player1Similarity = CompareRecipe(
            recipeManager.player1Recipe,
            player1Ingredients
        );

        player2Similarity = CompareRecipe(
            recipeManager.player2Recipe,
            player2Ingredients
        );

        int player1CorrectIngredients = CountCorrectIngredients(
            recipeManager.player1Recipe,
            player1Ingredients
        );

        int player2CorrectIngredients = CountCorrectIngredients(
            recipeManager.player2Recipe,
            player2Ingredients
        );

        // Puntos por ingredientes correctos
        player1IngredientPoints =
            player1CorrectIngredients * pointsPerIngredient;

        player2IngredientPoints =
            player2CorrectIngredients * pointsPerIngredient;

        // Puntos extra por robar ingredientes
        player1StealPoints =
            player1Grab.stolenIngredientsThisRound * pointsPerSteal;

        player2StealPoints =
            player2Grab.stolenIngredientsThisRound * pointsPerSteal;

        // Puntos adicionales por similitud
        player1SimilarityPoints = Mathf.RoundToInt(
            player1Similarity / 100f * maxSimilarityPoints
        );

        player2SimilarityPoints = Mathf.RoundToInt(
            player2Similarity / 100f * maxSimilarityPoints
        );

        // Puntaje de esta ronda
        player1Score =
            player1IngredientPoints +
            player1StealPoints +
            player1SimilarityPoints;

        player2Score =
            player2IngredientPoints +
            player2StealPoints +
            player2SimilarityPoints;

        // Sumar al acumulado recuperado
        player1TotalScore += player1Score;
        player2TotalScore += player2Score;

        // Guardar inmediatamente los acumulados
        GuardarPuntajesAcumulados();

        Debug.Log("======= RESULTADOS DE RONDA =======");
        Debug.Log("P1 similitud: " + player1Similarity + "%");
        Debug.Log("P1 ingredientes: " + player1IngredientPoints);
        Debug.Log("P1 robos: " + player1StealPoints);
        Debug.Log("P1 puntos similitud: " + player1SimilarityPoints);
        Debug.Log("P1 puntaje ronda: " + player1Score);
        Debug.Log("P1 TOTAL ACUMULADO: " + player1TotalScore);

        Debug.Log("-----------------------------------");

        Debug.Log("P2 similitud: " + player2Similarity + "%");
        Debug.Log("P2 ingredientes: " + player2IngredientPoints);
        Debug.Log("P2 robos: " + player2StealPoints);
        Debug.Log("P2 puntos similitud: " + player2SimilarityPoints);
        Debug.Log("P2 puntaje ronda: " + player2Score);
        Debug.Log("P2 TOTAL ACUMULADO: " + player2TotalScore);
        Debug.Log("===================================");
    }

    // =====================================================
    // GUARDAR ACUMULADOS
    // =====================================================

    private void GuardarPuntajesAcumulados()
    {
        PlayerPrefs.SetInt(CLAVE_PUNTAJE_P1, player1TotalScore);
        PlayerPrefs.SetInt(CLAVE_PUNTAJE_P2, player2TotalScore);
        PlayerPrefs.Save();

        Debug.Log("PUNTAJES GUARDADOS CORRECTAMENTE");
    }

    // =====================================================
    // OBTENER INGREDIENTES DEL JUGADOR
    // =====================================================

    private List<IngredientType> GetPlayerIngredients(
        PlayerGrab playerGrab)
    {
        List<IngredientType> ingredients =
            new List<IngredientType>(playerGrab.inventory);

        if (playerGrab.heldObject != null)
        {
            Ingredient ingredient =
                playerGrab.heldObject.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                ingredients.Add(ingredient.ingredientType);
            }
        }

        return ingredients;
    }

    // =====================================================
    // CONTAR INGREDIENTES CORRECTOS
    // =====================================================

    private int CountCorrectIngredients(
        List<IngredientType> recipe,
        List<IngredientType> ingredients)
    {
        int correctIngredients = 0;

        List<IngredientType> ingredientsCopy =
            new List<IngredientType>(ingredients);

        foreach (IngredientType ingredient in recipe)
        {
            if (ingredientsCopy.Contains(ingredient))
            {
                correctIngredients++;
                ingredientsCopy.Remove(ingredient);
            }
        }

        return correctIngredients;
    }

    // =====================================================
    // CALCULAR PORCENTAJE DE SIMILITUD
    // =====================================================

    private float CompareRecipe(
        List<IngredientType> recipe,
        List<IngredientType> ingredients)
    {
        if (recipe == null || recipe.Count == 0)
        {
            return 0f;
        }

        int correctIngredients =
            CountCorrectIngredients(recipe, ingredients);

        return (float)correctIngredients / recipe.Count * 100f;
    }
}
