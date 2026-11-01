using UnityEngine;
using TMPro;

public class CustomerController : MonoBehaviour
{
    public string customerName = "Ramesh";
    public string requestedPaan = "Meetha Paan";
    public Transform spawnPosition;
    public Transform counterPosition;
    public TMP_Text orderText;
    public TMP_Text satisfactionText;
    public float moveSpeed = 2f;

    public float satisfactionTimeLimit = 45f;
    public int satisfactionPoints;

    private bool orderCompleted = false;
    private bool leaving = false;
    private Vector3 targetPosition;
    private float orderStartTime;

    void Start()
    {
        if (spawnPosition != null)
        {
            transform.position = spawnPosition.position;
        }

        if (counterPosition != null)
        {
            targetPosition = counterPosition.position;
        }

        orderStartTime = Time.time;

        UpdateOrderText();

        Debug.Log(customerName + " entered the shop.");
        Debug.Log(customerName + " ordered " + requestedPaan + ".");
    }

    void Update()
    {
        if (counterPosition == null)
        {
            return;
        }

        if (!orderCompleted && !leaving)
        {
            MoveToCounter();
        }

        if (leaving)
        {
            LeaveShop();
        }
    }

    void MoveToCounter()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    public bool ServePaan(string paanName, int price)
    {
        if (orderCompleted || leaving)
        {
            return false;
        }

        if (paanName != requestedPaan)
        {
            if (orderText != null)
            {
                orderText.text = customerName + ": Wrong paan!";
            }

            Debug.Log(customerName + " received the wrong paan.");

            return false;
        }

        float waitingTime = Time.time - orderStartTime;

        if (waitingTime <= 15f)
        {
            satisfactionPoints = 20;
        }
        else if (waitingTime <= 30f)
        {
            satisfactionPoints = 15;
        }
        else if (waitingTime <= satisfactionTimeLimit)
        {
            satisfactionPoints = 10;
        }
        else
        {
            satisfactionPoints = 5;
        }

        orderCompleted = true;
        leaving = true;

        if (orderText != null)
        {
            orderText.text = customerName + ": Order completed!";
        }

        if (satisfactionText != null)
        {
            satisfactionText.text = "Satisfaction: +" + satisfactionPoints;
        }

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.EarnMoney(price);
        }

        if (DealCompletionUI.Instance != null)
        {
            DealCompletionUI.Instance.ShowCompletion(
                customerName,
                satisfactionPoints,
                price
            );
        }

        Debug.Log(customerName + " received the correct paan.");
        Debug.Log(customerName + " paid Rs. " + price + ".");
        Debug.Log(customerName + " satisfaction: +" + satisfactionPoints);
        Debug.Log("Customer waiting time: " + waitingTime.ToString("F1") + " seconds.");

        return true;
    }

    void UpdateOrderText()
    {
        if (orderText != null)
        {
            orderText.text = customerName + " wants: " + requestedPaan;
        }
    }

    void LeaveShop()
    {
        Vector3 exitPosition = spawnPosition != null
            ? spawnPosition.position
            : transform.position + Vector3.forward * 5f;

        transform.position = Vector3.MoveTowards(
            transform.position,
            exitPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, exitPosition) < 0.1f)
        {
            Debug.Log(customerName + " left the shop.");
            Destroy(gameObject);
        }
    }

    public bool HasCompletedOrder()
    {
        return orderCompleted;
    }
}