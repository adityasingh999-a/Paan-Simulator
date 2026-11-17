using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Vehicle Prefabs")]
    public GameObject[] carPrefabs;

    [Header("Spawn Configuration")]
    public Transform[] spawnPoints;
    public TrafficWaypoint[] startingWaypoints;

    [Header("Traffic Settings")]
    public int initialCars = 5;
    public int maxCars = 8;

    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;

    [Header("Spawn Safety")]
    public float minDistanceBetweenCars = 12f;
    public float spawnClearanceRadius = 4f;
    public float spawnRetryInterval = 1f;

    [Header("Speed Settings")]
    public float minSpeed = 5f;
    public float maxSpeed = 9f;

    private List<GameObject> activeCars = new List<GameObject>();

    private int lastCarIndex = -1;

    private void Start()
    {
        initialCars = Mathf.Clamp(initialCars, 0, maxCars);

        StartCoroutine(InitializeTraffic());
    }

    private IEnumerator InitializeTraffic()
    {
        int spawnedCars = 0;

        while (spawnedCars < initialCars)
        {
            activeCars.RemoveAll(car => car == null);

            if (activeCars.Count < maxCars)
            {
                if (SpawnCar())
                {
                    spawnedCars++;
                }
            }

            yield return new WaitForSeconds(spawnRetryInterval);
        }

        StartCoroutine(SpawnTraffic());
    }

    private IEnumerator SpawnTraffic()
    {
        while (true)
        {
            float waitTime = Random.Range(
                minSpawnInterval,
                maxSpawnInterval
            );

            yield return new WaitForSeconds(waitTime);

            activeCars.RemoveAll(car => car == null);

            if (activeCars.Count < maxCars)
            {
                SpawnCar();
            }
        }
    }

    private bool SpawnCar()
    {
        if (carPrefabs == null || carPrefabs.Length == 0)
            return false;

        if (spawnPoints == null || spawnPoints.Length == 0)
            return false;

        if (startingWaypoints == null ||
            startingWaypoints.Length != spawnPoints.Length)
            return false;

        List<int> availablePoints = new List<int>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null ||
                startingWaypoints[i] == null)
                continue;

            bool occupied = false;

            foreach (GameObject existingCar in activeCars)
            {
                if (existingCar == null)
                    continue;

                float distance = Vector3.Distance(
                    spawnPoints[i].position,
                    existingCar.transform.position
                );

                if (distance < minDistanceBetweenCars)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                Collider[] nearbyObjects = Physics.OverlapSphere(
                    spawnPoints[i].position,
                    spawnClearanceRadius
                );

                foreach (Collider nearby in nearbyObjects)
                {
                    if (nearby.GetComponentInParent<TrafficVehicle>() != null)
                    {
                        occupied = true;
                        break;
                    }
                }
            }

            if (!occupied)
            {
                availablePoints.Add(i);
            }
        }

        if (availablePoints.Count == 0)
            return false;

        int selectedPoint = availablePoints[
            Random.Range(0, availablePoints.Count)
        ];

        int carIndex = Random.Range(0, carPrefabs.Length);

        if (carPrefabs.Length > 1 && carIndex == lastCarIndex)
        {
            carIndex = (carIndex + 1) % carPrefabs.Length;
        }

        lastCarIndex = carIndex;

        GameObject car = Instantiate(
            carPrefabs[carIndex],
            spawnPoints[selectedPoint].position,
            spawnPoints[selectedPoint].rotation
        );

        car.name = carPrefabs[carIndex].name + "_Traffic";

        TrafficVehicle vehicle = car.GetComponent<TrafficVehicle>();

        if (vehicle == null)
        {
            vehicle = car.AddComponent<TrafficVehicle>();
        }

        vehicle.currentWaypoint = startingWaypoints[selectedPoint];

        vehicle.maxSpeed = Random.Range(minSpeed, maxSpeed);

        activeCars.Add(car);

        return true;
    }

    public void RemoveCar(GameObject car)
    {
        if (activeCars.Contains(car))
        {
            activeCars.Remove(car);
            Destroy(car);
        }
    }

    public int GetActiveCarCount()
    {
        activeCars.RemoveAll(car => car == null);

        return activeCars.Count;
    }
}