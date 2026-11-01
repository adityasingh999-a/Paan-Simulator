using System.Collections;
using UnityEngine;
using TMPro;

public class DealCompletionUI : MonoBehaviour
{
    public static DealCompletionUI Instance;

    public CanvasGroup canvasGroup;
    public TMP_Text completionText;
    public TMP_Text moneyRewardText;

    public float displayDuration = 3f;
    public float fadeDuration = 0.5f;

    private Coroutine currentRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowCompletion(string customerName, int satisfaction, int money)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowRoutine(customerName, satisfaction, money));
    }

    IEnumerator ShowRoutine(string customerName, int satisfaction, int money)
    {
        if (completionText != null)
        {
            completionText.text = "SATISFACTION: +" + satisfaction + " " + customerName.ToUpper();
        }

        if (moneyRewardText != null)
        {
            moneyRewardText.text = "+Rs. " + money;
        }

        yield return StartCoroutine(Fade(0, 1));

        yield return new WaitForSeconds(displayDuration);

        yield return StartCoroutine(Fade(1, 0));

        currentRoutine = null;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                elapsed / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}