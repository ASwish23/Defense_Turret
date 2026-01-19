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

<<<<<<< Updated upstream
    private bool upgradeAlreadyDone = false; // Flag ca sa stim daca am cumparat deja
=======
    [Header("UI Buttons Text")]
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI speedCostText;
    public TextMeshProUGUI rangeCostText;
>>>>>>> Stashed changes

    void Update()
    {
        if (LevelManager.instance != null && turret != null)
        {
            int baniDisponibili = LevelManager.instance.currentCurrency;

<<<<<<< Updated upstream
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
=======
            if (rocketTurret != null && rocketTurret.gameObject.activeInHierarchy)
            {
                rocketTurret.turretDamage += 1;
                Debug.Log("V2 Damage Upgraded!");
            }
            else
            {
                standardTurret.turretDamage += 1;
                Debug.Log("V1 Damage Upgraded!");
            }
        }
        else
        {
            Debug.Log("Nu ai bani de Damage!");
>>>>>>> Stashed changes
        }
    }

    public void BuyDamageUpgrade()
    {
        if (LevelManager.instance.SpendCurrency(damageCost))
        {
<<<<<<< Updated upstream
            turret.turretDamage += 1;
            damageCost += 25;
=======
            LevelManager.instance.SpendCurrency(speedCost);

            if (rocketTurret != null && rocketTurret.gameObject.activeInHierarchy)
            {
                rocketTurret.fireRate += 0.2f;
            }
            else
            {
                standardTurret.fireRate += 0.2f;
            }
        }
        else
        {
            Debug.Log("Nu ai bani de Viteza!");
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
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
=======
        if (LevelManager.instance.currentCurrency >= rocketUnlockCost)
        {
            if (rocketTurret.gameObject.activeInHierarchy)
>>>>>>> Stashed changes
            {
                myNewTurretScript.turretDamage = savedDmg + 2; // Bonus de upgrade
                myNewTurretScript.fireRate = savedFR + 0.5f;
                myNewTurretScript.range = savedRng;
                myNewTurretScript.hasRockets = true;
            }

<<<<<<< Updated upstream
            upgradeAlreadyDone = true;
=======
            LevelManager.instance.SpendCurrency(rocketUnlockCost);

            // 1. OPRIM Tureta 1
            standardTurret.gameObject.SetActive(false);

            // 2. PORNIM Tureta 2
            rocketTurret.gameObject.SetActive(true);

            // 3. Transferam stats-urile
            rocketTurret.turretDamage = standardTurret.turretDamage + 1;
            rocketTurret.fireRate = standardTurret.fireRate;
            rocketTurret.range = standardTurret.range;

            // --- AM SCOS PARTEA CU ACTIVATE HARD MODE ---
            // Acum jocul isi va continua valurile normale, definite in Inspector.

            Debug.Log("ROCKETS UNLOCKED!");
        }
        else
        {
            Debug.Log("Nu ai 500$ pentru rachete!");
>>>>>>> Stashed changes
        }
    }
}