using UnityEngine;
using TMPro; // Ai nevoie de asta pentru UI-ul de text (daca folosesti TextMeshPro)

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Singleton pentru acces u?or

    public int currentCurrency = 500;
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
            return true; // Tranzac?ie reu?it�
        }
        else
        {
            Debug.Log("Fonduri insuficiente!");
            return false; // Tranzac?ie e?uat�
        }
    }

    void UpdateUI()
    {
        if (currencyText != null)
            currencyText.text = "Credits: " + currentCurrency.ToString();
    }
}