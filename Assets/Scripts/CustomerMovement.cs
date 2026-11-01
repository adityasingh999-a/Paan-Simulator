using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
public Transform target;
public float moveSpeed = 2f;

void Update()
{
    if (target == null)
        return;

    Vector3 targetPosition = new Vector3(
        target.position.x,
        transform.position.y,
        target.position.z
    );

    transform.position = Vector3.MoveTowards(
        transform.position,
        targetPosition,
        moveSpeed * Time.deltaTime
    );

    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
    {
        transform.position = targetPosition;
    }
}
}
