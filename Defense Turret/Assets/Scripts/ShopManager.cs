using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Referinte Tureta")]
    public Turret turret;               // Referinta la SCRIPTUL turetei din scena
    public GameObject currentTurretGO;  // Referinta la OBIECTUL turetei din scena
    public GameObject upgradedTurretPrefab; // Prefab-ul turetei noi (cea cu 2 tevi)

    [Header("Referinte UI Butoane")]
    public Button damageButton;
    public Button fireRateButton;
    public Button rangeButton;
    public Button rocketButton;

    [Header("Upgrade Costs")]
    public int damageCost = 50;
    public int fireRateCost = 75;
    public int rangeCost = 100;
    public int rocketUnlockCost = 500;

    private bool upgradeAlreadyDone = false; // Flag ca sa stim daca am cumparat deja

    void Update()
    {
        if (LevelManager.instance != null && turret != null)
        {
            int baniDisponibili = LevelManager.instance.currentCurrency;

            // Upgrade-uri normale
            if (damageButton != null) damageButton.interactable = (baniDisponibili >= damageCost);
            if (fireRateButton != null) fireRateButton.interactable = (baniDisponibili >= fireRateCost);
            if (rangeButton != null) rangeButton.interactable = (baniDisponibili >= rangeCost);

            // Logica Buton Rachete
            if (rocketButton != null)
            {
                TextMeshProUGUI btnText = rocketButton.GetComponentInChildren<TextMeshProUGUI>();

                if (upgradeAlreadyDone)
                {
                    rocketButton.interactable = false;
                    if (btnText != null) btnText.text = "MAX LEVEL";
                }
                else
                {
                    // Afisam textul dorit de tine
                    if (btnText != null) btnText.text = "Unlock Rocket (" + rocketUnlockCost + "$)";

                    // Se activeaza doar la 500 credits
                    rocketButton.interactable = (baniDisponibili >= rocketUnlockCost);
                }
            }
        }
    }

    public void BuyDamageUpgrade()
    {
        if (LevelManager.instance.SpendCurrency(damageCost))
        {
            turret.turretDamage += 1;
            damageCost += 25;
        }
    }

    public void BuyFireRateUpgrade()
    {
        if (LevelManager.instance.SpendCurrency(fireRateCost))
        {
            turret.fireRate += 0.5f;
            fireRateCost += 50;
        }
    }

    public void BuyRangeUpgrade()
    {
        if (LevelManager.instance.SpendCurrency(rangeCost))
        {
            turret.range += 2f;
            rangeCost += 30;
        }
    }

    public void BuyRocketLauncher()
    {
        // Verificam banii si daca avem prefab-ul setat
        if (!upgradeAlreadyDone && upgradedTurretPrefab != null && LevelManager.instance.SpendCurrency(rocketUnlockCost))
        {
            // Salvam datele
            int currentDmg = turret.turretDamage;
            float currentFR = turret.fireRate;
            float currentRng = turret.range;

            Vector3 position = currentTurretGO.transform.position;
            Quaternion rotation = currentTurretGO.transform.rotation;

            // Schimbam obiectele
            Destroy(currentTurretGO);
            GameObject newTurret = Instantiate(upgradedTurretPrefab, position, rotation);

            // Actualizam referintele interne
            currentTurretGO = newTurret;
            turret = newTurret.GetComponent<Turret>();

            // Aplicam statisticile vechi + deblocam rachetele
            turret.turretDamage = currentDmg;
            turret.fireRate = currentFR;
            turret.range = currentRng;
            turret.hasRockets = true;

            // Marcam ca upgrade-ul a fost facut
            upgradeAlreadyDone = true;

            Debug.Log("Upgrade reusit la Tureta cu 2 tevi!");
        }
        else if (upgradedTurretPrefab == null)
        {
            Debug.LogError("Atentie! Nu ai tras Prefab-ul turetei noi in Inspector la ShopManager!");
        }
    }
}