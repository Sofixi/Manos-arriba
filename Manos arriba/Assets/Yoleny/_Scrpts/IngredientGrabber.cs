using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGrabber : MonoBehaviour
{
    public Transform holdPoint;

    public GameObject heldIngredient;

    private void Update()
    {
        // Soltar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropIngredient();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Ingredient ingredient =
        other.GetComponent<Ingredient>();

        if (ingredient == null)
            return;

        // Agarrar / robar
        if (Input.GetKeyDown(KeyCode.E))
        {
            GrabIngredient(other.gameObject);
        }
    }

    void GrabIngredient(GameObject ingredientObject)
    {
        // Si ya tengo algo, lo suelto primero
        if (heldIngredient != null)
        {
            DropIngredient();
        }

        // Buscar TODOS los jugadores
        IngredientGrabber[] players =
        FindObjectsOfType<IngredientGrabber>();

        // Quitárselo al que lo tenga
        foreach (IngredientGrabber player in players)
        {
            if (player.heldIngredient ==
                ingredientObject)
            {
                player.heldIngredient = null;
            }
        }

        // Ahora este jugador lo toma
        heldIngredient = ingredientObject;

        ingredientObject.transform.SetParent(
        holdPoint);

        ingredientObject.transform.localPosition =
        Vector3.zero;

        ingredientObject.transform.localRotation =
        Quaternion.identity;

        Debug.Log("Ingrediente robado");
    }

    void DropIngredient()
    {
        if (heldIngredient == null)
            return;

        heldIngredient.transform.SetParent(null);

        heldIngredient = null;
    }
}