using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    [Header("Jugador")]
    public PlayerGrab playerGrab;

    [Header("Receta")]
    public RecipeManager recipeManager;

    [Tooltip("ACTIVADO para Player 1. DESACTIVADO para Player 2.")]
    public bool isPlayer1 = true;

    [Header("Base de datos")]
    public IngredientSpriteDatabase database;

    [Header("Imagenes de ingredientes")]
    public Image[] ingredientSlots;

    [Header("Checks")]
    public GameObject[] checkImages;

    private List<IngredientType> currentRecipe;

    private bool recipeLoaded = false;

    private Coroutine recipeCoroutine;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        TryLoadRecipe();
    }


    // =====================================================
    // ON ENABLE
    // =====================================================

    void OnEnable()
    {
        // Esto es importante porque tu UIManager
        // desactiva y vuelve a activar el HUD.
        TryLoadRecipe();
    }


    // =====================================================
    // INTENTAR CARGAR RECETA
    // =====================================================

    void TryLoadRecipe()
    {
        if (recipeLoaded)
        {
            return;
        }

        if (recipeCoroutine != null)
        {
            StopCoroutine(recipeCoroutine);
        }

        recipeCoroutine = StartCoroutine(WaitForRecipe());
    }


    // =====================================================
    // ESPERAR A QUE RECIPEMANAGER TENGA LA RECETA
    // =====================================================

    IEnumerator WaitForRecipe()
    {
        Debug.Log(
            gameObject.name +
            " esperando receta..."
        );

        // Esperar a que exista RecipeManager
        while (recipeManager == null)
        {
            yield return null;
        }


        // Esperar hasta que RecipeManager haya
        // cargado las recetas
        while (
            recipeManager.player1Recipe.Count == 0 &&
            recipeManager.player2Recipe.Count == 0
        )
        {
            yield return null;
        }


        LoadRecipe();

        recipeCoroutine = null;
    }


    // =====================================================
    // CARGAR RECETA
    // =====================================================

    void LoadRecipe()
    {
        if (recipeManager == null)
        {
            Debug.LogError(
                gameObject.name +
                "  RecipeManager NO está asignado."
            );

            return;
        }


        if (database == null)
        {
            Debug.LogError(
                gameObject.name +
                "  IngredientSpriteDatabase NO está asignado."
            );

            return;
        }


        if (isPlayer1)
        {
            currentRecipe =
                recipeManager.player1Recipe;

            Debug.Log(
                "================================="
            );

            Debug.Log(
                gameObject.name +
                " Cargando RECETA PLAYER 1"
            );
        }
        else
        {
            currentRecipe =
                recipeManager.player2Recipe;

            Debug.Log(
                "================================="
            );

            Debug.Log(
                gameObject.name +
                " Cargando RECETA PLAYER 2"
            );
        }


        if (currentRecipe == null ||
            currentRecipe.Count == 0)
        {
            Debug.LogError(
                gameObject.name +
                "  La receta está vacía."
            );

            return;
        }


        // =================================================
        // LIMPIAR IMAGENES
        // =================================================

        for (
            int i = 0;
            i < ingredientSlots.Length;
            i++
        )
        {
            if (ingredientSlots[i] == null)
            {
                continue;
            }

            ingredientSlots[i].sprite = null;

            Color color =
                ingredientSlots[i].color;

            color.a = 0f;

            ingredientSlots[i].color = color;
        }


        // =================================================
        // APAGAR CHECKS
        // =================================================

        for (
            int i = 0;
            i < checkImages.Length;
            i++
        )
        {
            if (checkImages[i] != null)
            {
                checkImages[i].SetActive(false);
            }
        }


        // =================================================
        // MOSTRAR INGREDIENTES DE LA RECETA
        // =================================================

        for (
            int i = 0;
            i < currentRecipe.Count;
            i++
        )
        {
            IngredientType ingredient =
                currentRecipe[i];


            Debug.Log(
                gameObject.name +
                " Slot " +
                i +
                " necesita: " +
                ingredient
            );


            // ---------------------------------------------
            // ¿EXISTE EL SLOT?
            // ---------------------------------------------

            if (i >= ingredientSlots.Length)
            {
                Debug.LogError(
                    gameObject.name +
                    "  NO hay suficientes slots."
                );

                continue;
            }


            if (ingredientSlots[i] == null)
            {
                Debug.LogError(
                    gameObject.name +
                    "  Ingredient Slot " +
                    i +
                    " está vacío."
                );

                continue;
            }


            // ---------------------------------------------
            // BUSCAR SPRITE
            // ---------------------------------------------

            Sprite sprite =
                database.GetSprite(ingredient);


            if (sprite == null)
            {
                Debug.LogError(
                    gameObject.name +
                    "  NO se encontró Sprite para " +
                    ingredient
                );

                continue;
            }


            Debug.Log(
                gameObject.name +
                "  Sprite encontrado para " +
                ingredient +
                ": " +
                sprite.name
            );


            // ---------------------------------------------
            // COLOCAR SPRITE
            // ---------------------------------------------

            ingredientSlots[i].sprite =
                sprite;


            Color slotColor =
                ingredientSlots[i].color;

            slotColor.a = 1f;

            ingredientSlots[i].color =
                slotColor;


            ingredientSlots[i].preserveAspect =
                true;
        }


        recipeLoaded = true;


        Debug.Log(
            gameObject.name +
            " RECETA CARGADA CORRECTAMENTE."
        );
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        if (!recipeLoaded)
        {
            return;
        }

        UpdateChecks();
    }


    // =====================================================
    // ACTUALIZAR CHECKS
    // =====================================================

    void UpdateChecks()
    {
        if (currentRecipe == null)
        {
            return;
        }


        for (
            int i = 0;
            i < currentRecipe.Count;
            i++
        )
        {
            if (i >= checkImages.Length)
            {
                continue;
            }


            if (checkImages[i] == null)
            {
                continue;
            }


            IngredientType requiredIngredient =
                currentRecipe[i];


            bool obtained =
                HasIngredient(
                    requiredIngredient
                );


            checkImages[i].SetActive(
                obtained
            );
        }
    }


    // =====================================================
    // COMPROBAR SI TIENE INGREDIENTE
    // =====================================================

    bool HasIngredient(
        IngredientType requiredIngredient
    )
    {
        if (playerGrab == null)
        {
            return false;
        }


        // ---------------------------------------------
        // INVENTARIO
        // ---------------------------------------------

        if (playerGrab.inventory != null)
        {
            if (
                playerGrab.inventory.Contains(
                    requiredIngredient
                )
            )
            {
                return true;
            }
        }


        // ---------------------------------------------
        // INGREDIENTE EN LA MANO
        // ---------------------------------------------

        if (playerGrab.heldObject != null)
        {
            Ingredient ingredient =
                playerGrab.heldObject
                .GetComponent<Ingredient>();


            if (ingredient != null)
            {
                if (
                    ingredient.ingredientType ==
                    requiredIngredient
                )
                {
                    return true;
                }
            }
        }


        return false;
    }
}