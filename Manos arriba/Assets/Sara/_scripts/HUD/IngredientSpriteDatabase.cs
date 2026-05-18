using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientSprite
{
    public IngredientType ingredientType;

    public Sprite sprite;
}

public class IngredientSpriteDatabase : MonoBehaviour
{
    // Lista editable en Unity
    public List<IngredientSprite> ingredientSprites =
    new List<IngredientSprite>();

    // Diccionario interno
    private Dictionary<IngredientType, Sprite> spriteDictionary =
    new Dictionary<IngredientType, Sprite>();

    void Awake()
    {
        // Guardar datos en diccionario
        foreach (IngredientSprite item in ingredientSprites)
        {
            if (!spriteDictionary.ContainsKey(item.ingredientType))
            {
                spriteDictionary.Add(
                    item.ingredientType,
                    item.sprite
                );
            }
        }
    }

    // Devuelve sprite según ingrediente
    public Sprite GetSprite(IngredientType type)
    {
        if (spriteDictionary.ContainsKey(type))
        {
            return spriteDictionary[type];
        }

        return null;
    }
}