using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int money = 100;
    public TMP_Text moneyText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        UpdateMoneyText();
    }

    public void EarnMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();

        Debug.Log("Earned Rs. " + amount);
        Debug.Log("Total Money: Rs. " + money);
    }

    public void SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyText();

            Debug.Log("Spent Rs. " + amount);
            Debug.Log("Remaining Money: Rs. " + money);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: Rs" + money;
        }
    }
}