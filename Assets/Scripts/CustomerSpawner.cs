using UnityEngine;
using TMPro;

public class CustomerSpawner : MonoBehaviour
{
public GameObject customerPrefab;
public Transform spawnPosition;
public Transform counterPosition;
public PaanPreparation preparationStation;
public TMP_Text orderText;


public float timeBetweenCustomers = 5f;

private float nextCustomerTime;
private CustomerController currentCustomer;

private string[] paanOrders =
{
    "Meetha Paan",
    "Saada Paan",
    "Special Paan"
};

void Start()
{
    if (orderText != null)
    {
        orderText.text = "Waiting for customer...";
    }

    nextCustomerTime = Time.time;
}

void Update()
{
    if (currentCustomer == null && Time.time >= nextCustomerTime)
    {
        SpawnCustomer();
    }
}

void SpawnCustomer()
{
    if (customerPrefab == null || spawnPosition == null)
    {
        return;
    }

    GameObject customer = Instantiate(
        customerPrefab,
        spawnPosition.position,
        Quaternion.identity
    );

    currentCustomer = customer.GetComponent<CustomerController>();

    if (currentCustomer != null)
    {
        currentCustomer.spawnPosition = spawnPosition;
        currentCustomer.counterPosition = counterPosition;

        int orderIndex = Random.Range(0, paanOrders.Length);
        currentCustomer.requestedPaan = paanOrders[orderIndex];

        if (orderText != null)
        {
            orderText.text = "Customer Order: " + currentCustomer.requestedPaan;
        }

        if (preparationStation != null)
        {
            preparationStation.currentCustomer = currentCustomer;
        }
    }
    else
    {
        Debug.LogError("CustomerController is missing from the customer prefab!");
    }
}

public void CustomerLeft()
{
    currentCustomer = null;
    nextCustomerTime = Time.time + timeBetweenCustomers;

    if (orderText != null)
    {
        orderText.text = "Waiting for next customer...";
    }
}


}
