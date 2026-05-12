using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    // Lista donde se guardan los ingredientes ya recolectados
    public List<IngredientType> inventory =
    new List<IngredientType>();

    // Objeto que el jugador tiene actualmente en la mano
    public GameObject heldObject;

    // Punto donde aparecerá el ingrediente en la mano
    public Transform holdPoint;

    // Distancia máxima para recoger ingredientes
    public float grabDistance = 2f;

    // Tecla para recoger
    public KeyCode grabKey = KeyCode.E;

    // Update se ejecuta cada frame
    void Update()
    {
        // Si el jugador presiona la tecla de recoger
        if (Input.GetKeyDown(grabKey))
        {
            // Intenta recoger ingrediente
            TryPickIngredient();
        }
    }
    
    // Método que intenta recoger un ingrediente
    void TryPickIngredient()
    {
        // Busca todos los colliders cerca del jugador
        Collider[] hits =
        Physics.OverlapSphere(transform.position, grabDistance);

        // Recorre todos los objetos encontrados
        foreach (Collider hit in hits)
        {
            // Revisa si el objeto tiene el tag "Ingredient"
            if (hit.CompareTag("Ingredient"))
            {
                if (hit.gameObject == heldObject)
                {
                    continue;
                }
                // Guarda el objeto encontrado
                GameObject newIngredient = hit.gameObject;

                // Llama al método para recogerlo
                PickIngredient(newIngredient);

                // Sale del foreach para no recoger varios a la vez
                break;
            }
        }
    }

    // Método que recoge el ingrediente
    void PickIngredient(GameObject newIngredient)
    {
        // Si ya tenía un ingrediente en la mano
        if (heldObject != null)
        {
            // Obtiene el script Ingredient del objeto actual
            Ingredient currentIngredient =
            heldObject.GetComponent<Ingredient>();

            // Guarda el tipo de ingrediente en el inventario
            inventory.Add(currentIngredient.ingredientType);

            // Destruye el objeto visual anterior
            Destroy(heldObject);
        }

        // El nuevo ingrediente pasa a la mano
        heldObject = newIngredient;

        // Obtiene el rigidbody del ingrediente
        Rigidbody rb =
        heldObject.GetComponent<Rigidbody>();

        // Desactiva físicas para evitar bugs
        rb.isKinematic = true;

        // Hace hijo el ingrediente del holdPoint
        heldObject.transform.SetParent(holdPoint);

        // Coloca el ingrediente exactamente en la mano
        heldObject.transform.localPosition = Vector3.zero;

        // Resetea la rotación local
        heldObject.transform.localRotation = Quaternion.identity;
    }
}