using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    // Inventario
    public List<IngredientType> inventory =
    new List<IngredientType>();

    // Objeto en mano
    public GameObject heldObject;

    // Punto donde se sostiene
    public Transform holdPoint;

    // Distancia de agarre
    public float grabDistance = 2f;

    // Teclas
    public KeyCode grabKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    void Update()
    {
        // Agarrar / robar
        if (Input.GetKeyDown(grabKey))
        {
            TryPickIngredient();
        }

        // Soltar
        if (Input.GetKeyDown(dropKey))
        {
            DropIngredient();
        }
    }

    void TryPickIngredient()
    {
        Collider[] hits =
        Physics.OverlapSphere(transform.position,
        grabDistance);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Ingredient"))
            {
                GameObject newIngredient =
                hit.gameObject;

                // Evita agarrar el mismo
                if (newIngredient == heldObject)
                {
                    continue;
                }

                PickIngredient(newIngredient);

                break;
            }
        }
    }

    void PickIngredient(GameObject newIngredient)
    {
        // Si otro jugador lo tiene, se lo roba
        PlayerGrab[] players =
        FindObjectsOfType<PlayerGrab>();

        foreach (PlayerGrab player in players)
        {
            if (player != this &&
                player.heldObject == newIngredient)
            {
                player.ForceDrop();
            }
        }

        // Si yo ya tengo algo, lo suelto
        if (heldObject != null)
        {
            DropIngredient();
        }

        // Tomar ingrediente
        heldObject = newIngredient;

        Rigidbody rb =
        heldObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Pegar al holdPoint
        heldObject.transform.SetParent(holdPoint);

        heldObject.transform.localPosition =
        Vector3.zero;

        heldObject.transform.localRotation =
        Quaternion.identity;
        heldObject.transform.localScale = Vector3.one;

        Debug.Log(gameObject.name +
        " agarró ingrediente");
    }

    void DropIngredient()
    {
        if (heldObject == null)
            return;

        GameObject ingredientToDrop =
        heldObject;

        heldObject = null;

        // Quitar padre
        ingredientToDrop.transform.SetParent(null);

        // Posición enfrente del jugador
        ingredientToDrop.transform.position =
            holdPoint.position +
            transform.forward;

        Rigidbody rb =
        ingredientToDrop.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log(gameObject.name +
        " soltó ingrediente");
    }

    // Soltar silenciosamente cuando te roban
    void ForceDrop()
    {
        if (heldObject == null)
            return;

        GameObject ingredientToDrop =
        heldObject;

        heldObject = null;

        ingredientToDrop.transform.SetParent(null);

        Rigidbody rb =
        ingredientToDrop.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // Dibuja distancia en escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
        transform.position,
        grabDistance);
    }
}