using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private bool isPickedUp = false;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public string ingredientName;

    void Start()
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (string.IsNullOrEmpty(ingredientName))
        {
            ingredientName = gameObject.name;
        }
    }

    public void PickUp(Transform holdPosition)
    {
        if (isPickedUp || !gameObject.activeSelf)
        {
            return;
        }

        isPickedUp = true;
        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void PlaceDown()
    {
        if (!isPickedUp)
        {
            return;
        }

        isPickedUp = false;
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    public void Consume()
    {
        isPickedUp = false;
        gameObject.SetActive(false);
    }

    public void Restock()
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        gameObject.SetActive(true);
        isPickedUp = false;
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }
}