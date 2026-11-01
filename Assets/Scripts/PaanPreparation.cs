using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaanPreparation : MonoBehaviour
{
    public TMP_Text recipeText;
    public CustomerController currentCustomer;

    private HashSet<string> ingredients = new HashSet<string>();
    private List<PickableObject> usedIngredients = new List<PickableObject>();
    private string preparedPaanName = "";
    private int preparedPaanPrice = 0;

    void Start()
    {
        UpdateRecipeText();
    }

    public void AddIngredient(PickableObject ingredient)
    {
        if (ingredient == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(preparedPaanName))
        {
            ShowMessage("Serve the prepared paan first!");
            return;
        }

        string ingredientName = ingredient.ingredientName;

        if (ingredients.Contains(ingredientName))
        {
            ShowMessage(ingredientName + " is already added!");
            return;
        }

        ingredients.Add(ingredientName);
        usedIngredients.Add(ingredient);
        ingredient.Consume();

        UpdateRecipeText();
    }

    public void FinishRecipe()
    {
        if (!string.IsNullOrEmpty(preparedPaanName))
        {
            ShowMessage("Serve the prepared paan first!");
            return;
        }

        bool hasLeaf = ingredients.Contains("PaanLeaf");
        bool hasGulkand = ingredients.Contains("Gulkand");
        bool hasSupari = ingredients.Contains("Supari");

        string paanName = "";
        int price = 0;

        if (hasLeaf && hasGulkand && hasSupari)
        {
            paanName = "Special Paan";
            price = 50;
        }
        else if (hasLeaf && hasGulkand)
        {
            paanName = "Meetha Paan";
            price = 30;
        }
        else if (hasLeaf && hasSupari)
        {
            paanName = "Saada Paan";
            price = 20;
        }

        if (string.IsNullOrEmpty(paanName))
        {
            RestockIngredients();
            ResetPreparedPaan();
            ShowMessage("Not enough ingredients! Try again.");
            return;
        }

        preparedPaanName = paanName;
        preparedPaanPrice = price;

        RestockIngredients();

        ShowMessage(
            preparedPaanName + " prepared!\n" +
            "Price: Rs. " + preparedPaanPrice +
            "\nLook at the customer and press E to serve."
        );
    }

    public bool ServeToCustomer(CustomerController customer)
    {
        if (customer == null)
        {
            ShowMessage("No customer to serve!");
            return false;
        }

        if (string.IsNullOrEmpty(preparedPaanName))
        {
            ShowMessage("Prepare a paan first!");
            return false;
        }

        string paanBeingServed = preparedPaanName;
        int priceBeingPaid = preparedPaanPrice;

        bool served = customer.ServePaan(paanBeingServed, priceBeingPaid);

        ResetPreparedPaan();

        if (served)
        {
            ShowMessage("Order completed! Prepare for the next customer.");
        }
        else
        {
            ShowMessage(
                "Wrong paan! Customer wants " +
                customer.requestedPaan +
                ". Prepare the correct paan."
            );
        }

        return served;
    }

    void ResetPreparedPaan()
    {
        preparedPaanName = "";
        preparedPaanPrice = 0;
        ingredients.Clear();
        usedIngredients.Clear();

        UpdateRecipeText();
    }

    void RestockIngredients()
    {
        foreach (PickableObject ingredient in usedIngredients)
        {
            if (ingredient != null)
            {
                ingredient.Restock();
            }
        }

        usedIngredients.Clear();
        ingredients.Clear();
    }

    void ShowMessage(string message)
    {
        Debug.Log(message);

        if (recipeText != null)
        {
            recipeText.text = message;
        }
    }

    void UpdateRecipeText()
    {
        if (recipeText == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(preparedPaanName))
        {
            recipeText.text = preparedPaanName +
                " is ready to serve!\nLook at the customer and press E.";
        }
        else if (ingredients.Count == 0)
        {
            recipeText.text = "Ingredients: None\nPress F to finish paan";
        }
        else
        {
            recipeText.text = "Ingredients: " +
                string.Join(", ", ingredients) +
                "\nPress F to finish paan";
        }
    }
}