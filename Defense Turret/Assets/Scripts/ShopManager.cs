using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("References")]
    public Turret standardTurret;      // Tureta de Nivel 1
    public TurretV2 rocketTurret;      // Tureta de Nivel 2 (V2)

    [Header("Costs")]
    public int damageCost = 50;
    public int speedCost = 75;
    public int rangeCost = 100;
    public int rocketUnlockCost = 500;

    [Header("UI Buttons Text")]
    // (Optional) Aici tragi textele de pe butoane ca sa actualizezi pretul vizual
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI speedCostText;
    public TextMeshProUGUI rangeCostText;

    // --- FUNCTII PENTRU UPGRADE ---

    public void PurchaseDamageUpgrade()
    {
        if (LevelManager.instance.currentCurrency >= damageCost)
        {
            LevelManager.instance.SpendCurrency(damageCost);

            // Verificam care tureta e activa
            if (rocketTurret != null && rocketTurret.gameObject.activeInHierarchy)
            {
                // Upgrade pentru V2
                rocketTurret.turretDamage += 1;
                Debug.Log("V2 Damage Upgraded!");
            }
            else
            {
                // Upgrade pentru V1
                standardTurret.turretDamage += 1;
                Debug.Log("V1 Damage Upgraded!");
            }

            // Optional: Creste pretul pentru urmatorul upgrade
            // damageCost += 25; 
            // UpdateUI();
        }
        else
        {
            Debug.Log("Nu ai bani de Damage!");
        }
    }

    public void PurchaseSpeedUpgrade()
    {
        if (LevelManager.instance.currentCurrency >= speedCost)
        {
            LevelManager.instance.SpendCurrency(speedCost);

            if (rocketTurret != null && rocketTurret.gameObject.activeInHierarchy)
            {
                // V2 trage mai repede (FireRate creste)
                rocketTurret.fireRate += 0.2f;
            }
            else
            {
                // V1 trage mai repede
                standardTurret.fireRate += 0.2f;
            }
        }
        else
        {
            Debug.Log("Nu ai bani de Viteza!");
        }
    }

    public void PurchaseRangeUpgrade()
    {
        if (LevelManager.instance.currentCurrency >= rangeCost)
        {
            LevelManager.instance.SpendCurrency(rangeCost);

            if (rocketTurret != null && rocketTurret.gameObject.activeInHierarchy)
            {
                rocketTurret.range += 1f;
            }
            else
            {
                standardTurret.range += 1f;
            }
        }
        else
        {
            Debug.Log("Nu ai bani de Range!");
        }
    }

    public void UnlockRockets()
    {
        // Aceasta functie face tranzitia de la V1 la V2
        if (LevelManager.instance.currentCurrency >= rocketUnlockCost)
        {
            // Verificam daca nu cumva avem deja rachetele
            if (rocketTurret.gameObject.activeInHierarchy)
            {
                Debug.Log("Ai deja rachetele!");
                return;
            }

            LevelManager.instance.SpendCurrency(rocketUnlockCost);

            // OPRIM Tureta 1
            standardTurret.gameObject.SetActive(false);

            // PORNIM Tureta 2
            rocketTurret.gameObject.SetActive(true);

            // BONUS: Putem transfera stats-urile de la Tureta 1 la Tureta 2 
            // ca sa nu o iei de la zero (OPTIONAL)
            rocketTurret.turretDamage = standardTurret.turretDamage + 1; // +1 bonus
            rocketTurret.fireRate = standardTurret.fireRate;
            rocketTurret.range = standardTurret.range;

            Debug.Log("ROCKETS UNLOCKED!");
        }
        else
        {
            Debug.Log("Nu ai 500$ pentru rachete!");
        }
    }

    /* void UpdateUI() {
       if(damageCostText != null) damageCostText.text = "Upgrade Damage (" + damageCost + "$)";
       // etc...
    }
    */
}