using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    [Header("Jugador")]
    public PlayerGrab playerGrab;

    [Header("Base de datos")]
    public IngredientSpriteDatabase database;

    [Header("Slots UI")]
    public Image[] slots;

    void Update()
    {
        UpdateHUD();
    }

    void UpdateHUD()
    {
        // Limpiar slots
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].sprite = null;

            Color c = slots[i].color;
            c.a = 0f;
            slots[i].color = c;
        }

        // Mostrar ingredientes guardados
        for (int i = 0;
             i < playerGrab.inventory.Count &&
             i < slots.Length;
             i++)
        {
            IngredientType ingredient =
            playerGrab.inventory[i];

            Sprite sprite =
            database.GetSprite(ingredient);

            if (sprite != null)
            {
                slots[i].sprite = sprite;

                Color c = slots[i].color;
                c.a = 1f;
                slots[i].color = c;
            }
        }

        // Mostrar ingrediente actual en mano
        if (playerGrab.heldObject != null)
        {
            Ingredient ingredient =
            playerGrab.heldObject.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                int lastIndex =
                playerGrab.inventory.Count;

                if (lastIndex < slots.Length)
                {
                    Sprite sprite =
                    database.GetSprite(
                        ingredient.ingredientType
                    );

                    slots[lastIndex].sprite = sprite;

                    Color c = slots[lastIndex].color;
                    c.a = 1f;
                    slots[lastIndex].color = c;
                }
            }
        }
    }
}