using UnityEngine.UI; // Necesar pentru a controla Butoanele (interactable)
using UnityEngine;
using TMPro; // Adăugat pentru a putea modifica textul butonului (opțional)

public class ShopManager : MonoBehaviour
{
    public TurretController turret; // Trage obiectul Turret din scenă aici

    [Header("Referinte UI Butoane")]
    public Button damageButton;   // Trage Button_Damage
    public Button fireRateButton; // Trage Button_FireRate
    public Button rangeButton;    // Trage Button_Range
    public Button rocketButton;   // <--- TRAGE NOUL BUTON "Button_UnlockRocket" AICI

    [Header("Upgrade Costs")]
    public int damageCost = 50;
    public int fireRateCost = 75;
    public int rangeCost = 100;
    public int rocketUnlockCost = 500; // Costul pentru rachete

    void Update()
    {
        // --- LOGICA PENTRU BUTOANE GRI (Feedback Vizual) ---

        if (LevelManager.instance != null)
        {
            int baniDisponibili = LevelManager.instance.currentCurrency;

            // 1. Logica pentru Upgrade-uri (exact ca înainte)
            if (damageButton != null)
                damageButton.interactable = (baniDisponibili >= damageCost);

            if (fireRateButton != null)
                fireRateButton.interactable = (baniDisponibili >= fireRateCost);

            if (rangeButton != null)
                rangeButton.interactable = (baniDisponibili >= rangeCost);

            // 2. Logica specială pentru Rachete (NOU)
            if (rocketButton != null)
            {
                // Verificăm dacă le avem deja
                bool dejaCumparat = turret.hasRockets;

                if (dejaCumparat)
                {
                    // Dacă e cumpărat, butonul devine inactiv permanent
                    rocketButton.interactable = false;

                    // Opțional: Schimbăm textul în "OWNED" dacă butonul are componenta TextMeshPro
                    TextMeshProUGUI btnText = rocketButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (btnText != null) btnText.text = "OWNED";
                }
                else
                {
                    // Dacă NU e cumpărat, e activ doar dacă avem banii (500$)
                    rocketButton.interactable = (baniDisponibili >= rocketUnlockCost);
                }
            }
        }
    }

    // --- METODE CUMPĂRARE ---

    public void BuyDamageUpgrade()
    {
        if (LevelManager.instance.SpendCurrency(damageCost))
        {
            turret.damage += 1;
            damageCost += 25;
            Debug.Log("Damage Upgraded!");
        }
    }

    public void BuyFireRateUpgrade()
    {
        if (LevelManager.instance.SpendCurrency(fireRateCost))
        {
            turret.fireRate += 0.5f;
            fireRateCost += 50;
            Debug.Log("Fire Rate Upgraded!");
        }
    }

    public void BuyRangeUpgrade()
    {
        if (LevelManager.instance.SpendCurrency(rangeCost))
        {
            turret.range += 2f;
            rangeCost += 30;
            Debug.Log("Range Upgraded!");
        }
    }

    // --- METODĂ NOUĂ PENTRU RACHETE ---
    public void BuyRocketLauncher()
    {
        // Verificăm să nu fie deja cumpărat ȘI să avem bani
        if (!turret.hasRockets && LevelManager.instance.SpendCurrency(rocketUnlockCost))
        {
            turret.hasRockets = true; // Activăm racheta pe turetă!
            Debug.Log("Rockets Unlocked!");
            // Update-ul vizual se face automat în funcția Update()
        }
    }
}