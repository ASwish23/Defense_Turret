using UnityEngine;

public class CreditPickup : MonoBehaviour
{
    public int value = 1;      // Câți bani valorează acest pickup
    public float moveSpeed = 5f; // Viteza cu care vine spre tine

    private Transform turret;

    void Start()
    {
        // MODIFICARE IMPORTANTA:
        // Căutăm tureta după TAG, nu după nume. 
        // Asta ajută când faci upgrade, pentru că numele se schimbă, dar Tag-ul rămâne "Turret".
        GameObject turretObj = GameObject.FindGameObjectWithTag("Turret");

        if (turretObj != null)
        {
            turret = turretObj.transform;
        }
    }

    void Update()
    {
        // Dacă tureta a dispărut (ex: în timpul upgrade-ului), încercăm să o găsim din nou
        if (turret == null)
        {
            GameObject t = GameObject.FindGameObjectWithTag("Turret");
            if (t != null) turret = t.transform;
            return;
        }

        // Mișcăm bănuțul către turetă
        transform.position = Vector3.MoveTowards(
            transform.position,
            turret.position,
            moveSpeed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Când atinge tureta
        if (other.CompareTag("Turret"))
        {
            // --- AICI AM REPARAT EROAREA ---
            // Folosim LevelManager în loc de CurrencyManager
            if (LevelManager.instance != null)
            {
                LevelManager.instance.AddCurrency(value);
            }

            // Distrugem bănuțul
            Destroy(gameObject);
        }
    }
}