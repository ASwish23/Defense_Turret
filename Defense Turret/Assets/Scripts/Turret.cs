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

    // --- MODIFICARE: Folosim un ARRAY (listă) pentru a putea avea 1, 2 sau mai multe țevi ---
    public Transform[] firePoints;

    [Space(10)]
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;

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

        // 2. --- ROTIREA TURETEI ---
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
                // Rachetele trag mai încet
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
            currentDamage = turretDamage * 4;
        }

        // --- PARTEA NOUĂ: Tragem din TOATE punctele definite (1 sau 2) ---
        foreach (Transform fp in firePoints)
        {
            if (fp != null)
            {
                // Instanțiem proiectilul la punctul curent (fp)
                GameObject bulletGO = Instantiate(prefabToUse, fp.position, fp.rotation);

                // Setăm damage-ul și ținta
                BulletMovement bullet = bulletGO.GetComponent<BulletMovement>();
                if (bullet != null)
                {
                    bullet.damage = currentDamage; // Folosim variabila ta 'damage'
                    bullet.Seek(target);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}