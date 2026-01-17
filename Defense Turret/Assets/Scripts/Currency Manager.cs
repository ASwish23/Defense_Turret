using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int credits = 0;
    public TMP_Text creditsText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCredits(int amount)
    {
        credits += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (creditsText != null)
            creditsText.text = "Credits: " + credits;
    }
}
