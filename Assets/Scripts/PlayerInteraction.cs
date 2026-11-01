using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public Transform holdPosition;
    public float interactionDistance = 3f;

    private PickableObject currentObject;
    private PickableObject heldObject;
    private PaanPreparation currentStation;
    private CustomerController currentCustomer;

    void Update()
    {
        CheckForObject();

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (currentStation != null && heldObject == null)
            {
                currentStation.FinishRecipe();
            }
        }
    }

    void CheckForObject()
    {
        currentObject = null;
        currentStation = null;
        currentCustomer = null;

        if (playerCamera == null)
        {
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            currentObject = hit.collider.GetComponentInParent<PickableObject>();
            currentStation = hit.collider.GetComponentInParent<PaanPreparation>();
            currentCustomer = hit.collider.GetComponentInParent<CustomerController>();
        }
    }

    void Interact()
    {
        if (heldObject != null)
        {
            if (currentStation != null)
            {
                currentStation.AddIngredient(heldObject);
                heldObject = null;
            }
            else
            {
                heldObject.PlaceDown();
                heldObject = null;
            }

            return;
        }

        if (currentCustomer != null)
        {
            PaanPreparation station = FindFirstObjectByType<PaanPreparation>();

            if (station != null)
            {
                station.ServeToCustomer(currentCustomer);
            }

            return;
        }

        if (currentObject != null)
        {
            heldObject = currentObject;
            heldObject.PickUp(holdPosition);
        }
    }
}