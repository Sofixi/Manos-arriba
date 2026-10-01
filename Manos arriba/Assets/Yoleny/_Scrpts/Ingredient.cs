
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
    // =====================================================
    // TIPO DE INGREDIENTE
    // =====================================================

    // Tipo de ingrediente de este objeto
    public IngredientType ingredientType;


    // =====================================================
    // ANIMACIÓN DEL INGREDIENTE
    // =====================================================

    // Altura máxima del movimiento
    public float floatHeight = 0.15f;

    // Velocidad de flotación
    public float floatSpeed = 2f;

    // Velocidad de rotación
    public float rotationSpeed = 50f;


    // Posición original
    private Vector3 startPosition;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        // Guardar posición inicial
        startPosition = transform.position;
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        // -----------------------------
        // FLOTAR
        // -----------------------------

        float newY =
            startPosition.y +
            Mathf.Sin(
                Time.time * floatSpeed
            ) * floatHeight;

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );


        // -----------------------------
        // ROTAR
        // -----------------------------

        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime
        );
    }
}
