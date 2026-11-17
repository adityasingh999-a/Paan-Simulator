using UnityEngine;

public class TrafficVehicle : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float acceleration = 3f;
    public float braking = 6f;
    public float turnSpeed = 5f;
    public float waypointDistance = 2f;

    [Header("Traffic Detection")]
    public float detectionDistance = 6f;
    public float safeDistance = 3f;
    public LayerMask vehicleLayer;

    [Header("Route")]
    public TrafficWaypoint currentWaypoint;

    private float currentSpeed;

    private void Start()
    {
        currentSpeed = 0f;
    }

    private void Update()
    {
        if (currentWaypoint == null)
            return;

        FollowRoad();
    }

    private void FollowRoad()
    {
        Vector3 target = currentWaypoint.transform.position;

        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.magnitude < waypointDistance)
        {
            SelectNextWaypoint();
            return;
        }

        direction.Normalize();

        bool blocked = CheckTraffic();

        float targetSpeed = blocked ? 0f :
            Mathf.Min(maxSpeed, currentWaypoint.speedLimit);

        if (currentSpeed < targetSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= braking * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, targetSpeed);

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );

        transform.position +=
            transform.forward * currentSpeed * Time.deltaTime;
    }

    private bool CheckTraffic()
{
    Vector3 origin = transform.position + Vector3.up * 0.7f;

    RaycastHit[] hits = Physics.SphereCastAll(
        origin,
        0.6f,
        transform.forward,
        detectionDistance
    );

    foreach (RaycastHit hit in hits)
    {
        TrafficVehicle otherCar =
            hit.collider.GetComponentInParent<TrafficVehicle>();

        if (otherCar != null && otherCar != this)
        {
            return hit.distance < safeDistance;
        }
    }

    return false;
}

    private void SelectNextWaypoint()
    {
        if (currentWaypoint.nextWaypoints == null ||
            currentWaypoint.nextWaypoints.Length == 0)
        {
            currentSpeed = 0f;
            return;
        }

        int index = Random.Range(
            0,
            currentWaypoint.nextWaypoints.Length
        );

        currentWaypoint =
            currentWaypoint.nextWaypoints[index];
    }
}