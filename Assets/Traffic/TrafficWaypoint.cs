using UnityEngine;

public class TrafficWaypoint : MonoBehaviour
{
    public TrafficWaypoint[] nextWaypoints;

    public float speedLimit = 8f;

    public bool stopPoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.25f);

        if (nextWaypoints == null)
            return;

        Gizmos.color = Color.yellow;

        foreach (TrafficWaypoint waypoint in nextWaypoints)
        {
            if (waypoint != null)
            {
                Gizmos.DrawLine(
                    transform.position,
                    waypoint.transform.position
                );
            }
        }
    }
}