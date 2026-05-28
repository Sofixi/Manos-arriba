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

    // Distancia para agarrar
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
        Physics.OverlapSphere(
        transform.position,
        grabDistance);

        foreach (Collider hit in hits)
        {
            // Revisar tag
            if (hit.CompareTag("Ingredient"))
            {
                GameObject newIngredient =
                hit.gameObject;

                // Evitar agarrar el mismo
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
        // Buscar jugadores
        PlayerGrab[] players =
        FindObjectsOfType<PlayerGrab>();

        // Robar ingrediente si otro lo tiene
        foreach (PlayerGrab player in players)
        {
            if (player != this &&
                player.heldObject == newIngredient)
            {
                player.ForceDrop();
            }
        }

        // Si ya tengo uno, guardarlo en inventario
        if (heldObject != null)
        {
            Ingredient currentIngredient =
            heldObject.GetComponent<Ingredient>();

            if (currentIngredient != null)
            {
                inventory.Add(
                currentIngredient.ingredientType);
            }

            // Destruir ingrediente viejo
            Destroy(heldObject);

            heldObject = null;
        }

        // Guardar ingrediente
        heldObject = newIngredient;

        AudioManager.Instance.PlaySFX(
        AudioManager.Instance.pickupSFX
        );

        Rigidbody rb =
        heldObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Hacer hijo del HoldPoint
        heldObject.transform.parent =
        holdPoint;

        // Posici n exacta en mano
        heldObject.transform.localPosition =
        Vector3.zero;

        // Rotaci n exacta
        heldObject.transform.localRotation =
        Quaternion.identity;

        // Escala normal
        heldObject.transform.localScale =
        Vector3.one;

        Debug.Log(gameObject.name +
        " agarr  ingrediente");
    }

    void DropIngredient()
    {
        if (heldObject == null)
            return;

        GameObject ingredientToDrop =
        heldObject;

        heldObject = null;

        // Quitar padre
        ingredientToDrop.transform.parent =
        null;

        // Tirarlo enfrente del jugador
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
        " solt  ingrediente");
    }

    // Soltar cuando te roban
    void ForceDrop()
    {
        if (heldObject == null)
            return;

        GameObject ingredientToDrop =
        heldObject;

        heldObject = null;

        ingredientToDrop.transform.parent =
        null;

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

    // Dibujar rango
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
        transform.position,
        grabDistance);
    }
}