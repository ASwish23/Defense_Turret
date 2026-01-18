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
        if (!upgradeAlreadyDone && upgradedTurretPrefab != null && LevelManager.instance.SpendCurrency(rocketUnlockCost))
        {
            // 1. Salvăm datele de la vechea turetă (cea a colegului)
            int savedDmg = turret.turretDamage;
            float savedFR = turret.fireRate;
            float savedRng = turret.range;

            Vector3 pos = currentTurretGO.transform.position;
            Quaternion rot = currentTurretGO.transform.rotation;

            // 2. Distrugem tureta veche
            Destroy(currentTurretGO);

            // 3. Spawnam noua turetă (a TA)
            GameObject newTurret = Instantiate(upgradedTurretPrefab, pos, rot);
            newTurret.tag = "Turret"; // Important pentru inamici!

            // 4. ACTUALIZARE REFERINȚE:
            currentTurretGO = newTurret;

            // Căutăm noul tău script pe obiectul spawnat
            TurretV2 myNewTurretScript = newTurret.GetComponent<TurretV2>();

            if (myNewTurretScript != null)
            {
                myNewTurretScript.turretDamage = savedDmg + 2; // Bonus de upgrade
                myNewTurretScript.fireRate = savedFR + 0.5f;
                myNewTurretScript.range = savedRng;
                myNewTurretScript.hasRockets = true;
            }

            upgradeAlreadyDone = true;
        }
    }
}