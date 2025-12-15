using UnityEngine;
using TMPro; // Ai nevoie de asta pentru UI-ul de text (daca folosesti TextMeshPro)

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Singleton pentru acces u?or

    public int currentCurrency = 0;
    public TextMeshProUGUI currencyText; // Trage textul de pe ecran aici

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            UpdateUI();
            return true; // Tranzac?ie reu?itã
        }
        else
        {
            Debug.Log("Fonduri insuficiente!");
            return false; // Tranzac?ie e?uatã
        }
    }

    void UpdateUI()
    {
        if (currencyText != null)
            currencyText.text = "Credits: " + currentCurrency.ToString();
    }
}