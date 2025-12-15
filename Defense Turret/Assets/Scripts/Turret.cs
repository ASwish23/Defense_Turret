using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Attributes")]
    public int turretDamage = 1;
    public float range = 10f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public Transform firePoint;

    [Space(10)] // Doar pentru aspect în Inspector
    public GameObject bulletPrefab; // Drag Bullet Prefab here
    public GameObject rocketPrefab; // <--- NOU: Drag Rocket Prefab here (o să îl facem imediat)

    [Header("Weapon Unlocks")]
    public bool hasRockets = false; // Se activează din Shop

    // Variabile interne
    private Transform target;
    private int currentWeaponMode = 0; // 0 = Mitraliera, 1 = Rachete

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        // ... (Logica ta originală de căutare inamici - neschimbată) ...
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        // 1. --- LOGICA DE SCHIMBARE A ARMEI (INPUT) ---
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponMode = 0;
            Debug.Log("Arma: MITRALIERA");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hasRockets)
            {
                currentWeaponMode = 1;
                Debug.Log("Arma: RACHETE");
            }
            else
            {
                Debug.Log("LOCKED! Trebuie să cumperi rachetele din Shop.");
            }
        }

        if (target == null) return;

        // 2. --- ROTIREA TURETEI (Codul tău original) ---
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        partToRotate.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        // 3. --- TRAGEREA ---
        if (fireCountdown <= 0f)
        {
            Shoot();

            // Calculăm cadența de tragere în funcție de armă
            if (currentWeaponMode == 1) // Dacă suntem pe Rachete
            {
                // Rachetele trag de 2 ori mai încet decât mitraliera
                fireCountdown = 1f / (fireRate * 0.5f);
            }
            else // Dacă suntem pe Mitralieră
            {
                fireCountdown = 1f / fireRate;
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        // Alegem ce proiectil folosim și ce damage dăm
        GameObject prefabToUse = bulletPrefab;
        int currentDamage = turretDamage;

        if (currentWeaponMode == 1) // Mod Rachetă
        {
            prefabToUse = rocketPrefab;
            currentDamage = turretDamage * 4; // Racheta dă damage de 4 ori mai mare!
        }

        // Instanțiem proiectilul ales
        GameObject bulletGO = (GameObject)Instantiate(prefabToUse, firePoint.position, firePoint.rotation);

        // Asigură-te că și racheta va avea scriptul BulletMovement (sau unul similar)
        BulletMovement bullet = bulletGO.GetComponent<BulletMovement>();

        if (bullet != null)
        {
            bullet.damage = currentDamage;
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}