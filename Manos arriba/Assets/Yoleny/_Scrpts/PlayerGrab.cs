using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    // =====================================================
    // INVENTARIO
    // =====================================================

    public List<IngredientType> inventory =
        new List<IngredientType>();


    // =====================================================
    // OBJETO EN LA MANO
    // =====================================================

    public GameObject heldObject;

    // Punto donde se sostiene el ingrediente
    public Transform holdPoint;

    // Distancia para agarrar ingredientes
    public float grabDistance = 2f;


    // =====================================================
    // PUNTAJE DE ROBOS
    // =====================================================

    [Header("Puntaje")]

    // Cantidad de ingredientes robados durante esta ronda
    public int stolenIngredientsThisRound = 0;


    // =====================================================
    // TECLAS
    // =====================================================

    public KeyCode grabKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;


    // =====================================================
    // UPDATE
    // =====================================================

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


    // =====================================================
    // BUSCAR INGREDIENTE
    // =====================================================

    void TryPickIngredient()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                grabDistance
            );

        foreach (Collider hit in hits)
        {
            // =============================================
            // PROTECCIÓN:
            // NO AGARRAR EL PROPIO JUGADOR
            // =============================================

            if (hit.transform.root == transform.root)
            {
                continue;
            }


            // =============================================
            // REVISAR TAG
            // =============================================

            if (!hit.CompareTag("Ingredient"))
            {
                continue;
            }


            // =============================================
            // COMPROBAR QUE REALMENTE SEA INGREDIENTE
            // =============================================

            Ingredient ingredient =
                hit.GetComponent<Ingredient>();

            if (ingredient == null)
            {
                continue;
            }


            GameObject newIngredient =
                hit.gameObject;


            // Evitar agarrar el mismo objeto
            if (newIngredient == heldObject)
            {
                continue;
            }


            // =============================================
            // AGARRAR
            // =============================================

            PickIngredient(newIngredient);

            break;
        }
    }


    // =====================================================
    // AGARRAR INGREDIENTE
    // =====================================================

    void PickIngredient(GameObject newIngredient)
    {
        // =============================================
        // PROTECCIONES
        // =============================================

        if (newIngredient == null)
        {
            return;
        }


        // Nunca agarrar al propio jugador
        if (newIngredient.transform.root == transform.root)
        {
            return;
        }


        // Comprobar que tenga Ingredient
        Ingredient ingredient =
            newIngredient.GetComponent<Ingredient>();

        if (ingredient == null)
        {
            return;
        }


        // =============================================
        // BUSCAR JUGADORES
        // =============================================

        PlayerGrab[] players =
            FindObjectsOfType<PlayerGrab>();


        // =============================================
        // ROBAR INGREDIENTE
        // =============================================

        foreach (PlayerGrab player in players)
        {
            if (player != this &&
                player.heldObject == newIngredient)
            {
                // Contar el robo
                stolenIngredientsThisRound++;

                // Hacer que el otro jugador lo suelte
                player.ForceDrop();

                Debug.Log(
                    gameObject.name +
                    " ROBÓ un ingrediente a " +
                    player.gameObject.name
                );
            }
        }


        // =============================================
        // SI YA TENGO UN INGREDIENTE
        // =============================================

        if (heldObject != null)
        {
            Ingredient currentIngredient =
                heldObject.GetComponent<Ingredient>();

            if (currentIngredient != null)
            {
                inventory.Add(
                    currentIngredient.ingredientType
                );
            }


            // Destruir ingrediente anterior
            Destroy(heldObject);

            heldObject = null;
        }


        // =============================================
        // GUARDAR INGREDIENTE
        // =============================================

        heldObject = newIngredient;


        // =============================================
        // SONIDO
        // =============================================

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                AudioManager.Instance.pickupSFX
            );
        }


        // =============================================
        // RIGIDBODY
        // =============================================

        Rigidbody rb =
            heldObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = false;
            rb.isKinematic = true;
        }


        // =============================================
        // HACER HIJO DEL HOLD POINT
        // =============================================

        if (holdPoint != null)
        {
            heldObject.transform.SetParent(
                holdPoint
            );


            // Posición exacta en la mano
            heldObject.transform.localPosition =
                Vector3.zero;


            // Rotación exacta
            heldObject.transform.localRotation =
                Quaternion.identity;


            // Escala normal
            heldObject.transform.localScale =
                Vector3.one;
        }
        else
        {
            Debug.LogWarning(
                gameObject.name +
                ": No hay Hold Point asignado."
            );
        }


        Debug.Log(
            gameObject.name +
            " agarró ingrediente: " +
            newIngredient.name
        );
    }


    // =====================================================
    // SOLTAR INGREDIENTE
    // =====================================================

    void DropIngredient()
    {
        if (heldObject == null)
        {
            return;
        }


        GameObject ingredientToDrop =
            heldObject;


        heldObject = null;


        // =============================================
        // QUITAR PADRE
        // =============================================

        ingredientToDrop.transform.SetParent(null);


        // =============================================
        // POSICIÓN DEL INGREDIENTE
        // =============================================

        if (holdPoint != null)
        {
            ingredientToDrop.transform.position =
                holdPoint.position +
                transform.forward;
        }
        else
        {
            ingredientToDrop.transform.position =
                transform.position +
                transform.forward;
        }


        // =============================================
        // RIGIDBODY
        // =============================================

        Rigidbody rb =
            ingredientToDrop.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }


        Debug.Log(
            gameObject.name +
            " soltó ingrediente"
        );
    }


    // =====================================================
    // FORZAR SOLTAR
    // =====================================================

    public void ForceDrop()
    {
        if (heldObject == null)
        {
            return;
        }


        GameObject ingredientToDrop =
            heldObject;


        heldObject = null;


        // Quitar padre
        ingredientToDrop.transform.SetParent(null);


        // =============================================
        // RIGIDBODY
        // =============================================

        Rigidbody rb =
            ingredientToDrop.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }


        Debug.Log(
            gameObject.name +
            " perdió su ingrediente porque otro jugador lo robó."
        );
    }


    // =====================================================
    // GIZMO
    // =====================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            grabDistance
        );
    }
}

